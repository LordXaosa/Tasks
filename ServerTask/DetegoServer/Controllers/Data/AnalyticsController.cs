using DetegoServer.Entities;
using DetegoServer.Helpers.Attributes;
using DetegoServer.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DetegoServer.Controllers.Data
{
    [Route("api/analytics")]
    [ApiController]
    [RequireToken]
    public class AnalyticsController:BaseTokenController
    {
        /// <summary>
        /// Method for statistical data, aggregating sum about stock info and groupped by countries
        /// </summary>
        /// <param name="countryName">country filter if needed</param>
        /// <returns></returns>
        [RequireFunction("Reader")]
        [HttpGet("countries")]
        public object GetStatsByCountries(string countryName)//object is just for example, that we can response with anonymous types, but it's not really good because it won't be at swagger documentation
        //also I haven't done custom parser of string arrays, but it can be easily done in Helpers/CustomBinderProvider.cs
        {
            using (DB db = new DB())
            {
                var query = db.VStockInfo.AsQueryable();
                if (!string.IsNullOrWhiteSpace(countryName))
                    query = query.Where(p => p.CountryCode.Contains(countryName));
                return query.GroupBy(p => p.CountryCode).Select(p => new //if we need some custom calculations, made on backend side, we should get data from DB via .ToList method or others that make IEnumerable and then make selection with custom functions
                {
                    countryCode = p.Key,
                    totalStock = p.Sum(s => s.FrontstoreCount + s.BackstoreCount + s.WindowCount)??0,
                    stockAccuracy = p.Average(s => s.Accuracy) ?? 0,
                    stockAvailability = p.Average(s => s.Availability) ?? 0,
                    stockMeanAge = p.Average(s => s.MeanAge) ?? 0
                }).OrderBy(p => p.countryCode).ThenByDescending(p => p.totalStock).ThenByDescending(p => p.stockAccuracy).ThenByDescending(p => p.stockAvailability).ThenByDescending(p => p.stockMeanAge).ToList();
            }
        }
        /// <summary>
        /// Method for store analitycs
        /// </summary>
        /// <returns></returns>
        [RequireFunction("Reader")]
        [HttpGet("stores")]
        public object GetStatsByStores()
        {
            using (DB db = new DB())
            {
                return db.VStockInfo.Select(p => new //if we need some custom calculations, made on backend side, we should get data from DB via .ToList method or others that make IEnumerable and then make selection with custom functions
                {
                    p.CountryCode,
                    p.Category,
                    p.StoreEmail,
                    p.StoreMgrEmail,
                    p.StoreMgrFirstName,
                    p.StoreMgrLastName,
                    p.StoreName,
                    totalStock = (p.FrontstoreCount + p.BackstoreCount + p.WindowCount)??0,
                    stockAccuracy = p.Accuracy ?? 0,
                    stockAvailability = p.Availability?? 0,
                    stockMeanAge = p.MeanAge ?? 0
                }).OrderBy(p => p.CountryCode).ThenByDescending(p => p.totalStock).ThenByDescending(p => p.stockAccuracy).ThenByDescending(p => p.stockAvailability).ThenByDescending(p => p.stockMeanAge).ToList();
            }
        }
        /// <summary>
        /// Method for statistical data, aggregating sum about stock info and groupped by categories
        /// </summary>
        /// <param name="categories">categories filter</param>
        /// <returns></returns>
        [RequireFunction("Reader")]
        [HttpGet("categories")]
        public IEnumerable<StoreDataByCategory> GetStatsByCategories(List<int> categories)//strong typed response, that will be displayed at swagger docs
        {
            using (DB db = new DB())
            {
                var query = db.VStockInfo.AsQueryable();
                if (categories?.Count > 0)
                    query = query.Where(p => categories.Contains(p.Category));

                return query.GroupBy(p => p.Category).Select(p => new StoreDataByCategory
                {
                    Category = p.Key,
                    TotalStock = p.Sum(s => s.FrontstoreCount + s.BackstoreCount + s.WindowCount) ?? 0,
                    Accuracy = p.Average(s => s.Accuracy) ?? 0,
                    Availability = p.Average(s => s.Availability) ?? 0,
                    MeanAge = p.Average(s => s.MeanAge) ?? 0
                }).ToList();
            }
        }
    }
}
