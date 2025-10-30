using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{

    public interface IDishService
    {
        int Create(int restaurantId, CreateDishDto dto);
        DishDto Get(int restaurantId, int id);
        List<DishDto> GetAll(int restaurantId);
        void DeleteAll(int restaurantId);
        void Delete(int restaurantId, int dishId);
    }

    public class DishService : IDishService
    {
        private readonly RestaurantDbContext _context;
        private readonly IMapper _mapper;
        public DishService(RestaurantDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public int Create(int restaurantId, CreateDishDto dto)
        {
            GetRestaurantById(restaurantId);

            var dish = _mapper.Map<Dish>(dto);
            dish.RestaurantId = restaurantId;
            _context.Dishes.Add(dish);
            _context.SaveChanges();
            return dish.Id;
        }

        public DishDto Get(int restaurantId, int id)
        {
            var restaurant = GetRestaurantById(restaurantId);

            var dish = restaurant.Dishes.FirstOrDefault(d => d.Id == id);

            if (dish == null)
            {
                throw new NotFoundException("Dish not found");
            }

            var dishDto = _mapper.Map<DishDto>(dish);
            return dishDto;
        }

        public List<DishDto> GetAll(int restaurantId)
        {
            var restaurant = GetRestaurantById(restaurantId);

            var dishes = restaurant.Dishes;

            var dishDtos = _mapper.Map<List<DishDto>>(dishes);
            return dishDtos;
        }

        public void DeleteAll(int restaurantId)
        {
            var restaurant = GetRestaurantById(restaurantId);

            var dishes = restaurant.Dishes;

            _context.Dishes.RemoveRange(dishes);
            _context.SaveChanges();
        }

        public void Delete(int restaurantId, int dishId)
        {
            var restaurant = GetRestaurantById(restaurantId);

            var dish = restaurant.Dishes.FirstOrDefault(d => d.Id == dishId);

            if (dish == null)
            {
                throw new NotFoundException("Dish not found");
            }

            _context.Dishes.Remove(dish);
            _context.SaveChanges();
        }

        private Restaurant GetRestaurantById(int restaurantId)
        {
            var restaurant = _context.Restaurants.Include(r => r.Dishes).FirstOrDefault(r => r.Id == restaurantId);

            if (restaurant == null)
            {
                throw new NotFoundException("Restaurant not found");
            }

            return restaurant;
        }
    }
}