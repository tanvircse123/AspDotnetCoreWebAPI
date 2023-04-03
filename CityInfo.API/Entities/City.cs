using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Entities.Base;

namespace CityInfo.API.Entities
{
    public class City:BaseEntity
    {
        public City(string name)
        {
            this.Name = name;
            this.PointOdInterests = new List<PointOdInterest>();
        }
        [Required]
        [MaxLength(50)]
        public string Name {get;set;}
        [MaxLength(200)]
        public string? Description {get;set;}

        public ICollection<PointOdInterest> PointOdInterests {get;set;}

    }
}