using System;
using System.Collections.Generic;

namespace DetegoServer.Entities
{
    public class BaseStore
    {
        public int Id { get; set; }
        public string CountryCode { get; set; }
        public string StoreName { get; set; }
        public string StoreEmail { get; set; }
        public string StoreMgrFirstName { get; set; }
        public string StoreMgrLastName { get; set; }
        public string StoreMgrEmail { get; set; }
        public int Category { get; set; }
    }
    public partial class Stores:BaseStore
    {
        public StoreData StoreData { get; set; }
    }
}
