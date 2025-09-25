using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WheelSellTA.BLL.DTO;
using WheelSellTA.DAL;
using WheelSellTA.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace WheelSellTA.BLL.Services;

public class ModelService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ModelService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ModelDTO>> GetAllModelsAsync()
    {
        var models = await _context.Models.ToListAsync();
        return _mapper.Map<List<ModelDTO>>(models);
    }

    public async Task<List<ModelDTO>> GetModelsByBrandAsync(int brandId)
    {
        var models = await _context.Models.Where(m => m.BrandId == brandId).ToListAsync();
        return _mapper.Map<List<ModelDTO>>(models);
    }

    public async Task<ModelDTO> CreateModelAsync(ModelDTO modelDto)
    {
        var model = _mapper.Map<Model>(modelDto);
        _context.Models.Add(model);
        await _context.SaveChangesAsync();
        return _mapper.Map<ModelDTO>(model);
    }

    public async Task<ModelDTO> UpdateModelAsync(int id, ModelDTO modelDto)
    {
        var existingModel = await _context.Models.FindAsync(id);
        if (existingModel == null)
        {
            return null;
        }
        _mapper.Map(modelDto, existingModel);
        await _context.SaveChangesAsync();
        return _mapper.Map<ModelDTO>(existingModel);
    }

    public async Task<bool> DeleteModelAsync(int id)
    {
        var model = await _context.Models.FindAsync(id);
        if (model == null)
        {
            return false;
        }
        _context.Models.Remove(model);
        await _context.SaveChangesAsync();
        return true;
    }
}