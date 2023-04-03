using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CityInfo.API.Data;
using CityInfo.API.Entities;
using CityInfo.API.Model;
using CityInfo.API.Model.Dto;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Services.CityRepository
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private CityInfoContext _context;

        public CityInfoRepository(CityInfoContext context)
        {
            _context = context;
        }

        public void AddCity(AddCityContextViewModel cityContextViewModel)
        {
            using(var transaction = _context.Database.BeginTransaction()){
                try{
                 _context.Cities.Add(cityContextViewModel.city);
                 _context.SaveChanges();
                var its = new  List<PointOdInterest>();
                foreach(var item in cityContextViewModel.POI){
                    item.CityId = cityContextViewModel.city.Id;
                    its.Add(item);
                }
                 
                 _context.PointOdInterests.AddRange(its);
                 _context.SaveChanges();
                 transaction.Commit();


                

            }catch{
                throw;
            }
    
            }
            
            
        }

        public async Task AddPointOfInterestForCityAsync(int cityId, PointOdInterest pointOdInterest)
        {
            try{
                var city = await GetCityAsync(cityId,false);
                if(city != null){
                    city.PointOdInterests.Add(pointOdInterest);
                }

            }catch{
                throw;
            }
        }

        public async Task<bool> CityExistsAsync(int cityId)
        {
            try{

                return await _context.Cities.AnyAsync(s=>s.Id == cityId);

            }catch{
                throw;
            }
        }

        public async Task<bool> CityNameMatchesCityId(string? cityName, int cityId)
        {
            try{
                var isavailable = await _context.Cities.AnyAsync(s=>s.Id == cityId && s.Name == cityName);
                return isavailable;
            }catch{
                throw;
            }
             
        }

        public void DeletePointOfInterest(PointOdInterest pointOdInterest)
        {
            try{
                _context.PointOdInterests.Remove(pointOdInterest);
            }catch{
                throw;
            }
        }

        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            try{
                var cities = await _context.Cities.OrderBy(x=>x.Name).ToListAsync();
                return cities;

            }catch{
                throw;
            }
        }

        public async Task<(IEnumerable<City>, PaginationMetadata)> GetCitiesAsync(
            string? name,
            string? searchQuery,
            int pageNumber, 
            int pageSize
            )
        {
            try{

                var collection = _context.Cities.AsQueryable<City>();
                if(!string.IsNullOrWhiteSpace(name)){
                    name = name.Trim();
                    collection = collection.Where(s=>s.Name == name);
                }
                if(!string.IsNullOrWhiteSpace(searchQuery)){
                    searchQuery = searchQuery.Trim();
                    collection = collection.Where(a=>a.Name.Contains(searchQuery)|| (a.Description !=null && a.Description.Contains(searchQuery)));

                }
                var totalItemCount = await collection.CountAsync();
                var paginationMetadata = new PaginationMetadata(totalItemCount,pageSize,pageNumber);
                var collectionReturn = await collection.OrderBy(s=>s.Name).Skip(pageSize*(pageNumber -1)).Take(pageSize).ToListAsync();
                return (collectionReturn,paginationMetadata);

            }catch{
                throw;
            }
        }

        public async Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest)
        {
            try{
                if(includePointsOfInterest){
                    return await _context.Cities
                    .Include(s=>s.PointOdInterests)
                    .Where(s=>s.Id == cityId)
                    .FirstOrDefaultAsync();
                    
                }
                return await _context.Cities
                    .Where(s=>s.Id == cityId)
                    .FirstOrDefaultAsync();


            }catch{
                throw;
            }
        }

        public async Task<PointOdInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId)
        {
            try{
                return await _context.PointOdInterests
                .Where(s=>s.CityId == cityId && s.Id == pointOfInterestId)
                .FirstOrDefaultAsync();

            }catch{
                throw;
            }
        }

        public async Task<IEnumerable<PointOdInterest>> GetPointOfInterestForCityAsync(int cityId)
        {
            try{
                return await _context.PointOdInterests
                .Where(s=>s.CityId == cityId).ToListAsync();
            }catch{
                throw;
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}