using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DetegoServer.Models
{
    public class StoreDataByCategory
    {
        public int Category { get; set; }
        public int? TotalStock { get; set; }
        public decimal? Accuracy { get; set; }
        public decimal? Availability { get; set; }
        public double? MeanAge { get; set; }
    }
}
