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
        //public City? City {get;set;} // dont put it in here it will create a circuler dependency 
        // from city it will come to here and from here it will go again in the city thus creating a 
        // circular dependency
        public int CityId {get;set;}
    }
}