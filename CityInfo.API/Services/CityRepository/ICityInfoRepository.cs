using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Entities;
using CityInfo.API.Model;
using CityInfo.API.Model.Dto;

namespace CityInfo.API.Services.CityRepository
{
    public interface ICityInfoRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync(); // get all the cities 
        Task<(IEnumerable<City>,PaginationMetadata)> GetCitiesAsync( 
            // this one is the overload based on the name and search
            // query and also the pagination informatin returns a tuple
            // od the city list and the pagination information

            string? name,
            string? searchQuery,
            int pageNumber,
            int pageSize
            );
        Task<bool> CityNameMatchesCityId(string? cityName,int cityId);
        Task<City?> GetCityAsync(int cityId,bool includePointsOfInterest);
        Task<bool> CityExistsAsync(int cityId);
        Task<PointOdInterest?> GetPointOfInterestForCityAsync(int cityId,int pointOfInterest);
        Task<IEnumerable<PointOdInterest>> GetPointOfInterestForCityAsync(int cityId);
        void AddCity(AddCityContextViewModel cityContextViewModel);
        Task AddPointOfInterestForCityAsync(int cityId,PointOdInterest pointOdInterest);
        void DeletePointOfInterest(PointOdInterest pointOdInterest);
        Task<bool> SaveChangesAsync(); 
    }
}