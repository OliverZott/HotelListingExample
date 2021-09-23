using HotelListingExample.Configurations;
using HotelListingExample.Configurations.Entities;
using HotelListingExample.Data;
using HotelListingExample.IRepository;
using HotelListingExample.Repository;
using HotelListingExample.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace HotelListingExample
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("sqlConnection"))
            );

            services.AddResponseCaching();
            services.AddAuthentication();
            services.ConfigureIdentity();
            services.ConfigureJwt(Configuration);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HotelListingExample", Version = "v1", Description = "First ASP.NET example project (Reference: udemy-course)" });
            });


            services.AddCors(service =>
            {
                service.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });

            services.AddAutoMapper(typeof(MapperInitializer));

            services.AddControllers().AddNewtonsoftJson(op => op.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.ConfigureVersioning();

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IEntityTypeConfiguration<IdentityRole>, RoleConfiguration>();  // not necessary, but used in db context
            services.AddScoped<IAuthManager, AuthManager>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HotelListingExample v1"));

            app.ConfigureExceptionHandler();

            app.UseHttpsRedirection();

            app.UseResponseCaching();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");    // specific controller convention
                //endpoints.MapControllers();
            });

            app.UseCors("AllowAll");
        }
    }
}
