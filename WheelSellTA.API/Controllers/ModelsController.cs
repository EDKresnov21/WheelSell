using Microsoft.AspNetCore.Mvc;
using WheelSellTA.BLL.Services;
using WheelSellTA.BLL.DTO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace WheelSell.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class ModelsController : ControllerBase
    {
        private readonly ModelService _modelService;

        public ModelsController(ModelService modelService)
        {
            _modelService = modelService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllModels()
        {
            var models = await _modelService.GetAllModelsAsync();
            return Ok(models);
        }

        [HttpGet("by-brand/{brandId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetModelsByBrand(int brandId)
        {
            var models = await _modelService.GetModelsByBrandAsync(brandId);
            return Ok(models);
        }

        [HttpPost]
        public async Task<IActionResult> PostModel([FromBody] ModelDTO modelDto)
        {
            var model = await _modelService.CreateModelAsync(modelDto);
            return CreatedAtAction("GetModel", new { id = model.Id }, model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutModel(int id, [FromBody] ModelDTO modelDto)
        {
            var updatedModel = await _modelService.UpdateModelAsync(id, modelDto);
            if (updatedModel == null)
            {
                return NotFound();
            }
            return Ok(updatedModel);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModel(int id)
        {
            var result = await _modelService.DeleteModelAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}