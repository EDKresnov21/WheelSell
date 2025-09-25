using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WheelSellTA.BLL.DTO;
using WheelSellTA.DAL;
using WheelSellTA.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WheelSellTA.BLL.Services;

public class BrandService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public BrandService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<BrandDTO>> GetAllBrandsAsync()
    {
        var brands = await _context.Brands.ToListAsync();
        return _mapper.Map<List<BrandDTO>>(brands);
    }

    public async Task<BrandDTO> GetBrandByIdAsync(int id)
    {
        var brand = await _context.Brands.FindAsync(id);
        return _mapper.Map<BrandDTO>(brand);
    }

    public async Task<BrandDTO> CreateBrandAsync(BrandDTO brandDto)
    {
        var brand = _mapper.Map<Brand>(brandDto);
        _context.Brands.Add(brand);
        await _context.SaveChangesAsync();
        return _mapper.Map<BrandDTO>(brand);
    }

    public async Task<BrandDTO> UpdateBrandAsync(int id, BrandDTO brandDto)
    {
        var existingBrand = await _context.Brands.FindAsync(id);
        if (existingBrand == null)
        {
            return null;
        }
        _mapper.Map(brandDto, existingBrand);
        await _context.SaveChangesAsync();
        return _mapper.Map<BrandDTO>(existingBrand);
    }

    public async Task<bool> DeleteBrandAsync(int id)
    {
        var brand = await _context.Brands.FindAsync(id);
        if (brand == null)
        {
            return false;
        }
        _context.Brands.Remove(brand);
        await _context.SaveChangesAsync();
        return true;
    }
}