using System.Security.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecretStore.Domain.Interfaces.Services;
using SecretStore.Domain.Interfaces.Services.Exceptions;
using SecretStore.Domain.Models;
using SecretStore.Web.Attributes;
using SecretStore.Web.Controllers.DTOs;

namespace SecretStore.Web.Controllers;
[ApiController]
[Route("auth")]
public class AuthController : Controller
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IUserService userService, ILogger<AuthController> logger, ITokenService tokenService)
    {
        _userService = userService;
        _logger = logger;
        _tokenService = tokenService;
    }
    [HttpPost]
    [Route("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(AuthDto authDto)
    {
        try
        {
            var userId = await _userService.ValidateCredentials(new Credentials()
            {
                ClientId = authDto.ClientId,
                Secret = authDto.ClientSecret
            });

            var tokens = await _tokenService.IssueTokens(userId);

            return Ok(new
            {
                AccessToken = tokens.AccessToken,
                RefreshToken = tokens.RefreshToken,
                UserId = tokens.UserId
            });
        }
        catch (Exception ex) when (ex is InvalidCredentialException or AlreadyAuthorizeException)
        {
            return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during login!");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

    [HttpPost]
    [Route("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> Refresh([FromQuery] string refreshToken)
    {
        try
        {
            var tokens = await _tokenService.ReissueTokens(refreshToken);
            return Ok(tokens);

        }
        catch (InvalidRefreshTokenException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error during refresh token!");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
        
    }

    [HttpPost]
    [Route("")]
    [AdminAuth]
    public async Task<IActionResult> Add([FromBody]AuthDto authDto)
    {
        try
        {
            var id = await _userService.Add(authDto.ClientId, authDto.ClientSecret);
            return Ok(new UserDto()
            {
                ClientId = authDto.ClientId,
                Groups = new List<Group>(),
                Tokens = new List<Tokens>(),
                Id = id
            });
        }
        catch (ClientIdOccupiedException ex)
        {
            throw new ClientIdOccupiedException(ex.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error during adding new user!");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }
}