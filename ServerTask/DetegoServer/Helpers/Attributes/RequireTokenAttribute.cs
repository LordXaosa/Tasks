using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DetegoServer.Helpers.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class RequireTokenAttribute : Attribute
    {

    }
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class RequireFunctionAttribute : Attribute
    {
        public string RoleName { get; set; }
        public RequireFunctionAttribute(string roleName)
        {
            RoleName = roleName;
        }
    }
}
