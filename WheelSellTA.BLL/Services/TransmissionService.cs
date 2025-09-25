using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WheelSellTA.BLL.DTO;
using WheelSellTA.DAL;
using WheelSellTA.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace WheelSellTA.BLL.Services;

public class TransmissionService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public TransmissionService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<TransmissionDTO>> GetAllTransmissionsAsync()
    {
        var transmissions = await _context.Transmissions.ToListAsync();
        return _mapper.Map<List<TransmissionDTO>>(transmissions);
    }

    public async Task<TransmissionDTO> CreateTransmissionAsync(TransmissionDTO transmissionDto)
    {
        var transmission = _mapper.Map<Transmission>(transmissionDto);
        _context.Transmissions.Add(transmission);
        await _context.SaveChangesAsync();
        return _mapper.Map<TransmissionDTO>(transmission);
    }

    public async Task<TransmissionDTO> UpdateTransmissionAsync(int id, TransmissionDTO transmissionDto)
    {
        var existingTransmission = await _context.Transmissions.FindAsync(id);
        if (existingTransmission == null)
        {
            return null;
        }
        _mapper.Map(transmissionDto, existingTransmission);
        await _context.SaveChangesAsync();
        return _mapper.Map<TransmissionDTO>(existingTransmission);
    }

    public async Task<bool> DeleteTransmissionAsync(int id)
    {
        var transmission = await _context.Transmissions.FindAsync(id);
        if (transmission == null)
        {
            return false;
        }
        _context.Transmissions.Remove(transmission);
        await _context.SaveChangesAsync();
        return true;
    }
}