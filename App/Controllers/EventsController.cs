using App.Interfaces;
using App.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace App.Controllers;

[ApiController]
[Route("[controller]")]
public class EventsController : ControllerBase
{
    private readonly IMSGraphService _msGraphService;
    private readonly ILogger<EventsController> _logger;

    public EventsController(
        IMSGraphService msGraphService,
        ILogger<EventsController> logger)
    {
        _msGraphService = msGraphService;
        _logger = logger;
    }

    /// <summary>
    /// Create event.
    /// </summary>
    /// <response code="200">Success</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Not Found</response>
    [HttpPost]
    public async Task<ActionResult<Event>> CreateAsync()
    {
        var authResponse = await _msGraphService.GetTokenAsync();

        if (!authResponse.Success)
            return Unauthorized(new { authResponse.Message });

        Event _event;

        using (StreamReader reader = new("event.sample.request.json"))
        {
            string json = reader.ReadToEnd();

            if (string.IsNullOrEmpty(json))
                return NotFound(new { Message = "Event not found" });

            _event = JsonConvert.DeserializeObject<Event>(json);
        }

        var createEventResponse = await _msGraphService.CreateEventAsync(authResponse.Token, _event);

        if (!createEventResponse.Success)
            return BadRequest(new { createEventResponse.Message });

        return Ok(new { createEventResponse.Result });
    }
}