using HotelListingExample.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

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
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = jwtSettings.GetSection("Issuer").Value,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                        };
                    });
        }
    }

}
