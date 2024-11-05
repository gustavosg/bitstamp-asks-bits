using Microsoft.AspNetCore.Mvc;
using PriceSimulator.Application.DTOs;
using PriceSimulator.Application.Interfaces;

namespace PriceSimulator.Presentation.Controllers
{
    [ApiController]
    [Route("price")]
    public class PriceController : Controller
    {
        private readonly IPriceService priceSimulatorService;

        public PriceController(IPriceService priceSimulatorService)
        {
            this.priceSimulatorService = priceSimulatorService;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromQuery] PriceSimulationRequest request)
        => Ok(await this.priceSimulatorService.CalculateBestBuyPrice(request));

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            if (!await this.priceSimulatorService.Exists(id))
                return NotFound();

            
            return Ok(await this.priceSimulatorService.Get(id));
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        => Ok(await this.priceSimulatorService.Get());
    }
}
