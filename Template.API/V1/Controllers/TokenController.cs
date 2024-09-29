using Microsoft.AspNetCore.Mvc;
using Template.API.Infrastructure;

namespace Template.API.V1.Controllers;

/// <summary>
///     Token Controller
/// </summary>
[ApiController]
[Route("v1/[controller]")]
public class TokenController : ControllerBase
{
    private readonly ILogger<TokenController> _logger;

    /// <summary>
    ///     Login Endpoint
    /// </summary>
    /// <param name="logger"></param>
    public TokenController(
        ILogger<TokenController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    ///     Get Admin Token
    /// </summary>
    /// <respone code="401">Not Authorize.</respone>
    /// <response code="500">Server error.</response>
    /// <returns></returns>
    [HttpPost]
    [Route("", Name = "Token")]
    public Task<IActionResult> Token()
    {
        var tokenString = JwtTokenService.GenerateJwtToken("1", "template");
        Response.Headers.Authorization = $"{tokenString}";

        _logger.LogInformation("Token Generated");
        return Task.FromResult<IActionResult>(Ok());
    }
}