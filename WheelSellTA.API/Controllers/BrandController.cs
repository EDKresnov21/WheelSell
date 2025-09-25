using Microsoft.AspNetCore.Mvc;
using WheelSellTA.BLL.Services;
using WheelSellTA.BLL.DTO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace WheelSellTA.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class BrandsController : ControllerBase
    {
        private readonly BrandService _brandService;

        public BrandsController(BrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllBrands()
        {
            var brands = await _brandService.GetAllBrandsAsync();
            return Ok(brands);
        }
        
        [HttpPost]
        public async Task<IActionResult> PostBrand([FromBody] BrandDTO brandDto)
        {
            var brand = await _brandService.CreateBrandAsync(brandDto);
            return CreatedAtAction("GetBrand", new { id = brand.Id }, brand);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBrand(int id)
        {
            var brand = await _brandService.GetBrandByIdAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            return Ok(brand);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBrand(int id, [FromBody] BrandDTO brandDto)
        {
            var updatedBrand = await _brandService.UpdateBrandAsync(id, brandDto);
            if (updatedBrand == null)
            {
                return NotFound();
            }
            return Ok(updatedBrand);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            var result = await _brandService.DeleteBrandAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}