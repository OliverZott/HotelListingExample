using AutoMapper;
using HotelListingExample.Data;
using HotelListingExample.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace HotelListingExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly UserManager<ApiUser> _userManager;
        //private readonly SignInManager<ApiUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IMapper _mapper;

        public AccountController(
            UserManager<ApiUser> userManager,
            //SignInManager<ApiUser> signInManager,
            ILogger<AccountController> logger,
            IMapper mapper)
        {
            _userManager = userManager;
            //_signInManager = signInManager;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            _logger.LogInformation($"Registration attempt for {userDto.Email}");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);      // depending on validation constraints inside UserDTO
            }

            try
            {
                var user = _mapper.Map<ApiUser>(userDto);
                user.UserName = userDto.Email;     // IdentityUser class needs to have UserName
                var result = await _userManager.CreateAsync(user, userDto.Password);

                if (!result.Succeeded)
                {
                    //var errors = string.Join(';', result.Errors.Select(x => x.Code));
                    //return BadRequest($"User registration failed. Error Codes: {errors}");

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }

                    return BadRequest(ModelState);
                }

                return Accepted();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Something went wrong in the {nameof(Register)}");
                return Problem($"Something went wrong in the {nameof(Register)}", statusCode: 500);
            }
        }


        //[HttpPost]
        //[Route("login")]
        //public async Task<IActionResult> Login([FromBody] LoginUserDto loginUserDto)
        //{
        //    _logger.LogInformation($"Login attempt for {loginUserDto.Email}");
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);      // depending on validation constraints inside UserDTO
        //    }

        //    try
        //    {
        //        var result = await _signInManager
        //            .PasswordSignInAsync(loginUserDto.Email, loginUserDto.Password, false, false);
        //        if (!result.Succeeded)
        //        {
        //            return Unauthorized(loginUserDto);
        //        }

        //        return Accepted();
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.LogError(e, $"Something went wrong in {nameof(Login)}");
        //        return StatusCode(StatusCodes.Status500InternalServerError, $"Something went wrong in {nameof(Login)}");
        //    }


        //}
    }
}
