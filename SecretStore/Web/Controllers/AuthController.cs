using System.Security.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecretStore.Domain.Interfaces.Services;
using SecretStore.Domain.Interfaces.Services.Exceptions;
using SecretStore.Domain.Models;
using SecretStore.Web.Attributes;
using SecretStore.Web.DTOs;

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
    [ProducesResponseType(type: typeof(TokensDto), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(type: typeof(string), statusCode: StatusCodes.Status403Forbidden)]
    [ProducesResponseType(type: typeof(string), statusCode: StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login(AuthDto authDto)
    {
        try
        {
            var tokens = await _userService.Login(
                clientId: authDto.ClientId, 
                clientSecret: authDto.ClientSecret);

            return Ok(new TokensDto()
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
            var tokens = await _tokenService.Reissue(refreshToken);
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

    [HttpDelete]
    [Route("logout")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(type: typeof(string), statusCode: StatusCodes.Status400BadRequest)]
    [ProducesResponseType(type: typeof(string), statusCode: StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Logout([FromBody] string token)
    {
        try
        {
            await _tokenService.Delete(token);
            return Ok();
        }
        catch (InvalidRefreshTokenException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error during refresh token!");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }
}