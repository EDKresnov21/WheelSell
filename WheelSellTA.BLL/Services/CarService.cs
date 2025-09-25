using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WheelSellTA.BLL.DTO;
using WheelSellTA.DAL;
using WheelSellTA.DAL.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WheelSellTA.BLL.DTO;

namespace WheelSellTA.BLL.Services;


//Сервис - то, на что ссылается АПИ и то, что ссылается на ДАЛ. Выполняет большинство посреднеческих операций по типу "Взять все машины из ДАЛ и отдать результат АПИ"
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
    
    public async Task<List<CarDTO>> GetCarsByOwnerIdAsync(int ownerId)
    {
        var cars = await _context.Cars
            .Where(c => c.OwnerId == ownerId)
            .Include(c => c.Brand)
            .Include(c => c.Model)
            .Include(c => c.FuelType)
            .Include(c => c.Transmission)
            .Include(c => c.Photos)
            .Include(c => c.Videos)
            .ToListAsync();
        return _mapper.Map<List<CarDTO>>(cars);
    }

    public async Task<List<CarDTO>> SearchCarsAsync(int? brandId, int? modelId, int? fuelTypeId, int? transmissionId)
    {
        IQueryable<Car> query = _context.Cars
            .Include(c => c.Brand)
            .Include(c => c.Model)
            .Include(c => c.FuelType)
            .Include(c => c.Transmission)
            .Include(c => c.Photos)
            .Include(c => c.Videos);
        if (brandId.HasValue)
        {
            query = query.Where(c => c.BrandId == brandId);
        }

        if (modelId.HasValue)
        {
            query = query.Where(c => c.ModelId == modelId);
        }
        if (fuelTypeId.HasValue)
        {
            query = query.Where(c => c.FuelTypeId == fuelTypeId.Value);
        }
        if (transmissionId.HasValue)
        {
            query = query.Where(c => c.TransmissionId == transmissionId.Value);
        }
        
        var cars = await query.ToListAsync();
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

    public async Task<CarDTO> CreateCarAsync(CarDTO carDTO, int ownerId)
    {
        var car = _mapper.Map<Car>(carDTO);
        car.OwnerId = ownerId;
        car.Owner = await _context.Users.FindAsync(ownerId);
        _context.Cars.Add(car);
        await _context.SaveChangesAsync();
        return _mapper.Map<CarDTO>(car);
    }

    public async Task<CarDTO> UpdateCarAsync(int id, CarDTO carDTO)
    {
        var existingCar = await _context.Cars.FindAsync(id);
        if (existingCar == null)
        {
            return null;
        }
        _mapper.Map(carDTO, existingCar);
        await _context.SaveChangesAsync();
        return _mapper.Map<CarDTO>(existingCar);
    }

    public async Task<bool> DeleteCarAsync(int id, int userId)
    {
        var car = await _context.Cars.FirstOrDefaultAsync(c => c.Id == id && c.OwnerId == userId);
        if (car == null)
        {
            return false;
        }
        _context.Cars.Remove(car);
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<bool> AddMediaToCarAsync(int carId, List<string> photoUrls, List<string> videoUrls)
    {
        var car = await _context.Cars.Include(c => c.Photos).Include(c => c.Videos).FirstOrDefaultAsync(c => c.Id == carId);
        if (car == null)
        {
            return false;
        }
            
        if (car.Photos == null) car.Photos = new List<Photo>();
        if (car.Videos == null) car.Videos = new List<Video>();

        foreach (var url in photoUrls)
        {
            car.Photos.Add(new Photo { Url = url, CarId = carId });
        }
            
        foreach (var url in videoUrls)
        {
            car.Videos.Add(new Video { Url = url, CarId = carId });
        }

        await _context.SaveChangesAsync();
        return true;
    }
}