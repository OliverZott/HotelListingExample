﻿using AutoMapper;
using HotelListingExample.Data;
using HotelListingExample.IRepository;
using HotelListingExample.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        [HttpGet("{id:int}", Name = "GetAHotel")]
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

        [Authorize(Roles = "Administrator, User")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateHotel([FromBody] CreateHotelDto createHotelDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(CreateHotel)}");
                return BadRequest(ModelState);
            }

            try
            {
                var hotel = _mapper.Map<Hotel>(createHotelDto);
                await _unitOfWork.Hotels.Insert(hotel);  // all checks must be done before (e.g. model is valid above)
                await _unitOfWork.Save();
                //return StatusCode(201, "Created ");
                return CreatedAtRoute("GetAHotel", new { id = hotel.Id }, hotel);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Something went wrong in {nameof(CreateHotel)}. Error message: {e.Message}");
                return StatusCode(500, "Internal Server Error. Sorry :)");
            }
        }


        // 2 variants:
        // passing ID in url or in Body!
        [Authorize]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateHotel(int id, [FromBody] UpdateHotelDto updateHotelDto)
        {
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateHotel)}");
                return BadRequest(ModelState);
            }

            try
            {
                var hotel = await _unitOfWork.Hotels.Get(q => q.Id == id);
                if (hotel == null)
                {
                    _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateHotel)}");
                    return BadRequest(ModelState);
                }
                _mapper.Map(updateHotelDto, hotel);  // mapping on existing object (instead of creating new object)
                _unitOfWork.Hotels.Update(hotel);
                await _unitOfWork.Save();
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Something went wrong in {nameof(UpdateHotel)}. Error message: {e.Message}");
                return StatusCode(500, "Internal Server Error. Sorry :)");
            }
        }

    }
}