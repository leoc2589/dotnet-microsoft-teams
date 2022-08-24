using App.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IMSGraphService _msGraphService;
    private readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(
        IMSGraphService msGraphService,
        ILogger<AuthenticationController> logger)
    {
        _msGraphService = msGraphService;
        _logger = logger;
    }

    /// <summary>
    /// Get token.
    /// </summary>
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    [HttpGet]
    public async Task<ActionResult<string>> GetTokenAsync()
    {
        var (Success, Token, Message) = await _msGraphService.GetTokenAsync();

        if (!Success)
            return Unauthorized(new { Message });

        return Ok(new { Token });
    }
}