using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Model.Dto;
using CityInfo.API.Services.CityRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController:ControllerBase
    {
        private ICityInfoRepository _repo;
        private IMapper _mapper;
        const int MaxCitiesPageSize = 20;

        public CitiesController(ICityInfoRepository repo,IMapper mapper)
       {
            _repo = repo;
            _mapper = mapper;
       }

       [Authorize]
       [HttpGet]

       public async Task<ActionResult> GetCities(string? name,string? searchQuery,int pageNumber = 1,int pageSize = 10){
        if (pageSize > MaxCitiesPageSize)
        {
            pageSize = MaxCitiesPageSize;
        }
        var (cityEntities,paginatinMatadata) = await _repo.GetCitiesAsync(name,searchQuery,pageNumber,pageSize);
        Response.Headers.Add("X-Pagination",JsonSerializer.Serialize(paginatinMatadata));
        var claims = HttpContext.User.Claims; // get all the claims the user have
        var username = HttpContext.User.FindFirst("Name").Value; // get the username by asking for Name as a the claim created
        // you can use thsi username and find the user by qiering with user table
        Console.WriteLine(username);
        
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

       [HttpGet("{Id}")]
       [ProducesResponseType(StatusCodes.Status200OK)]
       [ProducesResponseType(StatusCodes.Status404NotFound)]
       [ProducesResponseType(StatusCodes.Status400BadRequest)]
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