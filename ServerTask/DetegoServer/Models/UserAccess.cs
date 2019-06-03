using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DetegoServer.Models
{
    public class UserAccess
    {
        public int UserId { get; set; }
        public List<string> Grants { get; set; }
    }
}
