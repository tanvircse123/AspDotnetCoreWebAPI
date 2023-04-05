using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Model.Dto;
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


        [HttpGet("{POIId}" ,Name ="GetPointOfInterest")]
        public async Task<ActionResult> GetPointOfInterestById(int cityId,int POIId){
            try{
                var city = await _repo.GetCityAsync(cityId,false);
                if(city == null){
                    return NotFound("City Not Found");
                }
                var POI = await _repo.GetPointOfInterestForCityAsync(cityId,POIId);
                if(POI == null){
                    return NotFound("POI Not Found");
                }
                return Ok(_mapper.Map<PointOfInterestDto>(POI));


            }catch{
                return BadRequest("Something Is Wrong");
            }
        }

        // we create a post request to add data
        // most of the time this would be void but
        // in this case after posting it 
        // we give newly created data access route
        [HttpPost]
        public async Task<ActionResult> AddPointOfInterest(int cityId,POICreationDto pOICreationDto){
            // first check if the city exists or not
            if(!await _repo.CityExistsAsync(cityId)){
                // if not exists
                return NotFound("City Not Found");
            }
            // map to domain model for creation
            var finalpointofInterest = _mapper.Map<PointOdInterest>(pOICreationDto);
            // adding the point of interest
            await _repo.AddPointOfInterestForCityAsync(cityId,finalpointofInterest);
            await _repo.SaveChangesAsync();// redundant
            // again map the saved entity to the response dto
            var just_created_POI_return = _mapper.Map<PointOfInterestDto>(finalpointofInterest);
            return CreatedAtRoute("GetPointOfInterest",
            new {
                cityId = cityId,
                POIId = just_created_POI_return.Id
                },
            just_created_POI_return);

            //  how CreatedAtRoute work and what the parameter are doing what
            //  first you need a name of a method that will do get request
            //  dont worry what the method return now
            // then you need to give the parameter value of the get method
            // as a form of annonymous class
            // then you need to provide the new just_creted for return
            // when you do that the "name" will find the method
            // and annonymous class with value will fill the parameter
            // and automatically create a url for that in the "HEADER"
            // in the header not the body
            // in the body you will get the newly created value


        
    }
    // [HttpPut("{POIId}")]
    // public async Task<ActionResult> UpdatePointOfInterest(int cityId,int POIId,POIUpdateDto updatemodel){
    //     if(_repo)
    // }
  }
}