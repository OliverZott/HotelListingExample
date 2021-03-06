using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HotelListingExample.Core.DTOs;
using HotelListingExample.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HotelListingExample.Core.Services
{
    public class AuthManager : IAuthManager
    {
        private readonly IConfiguration _configuration;

        private readonly UserManager<ApiUser> _userManager;
        private ApiUser _user;


        public AuthManager(UserManager<ApiUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        // Checks if user exists and if password is correct.
        public async Task<bool> ValidateUser(LoginUserDto userDto)
        {
            _user = await _userManager.FindByNameAsync(userDto.Email);
            return _user != null && await _userManager.CheckPasswordAsync(_user, userDto.Password);
        }

        public async Task<string> CreateToken()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var token = GenerateToken(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // JWT contains issuer validation!!
        private JwtSecurityToken GenerateToken(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("Jwt");

            var expirationDateTime = DateTime.Now.AddMinutes(Convert.ToInt32(jwtSettings.GetSection("lifetime").Value));

            var token = new JwtSecurityToken(
                jwtSettings.GetSection("Issuer").Value,
                claims: claims,
                expires: expirationDateTime,
                signingCredentials: signingCredentials
            );

            return token;
        }

        private static SigningCredentials GetSigningCredentials()
        {
            var key = Environment.GetEnvironmentVariable("KEY");
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }


        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, _user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(_user);
            foreach (var role in roles) claims.Add(new Claim(ClaimTypes.Role, role));

            return claims;
        }
    }
}