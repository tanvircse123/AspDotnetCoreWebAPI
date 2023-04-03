using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Model.Dto;

namespace CityInfo.API.Profiles
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<CreatePOIViewModel,PointOdInterest>();
            CreateMap<City,CityDto>();
            CreateMap<City,CityWithOutPOIDto>();
            //CreateMap<PointOdInterest,CreatePOIViewModel>();
        }
    }
}