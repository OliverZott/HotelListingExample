﻿using AutoMapper;
using HotelListingExample.IRepository;
using HotelListingExample.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace HotelListingExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly ILogger<HotelController> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public HotelController(ILogger<HotelController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200)] // For swaggger documentation
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // usage of response type CONSTANTS
        public async Task<IActionResult> GetHotels()
        {
            try
            {
                var hotels = await _unitOfWork.Hotels.GetAll();
                var result = _mapper.Map<IList<HotelDto>>(hotels);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Something went wrong in {nameof(GetHotels)}. Error message: {e.Message}");
                return StatusCode(500, "Internal Server Error. Sorry :)");
            }
        }

        [Authorize]
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotel(int id)
        {
            try
            {
                var hotel = await _unitOfWork.Hotels.Get(q => q.Id == id, new List<string> { "Country" });
                var result = _mapper.Map<HotelDto>(hotel);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Something went wrong in {nameof(GetHotels)}. Error message: {e.Message}");
                return StatusCode(500, "Internal Server Error. Sorry :)");
            }
        }
    }
}