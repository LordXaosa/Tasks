using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DetegoServer.Entities
{
    public class VStockInfo
    {
        public int Id { get; set; }
        public string CountryCode { get; set; }
        public string StoreName { get; set; }
        public string StoreEmail { get; set; }
        public string StoreMgrFirstName { get; set; }
        public string StoreMgrLastName { get; set; }
        public string StoreMgrEmail { get; set; }
        public int Category { get; set; }
        public int? BackstoreCount { get; set; }
        public int? FrontstoreCount { get; set; }
        public int? WindowCount { get; set; }
        public decimal? Accuracy { get; set; }
        public decimal? Availability { get; set; }
        public int? MeanAge { get; set; }
    }
}
