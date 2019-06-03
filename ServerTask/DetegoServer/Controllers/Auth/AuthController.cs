using DetegoServer.Helpers;
using DetegoServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DetegoServer.Controllers.Auth
{
    /// <summary>
    /// Main auth controller
    /// </summary>
    [Route("api/auth")]
    [AllowAnonymous]
    public class AuthController : Controller
    {
        [HttpPost("login")]
        public string Login([FromQuery]string login, [FromBody] string password)
        {
            //check in DB like db.Users.FirstOrDefault(p=>p.Login == login && p.Password==password);
            #region hardcode auth
            if (login.ToLower() == "test" && password == "123")
            {
                UserAccess ua = new UserAccess()
                {
                    UserId = 1,
                    Grants = new List<string>() { "Reader" }
                };
                return JWTHelper.CreateToken(ua);
            }
            else if (login.ToLower() == "test2" && password == "123")
            {
                UserAccess ua = new UserAccess()
                {
                    UserId = 1,
                    Grants = new List<string>() { "Writer", "Reader" }
                };
                return JWTHelper.CreateToken(ua);
            }
            #endregion

            throw new NotStoredException("Incorrect login or password!");
        }
    }
}
