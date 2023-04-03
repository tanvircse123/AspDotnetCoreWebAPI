using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Entities;

namespace CityInfo.API.Model.Dto
{
    public class AddCityContextViewModel
    {
        public City city {get;set;}
        public IEnumerable<PointOdInterest> POI {get;set;}
    }
}