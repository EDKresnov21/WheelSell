using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WheelSellTA.BLL.DTO;
using WheelSellTA.DAL;
using WheelSellTA.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WheelSellTA.BLL.Services;

public class FuelTypeService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public FuelTypeService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<FuelTypeDTO>> GetAllFuelTypesAsync()
    {
        var fuelTypes = await _context.FuelTypes.ToListAsync();
        return _mapper.Map<List<FuelTypeDTO>>(fuelTypes);
    }

    public async Task<FuelTypeDTO> CreateFuelTypeAsync(FuelTypeDTO fuelTypeDto)
    {
        var fuelType = _mapper.Map<FuelType>(fuelTypeDto);
        _context.FuelTypes.Add(fuelType);
        await _context.SaveChangesAsync();
        return _mapper.Map<FuelTypeDTO>(fuelType);
    }

    public async Task<FuelTypeDTO> UpdateFuelTypeAsync(int id, FuelTypeDTO fuelTypeDto)
    {
        var existingFuelType = await _context.FuelTypes.FindAsync(id);
        if (existingFuelType == null)
        {
            return null;
        }
        _mapper.Map(fuelTypeDto, existingFuelType);
        await _context.SaveChangesAsync();
        return _mapper.Map<FuelTypeDTO>(existingFuelType);
    }

    public async Task<bool> DeleteFuelTypeAsync(int id)
    {
        var fuelType = await _context.FuelTypes.FindAsync(id);
        if (fuelType == null)
        {
            return false;
        }
        _context.FuelTypes.Remove(fuelType);
        await _context.SaveChangesAsync();
        return true;
    }
}