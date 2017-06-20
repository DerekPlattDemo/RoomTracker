using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;
using RoomInfo.API.Services;
using Microsoft.Extensions.Configuration;
using RoomInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace RoomInfo.API
{
    public class Startup
    {
        public static IConfigurationRoot Configuration;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appSettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var connectionString = Startup.Configuration["connectionStrings:roomInfoDBConnectionString"];
            services.AddDbContext<RoomInfoContext>(o => o.UseSqlServer(connectionString));

            services.AddScoped<IRoomInfoRepository, RoomInfoRepository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            RoomInfoContext roomInfoContext)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();
            loggerFactory.AddNLog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }

            //TODO, research -- extension method may be baked into Core now
            roomInfoContext.EnsureSeedDataForContext();

            app.UseStatusCodePages();

            //setup basic automappers for DTO's and entities 
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Entities.Room, Models.RoomWithoutItemsOfInterestDto>();
                cfg.CreateMap<Entities.Room, Models.RoomDto>();
                cfg.CreateMap<Entities.ItemOfInterest, Models.ItemOfInterestDto>();
                cfg.CreateMap<Models.ItemOfInterestForCreationDto, Entities.ItemOfInterest>();
                cfg.CreateMap<Models.ItemOfInterestForUpdateDto, Entities.ItemOfInterest>();
                cfg.CreateMap<Entities.ItemOfInterest, Models.ItemOfInterestForUpdateDto>();
            });

            app.UseMvc();

            //app.Run((context) =>
            //{
            //    throw new Exception("Example exception");
            //});

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
        }
    }
}
