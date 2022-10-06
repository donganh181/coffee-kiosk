using coffee_kiosk_solution.App_Start;
using coffee_kiosk_solution.Business.Hubs;
using coffee_kiosk_solution.Business.SystemSchedules.Jobs;
using coffee_kiosk_solution.Data.Context;
using coffee_kiosk_solution.Handler;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coffee_kiosk_solution
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<Coffee_KioskContext>(options =>
                options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers();

            services.ConfigureDI();
            services.AddSwaggerGenNewtonsoftSupport();
            services.ConfigureSwagger();
            services.AddCors();
            services.AddSignalR();

            JWTBearerConfig.ConfigAuthentication(services, _configuration);

            services.ConfigureFilter<ErrorHandlingFilter>();

            // services.Configure<QuartzOptions>(Configuration.GetSection("Quartz"));
            // services.Configure<QuartzOptions>(options =>
            // {
            //     options.Scheduling.IgnoreDuplicates = true; // default: false
            //     options.Scheduling.OverWriteExistingData = true; // default: true
            // });


            //
            services.AddQuartz(q =>
            {
                // handy when part of cluster or you want to otherwise identify multiple schedulers
                q.SchedulerId = "Scheduler-Core";

                // we take this from appsettings.json, just show it's possible
                // q.SchedulerName = "Quartz ASP.NET Core Sample Scheduler";

                // as of 3.3.2 this also injects scoped services (like EF DbContext) without problems
                q.UseMicrosoftDependencyInjectionJobFactory();

                // or for scoped service support like EF Core DbContext
                // q.UseMicrosoftDependencyInjectionScopedJobFactory();

                // these are the defaults
                q.UseSimpleTypeLoader();
                q.UseInMemoryStore();
                q.UseDefaultThreadPool(tp => { tp.MaxConcurrency = 10; });

                ITrigger campaignTrigger = TriggerBuilder.Create()
                                        .WithIdentity("campaignJob")
                                        .StartNow()
                                        .WithCronSchedule("0 0 0 * * ?")
                                        .Build();

                
                ITrigger supplyTrigger = TriggerBuilder.Create()
                                        .WithIdentity("supplyJob")
                                        .StartNow()
                                        .WithCronSchedule("0 0 12 ? * SUN")
                                        .Build();

                
                // quickest way to create a job with single trigger is to use ScheduleJob
                // (requires version 3.2)
                q.ScheduleJob<CheckCampaignJob>(trigger => trigger
                    .WithIdentity("campaignJob")
                    .StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddSeconds(7)))
                    .WithCronSchedule("0 0 0 * * ?")
                    .WithDescription("my awesome trigger configured for a job with single call")
                );
                q.ScheduleJob<CheckSupplyJob>(trigger => trigger
                     .WithIdentity("supplyJob")
                     .StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddSeconds(7)))
                     .WithCronSchedule("0 0 12 ? * SUN")
                     .WithDescription("my awesome trigger configured for a job with single call")
                 );
            });
            // Quartz.Extensions.Hosting allows you to fire background service that handles scheduler lifecycle
            services.AddQuartzHostedService(options =>
            {
                // when shutting down we want jobs to complete gracefully
                options.WaitForJobsToComplete = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
               
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(builder =>
            {
                builder.AllowAnyHeader()
                    .AllowAnyMethod()
                    .SetIsOriginAllowed((host) => true)
                    .AllowCredentials();
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.ConfigureSwagger(provider);
            // end point signalR
            app.UseEndpoints(routes => { routes.MapHub<SystemEventHub>("/signalr"); });
        }
    }
}
