using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WheelSell.BLL.DTO;
using WheelSell.DAL;
using WheelSell.DAL.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WheelSell.BLL.Services
{
    public class CarService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CarService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CarDTO>> GetAllCarsAsync()
        {
            var cars = await _context.Cars
                                    .Include(c => c.Brand)
                                    .Include(c => c.Model)
                                    .Include(c => c.FuelType)
                                    .Include(c => c.Transmission)
                                    .Include(c => c.Photos)
                                    .Include(c => c.Videos)
                                    .Include(c => c.Owner)
                                    .ToListAsync();
            return _mapper.Map<List<CarDTO>>(cars);
        }

        public async Task<CarDTO> GetCarByIdAsync(int id)
        {
            var car = await _context.Cars
                                    .Include(c => c.Brand)
                                    .Include(c => c.Model)
                                    .Include(c => c.FuelType)
                                    .Include(c => c.Transmission)
                                    .Include(c => c.Photos)
                                    .Include(c => c.Videos)
                                    .Include(c => c.Owner)
                                    .FirstOrDefaultAsync(c => c.Id == id);
            return _mapper.Map<CarDTO>(car);
        }

        public async Task<CarDTO> CreateCarAsync(CarDTO carDto, int ownerId)
        {
            var car = _mapper.Map<Car>(carDto);
            car.OwnerId = ownerId;
            car.Owner = await _context.Users.FindAsync(ownerId);
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
            return _mapper.Map<CarDTO>(car);
        }

        public async Task<CarDTO> UpdateCarAsync(int id, CarDTO carDto)
        {
            var existingCar = await _context.Cars.FindAsync(id);
            if (existingCar == null)
            {
                return null;
            }

            _mapper.Map(carDto, existingCar);
            await _context.SaveChangesAsync();
            return _mapper.Map<CarDTO>(existingCar);
        }

        public async Task<bool> DeleteCarAsync(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return false;
            }
            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}