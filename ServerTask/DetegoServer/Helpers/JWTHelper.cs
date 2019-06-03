using DetegoServer.Models;
using Jose;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetegoServer.Helpers
{
    public static class JWTHelper
    {
        private static JsonSerializerSettings JsonSettings { get; set; } = new JsonSerializerSettings()//different form main, because other rules for tokens. For ex. We should include null values, ignoring config options.
        {
            MissingMemberHandling = JsonConfigurate.JsonSettings.MissingMemberHandling,
            NullValueHandling = JsonConfigurate.JsonSettings.NullValueHandling,
            DefaultValueHandling = JsonConfigurate.JsonSettings.DefaultValueHandling,
            DateFormatString = JsonConfigurate.JsonSettings.DateFormatString,
            ReferenceLoopHandling = JsonConfigurate.JsonSettings.ReferenceLoopHandling,
            ContractResolver = new ExceptionResolver(),
        };
        static JWTHelper()
        {
        }
        public static string CreateToken(UserAccess ua)
        {
            string sub = JsonConvert.SerializeObject(ua, Newtonsoft.Json.Formatting.None, JsonSettings);
            var baseDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var payload = new Dictionary<string, object>()
            {
                { "sub", Convert.ToBase64String(Encoding.UTF8.GetBytes(sub)) },
                { "exp", (int)(DateTime.UtcNow.AddHours(24) - baseDate).TotalSeconds},
                { "iat", (int)(DateTime.UtcNow - baseDate).TotalSeconds },
                { "nbf", (int)(DateTime.UtcNow - baseDate).TotalSeconds },
                { "role", ua.Grants}
            };

            string token = JWT.Encode(payload, ConfigHelper.Instance.SecurityKey, JwsAlgorithm.HS512);
            return token;
        }

        public static UserAccess ReadToken(string token)
        {
            UserAccess ua = new UserAccess();
            string payload = Jose.JWT.Decode(token, ConfigHelper.Instance.SecurityKey, JwsAlgorithm.HS512);
            Dictionary<string, object> dict = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(payload, JsonSettings);
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds((long)dict["exp"]);
            if (dt < DateTime.UtcNow)
                throw new UnauthorizedAccessException("Token expired");
            ua = Newtonsoft.Json.JsonConvert.DeserializeObject<UserAccess>(Encoding.UTF8.GetString(Convert.FromBase64String((string)dict["sub"])));
            return ua;
        }

        public static UserAccess CheckToken(this HttpRequest request)
        {
            var headers = request.Headers;
            if (headers.ContainsKey("Authorization"))
                return ReadToken(headers["Authorization"].FirstOrDefault().Split(' ')[1]);
            else
                throw new UnauthorizedAccessException("Token required");
        }
    }
}
