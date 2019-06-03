using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DetegoServer.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;

namespace DetegoServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
            services.AddMvc(options =>
            {
                options.ModelBinderProviders.Insert(0, new CustomBinderProvider());
            }).AddJsonOptions(options =>
            {
                options.SerializerSettings.DateFormatString = JsonConfigurate.JsonSettings.DateFormatString;
                options.SerializerSettings.ReferenceLoopHandling = JsonConfigurate.JsonSettings.ReferenceLoopHandling;
                options.SerializerSettings.NullValueHandling = JsonConfigurate.JsonSettings.NullValueHandling;
                options.SerializerSettings.MissingMemberHandling = JsonConfigurate.JsonSettings.MissingMemberHandling;
                options.SerializerSettings.ContractResolver = JsonConfigurate.JsonSettings.ContractResolver;
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Stock API", Version = "v1" });
                c.AddSecurityDefinition("JWT Token", new ApiKeyScheme
                {
                    Description = "JWT Token",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    {"JWT Token", new string[]{ } }
                });
                c.IncludeXmlComments(string.Format(@"{0}Server.xml", AppDomain.CurrentDomain.BaseDirectory));
                c.OperationFilter<AuthFilter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseExceptionHandler(
            builder =>
            {
                builder.Run(
                  async context =>
                  {
                      context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                      context.Response.ContentType = "application/json";
                      context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                      context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");

                      var error = context.Features.Get<IExceptionHandlerFeature>();
                      if (error != null)
                      {
                          var exception = error.Error;
                          if (env.IsProduction() && !(exception is NotStoredException))
                          {
                              //log or saving to DB
                          }
                          string jsonException = JsonConvert.SerializeObject(exception, JsonConfigurate.JsonSettings);
                          Console.Error.WriteLine(jsonException);
                          await context.Response.WriteAsync(jsonException).ConfigureAwait(false);
                      }
                  });
            });
            app.UseCors("CorsPolicy");
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Stock API");
            });
        }
    }
}
