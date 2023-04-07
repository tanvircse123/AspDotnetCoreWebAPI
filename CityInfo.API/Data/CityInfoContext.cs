using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Data
{
    public class CityInfoContext:DbContext
    {
        public CityInfoContext(DbContextOptions<CityInfoContext> options):base(options)
        {
        }

        public DbSet<City>?Cities {get;set;}
        public DbSet<PointOdInterest>?PointOdInterests {get;set;}
        public DbSet<User>?Users {get;set;}
    }
}