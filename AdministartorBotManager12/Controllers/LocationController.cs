using Microsoft.AspNetCore.Mvc;
using Repository.Abstraction;
using Service.Services.DataScrapper.Implementation;

namespace AdministartorBotManager.Controllers;

[ApiController]
[Route("api/convert")]
public class LocationController : ControllerBase
{
    private readonly ILocationService _locationService;

    public LocationController(ILocationService locationService)
    {
        _locationService=locationService;
    }

    [HttpPut("UpdateLocations")]
    public async Task<IActionResult> UpdateLocationsAsync()
    {
        await _locationService.GetLocationsAsync(nameof(AmeriaBankDataScrapper));
        return NoContent();
    }

    [HttpPut(nameof(UpdateAllBankLocations))]
    public async Task<IActionResult> UpdateAllBankLocations()
    {
        return Ok(await _locationService.UpdateAllBankLocationsAsync());
    }
}
