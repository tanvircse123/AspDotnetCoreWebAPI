using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using CityInfo.API.Controllers.Base;
using CityInfo.API.Entities;
using CityInfo.API.Model.Dto;
using CityInfo.API.Services.CityRepository;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    public class CitiesController:BaseApi
    {
        private ICityInfoRepository _repo;
        private IMapper _mapper;
        const int MaxCitiesPageSize = 20;

        public CitiesController(ICityInfoRepository repo,IMapper mapper)
       {
            _repo = repo;
            _mapper = mapper;
       }

       [HttpPost]
       public ActionResult AddCity(CreateCityViewModel cityreq){
            var city = new City(cityreq.Name);
            city.Description = cityreq.Description;
            var POI = _mapper.Map<IEnumerable<PointOdInterest>>(cityreq.PointOfInterests);
            var contextModel = new AddCityContextViewModel{
                city = city,
                POI = POI
            };
            Console.WriteLine(JsonSerializer.Serialize(contextModel));
            _repo.AddCity(contextModel);
            return Ok("Added");

            

       }

    }
}