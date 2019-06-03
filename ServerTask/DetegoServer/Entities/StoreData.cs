using System;
using System.Collections.Generic;

namespace DetegoServer.Entities
{
    public class BaseStoreData
    {
        public int StoreId { get; set; }
        public int? BackstoreCount { get; set; }
        public int? FrontstoreCount { get; set; }
        public int? WindowCount { get; set; }
        public decimal? Accuracy { get; set; }
        public decimal? Availability { get; set; }
        public int? MeanAge { get; set; }
    }
    public partial class StoreData:BaseStoreData
    {
        public Stores Store { get; set; }
    }
}
