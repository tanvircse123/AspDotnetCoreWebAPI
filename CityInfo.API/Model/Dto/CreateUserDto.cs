using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Model.Dto
{
    public class CreateUserDto
    {
        [Required]
        public string UserName { get; set; }
        public string? FirstName {get;set;}
        public string? LastName {get;set;}
        public string? City {get;set;}
        [Required]
        public string Password {get;set;}
    }
}