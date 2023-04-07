using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Model.Dto;
using CityInfo.API.Services.CityRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch.Adapters;
namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities/{cityId}/pointofinterest")]
    public class PointsOfinterestController : ControllerBase
    {
        private ICityInfoRepository _repo;
        private IMapper _mapper;

        public PointsOfinterestController(ICityInfoRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult> GetPointsOfInterest(int cityId)
        {
            try
            {
                // try if the city exists or not
                var city = await _repo.GetCityAsync(cityId, false);
                if (city == null)
                {
                    return NotFound("City Not Found");
                }
                // if exists then 
                var POI = await _repo.GetPointOfInterestForCityAsync(cityId);
                return Ok(POI);
            }
            catch
            {
                return BadRequest("Something Is Wrong");
            }
        }


        [HttpGet("{POIId}", Name = "GetPointOfInterest")]
        public async Task<ActionResult> GetPointOfInterestById(int cityId, int POIId)
        {
            try
            {
                var city = await _repo.GetCityAsync(cityId, false);
                if (city == null)
                {
                    return NotFound("City Not Found");
                }
                var POI = await _repo.GetPointOfInterestForCityAsync(cityId, POIId);
                if (POI == null)
                {
                    return NotFound("POI Not Found");
                }
                return Ok(_mapper.Map<PointOfInterestDto>(POI));


            }
            catch
            {
                return BadRequest("Something Is Wrong");
            }
        }

        // we create a post request to add data
        // most of the time this would be void but
        // in this case after posting it 
        // we give newly created data access route
        [HttpPost]
        public async Task<ActionResult> AddPointOfInterest(int cityId, POICreationDto pOICreationDto)
        {
            // first check if the city exists or not
            if (!await _repo.CityExistsAsync(cityId))
            {
                // if not exists
                return NotFound("City Not Found");
            }
            // map to domain model for creation
            var finalpointofInterest = _mapper.Map<PointOdInterest>(pOICreationDto);
            // adding the point of interest
            await _repo.AddPointOfInterestForCityAsync(cityId, finalpointofInterest);
            await _repo.SaveChangesAsync();// redundant
            // again map the saved entity to the response dto
            var just_created_POI_return = _mapper.Map<PointOfInterestDto>(finalpointofInterest);
            return CreatedAtRoute("GetPointOfInterest",
            new
            {
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
        [HttpPut("{POIId}")]
        public async Task<ActionResult> UpdatePointOfInterest(int cityId, int POIId, POIUpdateDto pOIUpdateDto)
        {
            // check if the city exists
            if (!await _repo.CityExistsAsync(cityId))
            {
                return NotFound("City NotFound");
            }
            // if found
            var POI = await _repo.GetPointOfInterestForCityAsync(cityId, POIId);
            if (POI == null)
            {
                return NotFound("POI Not Found");
            }

            // update with mapper
            _mapper.Map(pOIUpdateDto, POI);
            await _repo.SaveChangesAsync();
            return NoContent();


        }

        [HttpPatch("{POIId}")]
        public async Task<ActionResult> UpdatePointOfInterest(int cityId, int POIId, JsonPatchDocument<POIUpdateDto> patchDocument){
            // find the city
            // in order to work patch document work
            // you need to also add newtonsoft.json package
            // and add it ti Program.cs
            // without it it wont work 

            // patch document like this
            /*
            [
	{
		"op":"replace",
		"path":"/name",
		"value":"Padma"
	},
	{
		"op":"replace",
		"path":"/description",
		"value":"Padma river is the biggest river"
	}
]
            
            */




            if(!await _repo.CityExistsAsync(cityId)){
                return NotFound("City Not Found");
            }
            var POI = await _repo.GetPointOfInterestForCityAsync(cityId, POIId);
            if (POI == null)
            {
                return NotFound("POI Not Found");
            }   
            

            // we will omit the ID so we assign the data to a dto
            var POIToPatch = new POIUpdateDto(){
                Name = POI.Name,
                Description  =  POI.Description
            };

            patchDocument.ApplyTo(POIToPatch,ModelState);        
            // you apply the patch document to POIToPatch
            // now you need to do one more mappin g to the 
            // endtity model
            
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            // after the patch if the model stil valid 
            // like in patch you completely remove the name
            // that is  a required that will check here
            if(!TryValidateModel(POIToPatch)){
                return BadRequest(ModelState);
            }


            _mapper.Map(POIToPatch,POI);
            await _repo.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{POIId}")]
        public async Task<ActionResult> DeletePointOfInterest(int cityId, int POIId)
        {
            if(!await _repo.CityExistsAsync(cityId)){
                return NotFound("City Not Found");
            }
            var POI = await _repo.GetPointOfInterestForCityAsync(cityId, POIId);
            if(POI == null){
                return NotFound("POI Not Found");
            }

            _repo.DeletePointOfInterest(POI);
            await _repo.SaveChangesAsync();
            return NoContent();
        }
        
    }
}