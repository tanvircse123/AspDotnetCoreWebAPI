using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Model.Dto
{
    public class CreateCityViewModel
    {
        public string Name {get;set;}
        public string? Description {get;set;}

        public ICollection<CreatePOIViewModel> PointOfInterests {get;set;}
    }
}