using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Entities.Base;

namespace CityInfo.API.Entities
{
    public class PointOdInterest:BaseEntity
    {
        public PointOdInterest(string name)
        {   
            this.Name = name;
        }

        [Required]
        [MaxLength(50)]
        public string Name {get;set;}

        [MaxLength(200)]
        public string? Description {get;set;}

        [ForeignKey("CityId")]
        public City? City {get;set;}
        public int CityId {get;set;}
    }
}