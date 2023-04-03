using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Entities;

namespace CityInfo.API.Model.Dto
{
    public class CityDto
    {
        public string Name {get;set;}
        
        public string? Description {get;set;}

        public int NumberOfPointsOfInterest{
            get
            {
                return PointOdInterests.Count;
            }
        }
        public ICollection<PointOdInterest> PointOdInterests {get;set;} = new List<PointOdInterest>();

    }
}