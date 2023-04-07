using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Entities
{
    public class User
    {
        [Key]
        public int UserId {get;set;}
        [Required]
        [MaxLength(15)]
        public string UserName { get; set; }
        public string? FirstName {get;set;}
        public string? LastName {get;set;}
        public string? City {get;set;}
        [Required]
        public string Password {get;set;}
        
        // when you do migration do not keep the constructor
        // add the constructor after the migration done
        
        // public User(string username,string password)
        // {
        //     UserName = username;
        //     Password = password;

        // }
        
    }
}