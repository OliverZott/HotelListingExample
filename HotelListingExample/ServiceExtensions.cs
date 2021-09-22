using HotelListingExample.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using HotelListingExample.Models;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;

namespace HotelListingExample
{
    public static class ServiceExtensions
    {
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentityCore<ApiUser>(q =>
            {
                q.User.RequireUniqueEmail = true;
                q.Password.RequiredLength = 8;
            });

            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), services);
            builder.AddEntityFrameworkStores<DatabaseContext>().AddDefaultTokenProviders();
        }


        public static void ConfigureIdentityAlternativeVersion(this IServiceCollection services)
        {
            services.AddIdentityCore<ApiUser>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequiredLength = 8;
                })
                .AddEntityFrameworkStores<DatabaseContext>()
                .AddDefaultTokenProviders();
        }


        public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("Jwt");
            var key = Environment.GetEnvironmentVariable("KEY");    // the env-var, set in windows by 'setx KEY "..." /m'

            services
                .AddAuthentication(o =>
                    {
                        o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // if someone trying to authenticate --> check for bearer token
                        o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                .AddJwtBearer(o =>
                    {
                        o.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateLifetime = true,
                            ValidateAudience = false,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = jwtSettings.GetSection("Issuer").Value,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                        };
                    });
        }


        /// <summary>
        /// Customized Exception handling:
        /// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-5.0
        /// https://www.tutorialsteacher.com/core/aspnet-core-exception-handling
        /// 
        /// </summary>
        /// <param name="app"></param>
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(error =>
            {
                error.Run(async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        // Logging the error
                        Log.Error($"Something went wrong in {contextFeature.Error}");

                        // Generate error-model with status code and message to response body.
                        // (defined in custom error-class)
                        await context.Response.WriteAsync(new Error
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error. Sorry :)"
                        }.ToString());
                    }
                });
            });
        }
    }

}
