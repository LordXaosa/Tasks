using DetegoServer.Helpers;
using DetegoServer.Helpers.Attributes;
using DetegoServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace DetegoServer.Controllers
{
    public class BaseTokenController : Controller
    {
        public UserAccess UserAccess { get; set; }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var authAttributes = context.Controller.GetType().GetCustomAttributes(true).Union(((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).MethodInfo.GetCustomAttributes(true)).OfType<RequireTokenAttribute>();
            var anonAttributes = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).MethodInfo.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>();

            if (!authAttributes.Any() || authAttributes.Any() && anonAttributes.Any())
            {
                return;
            }
            UserAccess = HttpContext.Request.CheckToken();
            var functionAttributes = context.Controller.GetType().GetCustomAttributes(true).Union(((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).MethodInfo.GetCustomAttributes(true)).OfType<RequireFunctionAttribute>();
            if (functionAttributes.Any() && !functionAttributes.Any(p => UserAccess.Grants.Contains(p.RoleName)))
                throw new NotStoredException("User has no grants for this method.");
        }
    }
}
