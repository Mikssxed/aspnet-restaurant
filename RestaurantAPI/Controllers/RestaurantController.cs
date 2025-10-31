using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant")]
    [ApiController]
    [Authorize]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }
        [HttpGet]
        [Authorize(Policy = "HasNationality")]
        public ActionResult<IEnumerable<RestaurantDto>> GetAll()
        {
            var restaurants = _restaurantService.GetAll();
            return Ok(restaurants);
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public ActionResult<RestaurantDto> GetById([FromRoute] int id)
        {
            var restaurant = _restaurantService.GetById(id);

            return Ok(restaurant);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto)
        {
            var id = _restaurantService.CreateRestaurant(dto);

            return CreatedAtAction(nameof(GetById), new { id }, null);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteRestaurant([FromRoute] int id)
        {
            _restaurantService.DeleteRestaurant(id);

            return NoContent();
        }

        [HttpPut("{id}")]
        public ActionResult UpdateRestaurant([FromRoute] int id, [FromBody] UpdateRestaurantDto dto)
        {
            _restaurantService.UpdateRestaurant(id, dto);

            return NoContent();
        }
    }
}