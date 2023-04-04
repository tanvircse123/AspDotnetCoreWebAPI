using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CityInfo.API.Services.CityRepository;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities/{cityId}/pointofinterest")]
    public class PointsOfinterestController: ControllerBase
    {
        private ICityInfoRepository _repo;
        private IMapper _mapper;

        public PointsOfinterestController(ICityInfoRepository repo,IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult> GetPointsOfInterest(int cityId){
            try{
                // try if the city exists or not
                var city = await _repo.GetCityAsync(cityId,false);
                if(city == null){
                    return NotFound("City Not Found");
                }
                // if exists then 
                var POI = await _repo.GetPointOfInterestForCityAsync(cityId);
                return Ok(POI);
            }catch{
                return BadRequest("Something Is Wrong");
            }
        }
        [HttpGet("{POIId}")]
        public async Task<ActionResult> GetPointOfInterestById(int cityId,int POIId){
            try{
                var city = await _repo.GetCityAsync(cityId,false);
                if(city == null){
                    return NotFound("City Not Found");
                }
                var POI = await _repo.GetPointOfInterestForCityAsync(cityId,POIId);
                return Ok(POI);


            }catch{
                return BadRequest("Something Is Wrong");
            }
        }


        
    }
}