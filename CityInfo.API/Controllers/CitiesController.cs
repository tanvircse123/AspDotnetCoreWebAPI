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

       [HttpGet]
       public async Task<ActionResult> GetCities(string? name,string? searchQuery,int pageNumber = 1,int pageSize = 10){
        if (pageSize > MaxCitiesPageSize)
        {
            pageSize = MaxCitiesPageSize;
        }
        var (cityEntities,paginatinMatadata) = await _repo.GetCitiesAsync(name,searchQuery,pageNumber,pageSize);
        Response.Headers.Add("X-Pagination",JsonSerializer.Serialize(paginatinMatadata));
        return Ok(cityEntities);

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
           
            _repo.AddCity(contextModel);
            return Ok("Added");
       }

       [HttpGet("getCity/{Id}")]
       public async Task<ActionResult> GetCityAsyncFull(int Id,bool includePointsOfInterest = false){
        var city = await _repo.GetCityAsync(Id,includePointsOfInterest);
        if(city == null){
            return NotFound();
        }
        if(includePointsOfInterest){
            return Ok(_mapper.Map<CityDto>(city));
        }
        return  Ok(_mapper.Map<CityWithOutPOIDto>(city));

        
       }

    }
}