using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")] //decorate the class with the initial part of URI
    public class PointsOfInterestController : Controller
    {
        [HttpGet("{cityid}/pointsofinterest")]
        public IActionResult GetPointsOfInterest(int cityid)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityid);
            if(city == null)
            {
                return NotFound();
            }

            return Ok(city.PointsOfInterest);
        }

        [HttpGet("{cityid}/pointsofinterest/{pointId}")]
        public IActionResult GetPointOfInterest(int cityid, int pointId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityid);
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
    }
}