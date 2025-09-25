using Microsoft.AspNetCore.Mvc;
using WheelSellTA.BLL.Services;
using WheelSellTA.BLL.DTO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace WheelSell.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarsController : ControllerBase
    {
        private readonly CarService _carService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CarsController(CarService carService, IWebHostEnvironment webHostEnvironment)
        {
            _carService = carService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> GetCars()
        {
            var cars = await _carService.GetAllCarsAsync();
            return Ok(cars);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchCars([FromQuery] int? brandId, [FromQuery] int? modelId, [FromQuery] int? fuelTypeId, [FromQuery] int? transmissionId)
        {
            var cars = await _carService.SearchCarsAsync(brandId, modelId, fuelTypeId, transmissionId);
            return Ok(cars);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCar(int id)
        {
            var car = await _carService.GetCarByIdAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            return Ok(car);
        }

        [HttpGet("my-cars")]
        [Authorize(Roles = "Saler,Admin")]
        public async Task<IActionResult> GetMyCars()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var cars = await _carService.GetCarsByOwnerIdAsync(userId);
            return Ok(cars);
        }

        [HttpPost]
        [Authorize(Roles = "Saler,Admin")]
        public async Task<IActionResult> PostCar([FromBody] CarDTO carDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var createdCar = await _carService.CreateCarAsync(carDto, userId);

            return CreatedAtAction("GetCar", new { id = createdCar.Id }, createdCar);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Saler,Admin")]
        public async Task<IActionResult> PutCar(int id, [FromBody] CarDTO carDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedCar = await _carService.UpdateCarAsync(id, carDto);
            if (updatedCar == null)
            {
                return NotFound();
            }
            return Ok(updatedCar);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Saler,Admin")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _carService.DeleteCarAsync(id, userId);
            if (!result)
            {
                return Forbid(); // Если у пользователя нет прав на удаление этого авто
            }
            return NoContent();
        }

        [HttpPost("upload-media/{carId}")]
        [Authorize(Roles = "Saler,Admin")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadMedia(int carId, [FromForm] IFormFileCollection photos, [FromForm] IFormFileCollection videos)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var car = await _carService.GetCarByIdAsync(carId);

            if (car == null || car.OwnerId != userId)
            {
                return Forbid();
            }

            if (photos.Count == 0 && videos.Count == 0)
            {
                return BadRequest("No files received.");
            }

            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "cars", carId.ToString());
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var photoUrls = new List<string>();
            foreach (var file in photos)
            {
                if (file.Length > 0)
                {
                    var fileExtension = Path.GetExtension(file.FileName);
                    var fileName = $"{Guid.NewGuid()}{fileExtension}";
                    var filePath = Path.Combine(uploadsFolder, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    var url = $"/uploads/cars/{carId}/{fileName}";
                    photoUrls.Add(url);
                }
            }
            
            var videoUrls = new List<string>();
            foreach (var file in videos)
            {
                if (file.Length > 0)
                {
                    var fileExtension = Path.GetExtension(file.FileName);
                    var fileName = $"{Guid.NewGuid()}{fileExtension}";
                    var filePath = Path.Combine(uploadsFolder, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    var url = $"/uploads/cars/{carId}/{fileName}";
                    videoUrls.Add(url);
                }
            }

            var result = await _carService.AddMediaToCarAsync(carId, photoUrls, videoUrls);
            if (result)
            {
                return Ok(new { message = "Files uploaded successfully." });
            }

            return BadRequest("Failed to add media to car.");
        }
    }
}