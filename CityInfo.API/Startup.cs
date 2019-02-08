using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Entities;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CityInfo.API.Models;

namespace CityInfo.API
{
    public class Startup
    {
        //This is for asp.net 1.0 web api
        //public static IConfigurationRoot Configuration;
        //dotnet core 2.0 uses the more generic IConfiguration Like so
        public static IConfiguration Configuration;
        public Startup(IConfiguration configuration) //IHostingEnvironment enc
        {
            Configuration = configuration;
            ////This is if working on an ASP.NET Core 1.0
            //var builder = new ConfigurationBuilder()
            //    .SetBasePath(env.ContentRootPath)
            //    .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
            //    .AddJsonFile($"appSettings.{env.EnvironmentName}.json", optional:true, reloadOnChange:true);
            //
            //Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //adding mvc middleware
            services.AddMvc();
            // .AddMvcOptions(o => o.OutputFormatters.Add( new XmlDataContractSerializerOutputFormatter()));
            //older legacy application may need to modfiy json serialize options here
            var connection = Configuration["connectionStrings:connectionStringCityInfoDB"];
            services.AddDbContext<CityInfoContext>(o => o.UseSqlServer(connection));
 
#if DEBUG
            services.AddTransient<IMailService, LocalMailService>();
#else
            services.AddTransient<IMailService, CloudMailService>();
#endif
            services.AddScoped<ICityInfoRepository, CityInfoRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, 
            ILoggerFactory loggerFactory, CityInfoContext cityInfoContext)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddNLog();
            loggerFactory.AddDebug();
            cityInfoContext.EnsureSeedDataForContext();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();

            }
            else
            {
                app.UseExceptionHandler();
            }

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<City, CityWithoutPointsOfInterestDto>();

            });

            app.UseMvc();

        }
    }
}
