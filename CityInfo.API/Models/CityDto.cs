using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Models
{
    public class CityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }


        public List<PointOfInterestDto> PointsOfInterest { get; set; }
            = new List<PointOfInterestDto>(); //using C# 6 new auto initialize property

        public int NumberOfPointsOfInterest
        {
            get
            {   //set number from the above property
                return PointsOfInterest.Count;
            }
        }
    }
}
