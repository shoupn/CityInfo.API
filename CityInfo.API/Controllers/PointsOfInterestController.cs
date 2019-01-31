using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.MailService;
using CityInfo.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")] //decorate the class with the initial part of URI
    public class PointsOfInterestController : Controller
    {
        private ILogger<PointsOfInterestController> _logger;
        private IMailService _mailSerice;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, 
            IMailService mailService)
        {
            _logger = logger;
            _mailSerice = mailService;
        }

        [HttpGet("{cityid}/pointsofinterest")]
        public IActionResult GetPointsOfInterest(int cityid)
        {
            try
            {
                var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityid);
                if (city == null)
                {
                    _logger.LogInformation($"City with cityId {cityid} was not found in points of interest");
                    return NotFound();
                }
                return Ok(city.PointsOfInterest);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.ToString());
                return StatusCode(500);
            } 
        }

        [HttpGet("{cityId}/pointsofinterest/{pointId}", Name = "GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int pointId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            var point = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointId);
            if (point == null)
            {
                return NotFound();
            }
            return Ok(point);
        }

        [HttpPost("{cityId}/pointsofinterest")]
        public IActionResult CreatePointOfInterest(int cityId, [FromBody]PointOfInterestCreationDto pointOfInterest)
        {
            if(pointOfInterest == null)
            {
                return BadRequest();
            }

            if(pointOfInterest.Name == pointOfInterest.Description)
            {
                ModelState.AddModelError("Description", "Description must be different than Name");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            int maxId = CitiesDataStore.Current.Cities.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);

            var newPointOfInterest = new PointsOfInterestDto()
            {
                Id = ++maxId,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };

            city.PointsOfInterest.Add(newPointOfInterest);
            return CreatedAtRoute("GetPointOfInterest", 
                new { cityId = city.Id, pointId = newPointOfInterest.Id }, newPointOfInterest);
        }


        [HttpPut("{cityId}/pointsofinterest/{pointId}")]
        //pass in the pointOfinterestBody to method signature
        public IActionResult UpdatePointOfInterest(int cityId, int pointId,
            [FromBody]PointOfInterestCreationDto pointOfInterest)
        {
            if (pointOfInterest == null)
            {
                return BadRequest();
            }

            if (pointOfInterest.Name == pointOfInterest.Description)
            {
                ModelState.AddModelError("Description", "Description must be different than Name");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointOfinterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointId);
            if (pointOfinterestFromStore == null)
            {
                return NotFound();
            }
            pointOfinterestFromStore.Name = pointOfInterest.Name;
            pointOfinterestFromStore.Description = pointOfInterest.Description;

            return NoContent();
        }

        [HttpPatch("{cityId}/pointsofinterest/{pointId}")]
        public IActionResult PartiallyUpdatePointOfInterest(int cityId, int pointId, 
            [FromBody]JsonPatchDocument<PointsOfInterestUpdateDto> patchDocument)
        {
            if(patchDocument == null)
            {
                return BadRequest();
            }


            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointOfinterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointId);
            if (pointOfinterestFromStore == null)
            {
                return NotFound();
            }

            var pointOfInteresetToPatch =
                new PointsOfInterestUpdateDto()
                {
                    Name = pointOfinterestFromStore.Name,
                    Description = pointOfinterestFromStore.Description
                };

            patchDocument.ApplyTo(pointOfInteresetToPatch, ModelState);


            if (pointOfInteresetToPatch.Name == pointOfInteresetToPatch.Description)
            {
                ModelState.AddModelError("Description", "Description must be different than Name");
            }

            TryValidateModel(pointOfInteresetToPatch);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            pointOfinterestFromStore.Name = pointOfInteresetToPatch.Name;
            pointOfinterestFromStore.Description = pointOfInteresetToPatch.Description;

            return NoContent();
        }

        [HttpDelete("{cityId}/pointsofinterest/{pointId}")]

        public IActionResult DeletePointOfIntrerest(int cityId, int pointId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointOfinterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointId);
            if (pointOfinterestFromStore == null)
            {
                return NotFound();
            }

            city.PointsOfInterest.Remove(pointOfinterestFromStore);
            _mailSerice.Send("Point Of Interest Deleted",
                $"Point of Interest: {pointOfinterestFromStore.Name} with ID {pointOfinterestFromStore.Id} was deleted");
            return NoContent();
        }
    }
}


/*
//Use Post for Creating a Resource
201 Created 
Header- Content Type

Validation
-DataAnnotations
-ModelState
*/
