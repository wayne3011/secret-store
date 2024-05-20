using Microsoft.AspNetCore.Mvc;
using SecretStore.Domain.Interfaces.Services;
using SecretStore.Domain.Interfaces.Services.Exceptions;
using SecretStore.Web.Controllers.Extensions;

namespace SecretStore.Web.Controllers;

[Route("secrets")]
public class SecretController : Controller
{
    private readonly ISecretService _secretService;
    private readonly IGroupService _groupService;
    private readonly ILogger<SecretController> _logger;
    
    public SecretController(ISecretService secretService, IGroupService groupService, ILogger<SecretController> logger)
    {
        _secretService = secretService;
        _groupService = groupService;
        _logger = logger;
    }

    [HttpGet]
    [Route("check")]
    public async Task<IActionResult> Check([FromQuery] string groupName, [FromQuery] string secretName)
    {
        try
        {
            var userId = HttpContext.GetUserId();
            if (!await _groupService.CheckHaveAccess(userId, groupName))
            {
                return StatusCode(StatusCodes.Status403Forbidden, "User in not belong to a group.");
            }

            bool isBusy = await _secretService.CheckBusy(secretName, groupName);

            return Ok(isBusy);
        }
        catch (Exception e) when (e is InvalidGroupException or InvalidSecretException)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error during checking that secret is busy!");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }
    
    [HttpPost]
    [Route("occupy")]
    public async Task<IActionResult> Occupy([FromQuery] string groupName, [FromQuery] string secretName)
    {
        try
        {
            var userId = HttpContext.GetUserId();

            if (!await _groupService.CheckHaveAccess(userId, groupName))
            {
                return StatusCode(StatusCodes.Status403Forbidden, "User in not belong to a group.");
            }
            
            await _secretService.Occupy(secretName, groupName, userId);

            return Ok();
        }
        catch (Exception e) when (e is InvalidGroupException or InvalidSecretException)
        {
            return BadRequest(e.Message);
        }
        catch (SecretAlreadyOccupiedException e)
        {
            return StatusCode(StatusCodes.Status403Forbidden, e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error during occupying secret");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }
    
    [HttpPost]
    [Route("release")]
    public async Task<IActionResult> Release([FromQuery] string groupName, [FromQuery] string secretName)
    {
        try
        {
            var userId = HttpContext.GetUserId();

            if (!await _groupService.CheckHaveAccess(userId, groupName))
            {
                return StatusCode(StatusCodes.Status403Forbidden, "User in not belong to a group.");
            }
            
            await _secretService.Release(secretName, groupName, userId);

            return Ok();
        }
        catch (Exception e) when (e is InvalidGroupException or InvalidSecretException)
        {
            return BadRequest(e.Message);
        }
        catch (SecretPermissionDeniedException e)
        {
            return StatusCode(StatusCodes.Status403Forbidden, e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error during release secret");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }
}