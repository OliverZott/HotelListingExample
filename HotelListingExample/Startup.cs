using AspNetCoreRateLimit;
using HotelListingExample.Core;
using HotelListingExample.Core.Repository;
using HotelListingExample.Core.Services;
using HotelListingExample.Data;
using HotelListingExample.Data.Configurations.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json; //using HotelListingExample.Core.Configurations;

namespace HotelListingExample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("sqlConnection"))
            );

            services.ConfigureRateLimiting();
            services.AddHttpContextAccessor(); // access to actual controller

            services.AddMemoryCache();
            services.ConfigureHttpCacheHeaders();

            services.AddAuthentication();
            services.ConfigureIdentity();
            services.ConfigureJwt(Configuration);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "HotelListingExample", Version = "v1",
                        Description = "First ASP.NET example project (Reference: udemy-course)"
                    });
            });


            services.AddCors(service =>
            {
                service.AddPolicy("AllowAll",
                    builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
            });

            services.ConfigureAutoMapper();

            services.AddControllers(config =>
            {
                config.CacheProfiles.Add("120SecondsDuration", new CacheProfile { Duration = 120 });
            });
            services.AddControllers().AddNewtonsoftJson(op =>
                op.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.ConfigureVersioning();

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services
                .AddTransient<IEntityTypeConfiguration<IdentityRole>,
                    RoleConfiguration>(); // not necessary, but used in db context
            services.AddScoped<IAuthManager, AuthManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                var swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
                c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "Hotel Listing API");
            });

            app.ConfigureExceptionHandler();

            app.UseHttpsRedirection();

            app.UseResponseCaching();
            app.UseHttpCacheHeaders();
            app.UseIpRateLimiting();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default",
                    "{controller=Home}/{action=Index}/{id?}"); // specific controller convention
                //endpoints.MapControllers();
            });

            app.UseCors("AllowAll");
        }
    }
}