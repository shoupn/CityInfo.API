using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.Models
{
    public class PointsOfInterestUpdateDto
    {
        [Required(ErrorMessage = "Provide a valid name")]
        [MaxLength(50)]
        public string Name { get; set; }


        [Required(ErrorMessage = "Provide a valid description")]
        [MaxLength(200)]
        public string Description { get; set; }
    }
}
