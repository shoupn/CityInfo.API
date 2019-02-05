using CityInfo.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API
{
    public static class CityInfoContextExtensions
    {
        public static void EnsureSeedDataForContext(this CityInfoContext context)
        {
            if (context.Cities.Any())
            {
                return;
            }
            else
            {
               var cities = new List<City>()
            {
                new City()
                {
                    Name = "New York",
                    Description = "Has a big park",
                    PointsOfInterest = new List<PointOfInterest>()
                    {
                        new PointOfInterest()
                        {
                            Name = "Central Park",
                            Description = " Well Liked Park"
                        }
                    }
                },
                new City()
                {
                    Name = "Antwerp",
                    Description = "Has a big cathedral that was never finished",
                    PointsOfInterest = new List<PointOfInterest>()
                    {
                        new PointOfInterest()
                        {
                            Name = "Central Park",
                            Description = " Well Liked Park"
                        }
                    }
                },
                new City()
                {
                    Name = "Paris",
                    Description = "Has a big tower",
                    PointsOfInterest = new List<PointOfInterest>()
                    {
                        new PointOfInterest()
                        {
                            Name = "Central Park",
                            Description = " Well Liked Park"
                        }
                    }
                }

            };
                context.Cities.AddRange(cities);
                context.SaveChanges();
            }
        }
    }
}
