// Изменяем CarService.cs, чтобы он реализовывал интерфейс
using Microsoft.EntityFrameworkCore;
using WheelSell.DAL;
using WheelSell.DAL.Entities;

namespace WheelSell.BLL.Services
{
    public class CarService : ICarService
    {
        private readonly AppDbContext _context;
        public CarService(AppDbContext context) => _context = context;

        public async Task<List<Car>> GetAllCarsAsync() => await _context.Cars.ToListAsync();
        public async Task<Car?> GetCarByIdAsync(int id) => await _context.Cars.FindAsync(id);
        public async Task AddCarAsync(Car car)
        {
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteCarAsync(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car != null)
            {
                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();
            }
        }
    }
}