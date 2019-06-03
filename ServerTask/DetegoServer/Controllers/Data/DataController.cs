using DetegoServer.Entities;
using DetegoServer.Helpers;
using DetegoServer.Helpers.Attributes;
using DetegoServer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace DetegoServer.Controllers.Data
{
    [Route("api/data")]
    [ApiController]
    [RequireToken]
    public class DataController : BaseTokenController
    {
        /// <summary>
        /// Method for getting info about stores and their stock with filter by conditions
        /// </summary>
        /// <param name="storeId">filter stock id</param>
        /// <param name="countryCode">filter country code</param>
        /// <param name="storeName">filter stock name</param>
        /// <param name="managerFirstName">filter manager first name</param>
        /// <param name="managerLastName">filter manager last name</param>
        /// <param name="categories">filter categories, could be multiple</param>
        /// <returns></returns>
        [RequireFunction("Reader")]
        [HttpGet("stores")]
        public IEnumerable<VStockInfo> GetStockData(int? storeId, string countryCode, string storeName, string managerFirstName, string managerLastName, List<int> categories)
        {
            using (DB db = new DB())//I could pass it like service.AddDbContext<DB>(), but this way seems to me more obvious
            {
                var query = db.VStockInfo.AsQueryable();
                if (storeId.HasValue)
                    query = query.Where(p => p.Id == storeId.Value);
                if (!string.IsNullOrWhiteSpace(countryCode))
                    query = query.Where(p => p.CountryCode.Contains(countryCode));
                if (!string.IsNullOrWhiteSpace(storeName))
                    query = query.Where(p => p.StoreName.Contains(storeName));
                if (!string.IsNullOrWhiteSpace(managerFirstName))
                    query = query.Where(p => p.StoreMgrFirstName.Contains(managerFirstName));
                if (!string.IsNullOrWhiteSpace(managerLastName))
                    query = query.Where(p => p.StoreMgrLastName.Contains(managerLastName));
                if (categories?.Count > 0)
                    query = query.Where(p => categories.Contains(p.Category));
                return query.ToList();
            }
        }
        /// <summary>
        /// Get info about specific store and it's stock
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [RequireFunction("Reader")]
        [HttpGet("store/{id}")]
        public VStockInfo GetStockData(int id)
        {
            return GetStockData(id, null, null, null, null, null).Single(); //if many - something went wrong, like constraints in DB were removed
        }
        /// <summary>
        /// Method to edit stock info of the store
        /// </summary>
        /// <param name="id">store id</param>
        /// <param name="storeData">stock info of the store</param>
        /// <returns>if true - edit was successfull</returns>
        [RequireFunction("Writer")]
        [HttpPost("store/{id}/stock")]
        public bool EditStockInfo([FromRoute] int id, [FromBody] BaseStoreData storeData)//also there could be patch method, just to edit single column, but I don't really like this approach
        {
            using (DB db = new DB())
            {
                var store = db.Stores.FirstOrDefault(p => p.Id == id);
                if (store == null)
                    throw new NotStoredException("Store not found");

                storeData.StoreId = store.Id;//just to be sure if id in path is the same that in body, to avoid fake editing
                if (store.StoreData == null)
                    store.StoreData = new StoreData();
                store.StoreData.Accuracy = storeData.Accuracy;
                store.StoreData.Availability = storeData.Availability;
                store.StoreData.BackstoreCount = storeData.BackstoreCount;
                store.StoreData.FrontstoreCount = storeData.FrontstoreCount;
                store.StoreData.WindowCount = storeData.WindowCount;
                store.StoreData.StoreId = storeData.StoreId;
                store.StoreData.MeanAge = storeData.MeanAge;
                db.SaveChanges();
                return true;
            }
        }
        /// <summary>
        /// Method to put new store data
        /// </summary>
        /// <param name="storeInfo">store data</param>
        /// <returns>store data with new id</returns>
        [RequireFunction("Writer")]
        [HttpPut("store")]
        public Stores AddStore([FromBody] BaseStore storeInfo)//I could return only new id, but for example I return whole object
        {
            using (DB db = new DB())
            {
                Stores store = new Stores();
                store.StoreEmail = storeInfo.StoreEmail;
                store.StoreMgrEmail = storeInfo.StoreMgrEmail;
                store.StoreMgrFirstName = storeInfo.StoreMgrFirstName;
                store.StoreMgrLastName = storeInfo.StoreMgrLastName;
                store.StoreName = storeInfo.StoreName;
                store.Category = storeInfo.Category;
                store.CountryCode = storeInfo.CountryCode;
                var result = db.Stores.Add(store);
                db.SaveChanges();
                return result.Entity;
            }
        }
        /// <summary>
        /// Method for editing store info
        /// </summary>
        /// <param name="id">id of the store</param>
        /// <param name="storeInfo">new data to update</param>
        /// <returns></returns>
        [RequireFunction("Writer")]
        [HttpPost("store/{id}")]
        public bool EditStoreInfo([FromRoute] int id, [FromBody] BaseStore storeInfo)
        {
            using (DB db = new DB())
            {
                var store = db.Stores.FirstOrDefault(p => p.Id == id);
                if (store == null)
                    throw new NotStoredException("Store not found");

                store.StoreEmail = storeInfo.StoreEmail;//regex for valid email format is on db side
                store.StoreMgrEmail = storeInfo.StoreMgrEmail;
                store.StoreMgrFirstName = storeInfo.StoreMgrLastName;
                store.StoreName = storeInfo.StoreName;
                store.Category = storeInfo.Category;
                store.CountryCode = storeInfo.CountryCode;

                db.SaveChanges();//I intentionally not check for nulls because it would be done by database, but if we unsure there could be checks
                return true;
            }
        }
        
    }
}