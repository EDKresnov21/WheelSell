using Microsoft.AspNetCore.Mvc;
using WheelSellTA.BLL.Services;
using WheelSellTA.BLL.DTO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace WheelSell.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class FuelTypesController : ControllerBase
    {
        private readonly FuelTypeService _fuelTypeService;

        public FuelTypesController(FuelTypeService fuelTypeService)
        {
            _fuelTypeService = fuelTypeService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllFuelTypes()
        {
            var fuelTypes = await _fuelTypeService.GetAllFuelTypesAsync();
            return Ok(fuelTypes);
        }

        [HttpPost]
        public async Task<IActionResult> PostFuelType([FromBody] FuelTypeDTO fuelTypeDto)
        {
            var fuelType = await _fuelTypeService.CreateFuelTypeAsync(fuelTypeDto);
            return CreatedAtAction("GetFuelType", new { id = fuelType.Id }, fuelType);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutFuelType(int id, [FromBody] FuelTypeDTO fuelTypeDto)
        {
            var updatedFuelType = await _fuelTypeService.UpdateFuelTypeAsync(id, fuelTypeDto);
            if (updatedFuelType == null)
            {
                return NotFound();
            }
            return Ok(updatedFuelType);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFuelType(int id)
        {
            var result = await _fuelTypeService.DeleteFuelTypeAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}