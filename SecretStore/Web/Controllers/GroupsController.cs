using Microsoft.AspNetCore.Mvc;
using SecretStore.Domain.Interfaces.Services;
using SecretStore.Domain.Interfaces.Services.Exceptions;
using SecretStore.Web.Attributes;
using SecretStore.Web.DTOs;

namespace SecretStore.Web.Controllers;
[ApiController]
[Route("groups")]
public class GroupsController : Controller
{
    private readonly IGroupService _groupService;
    private readonly ILogger<GroupsController> _logger;
    public GroupsController(IGroupService groupService, ILogger<GroupsController> logger)
    {
        _groupService = groupService;
        _logger = logger;
    }

    [HttpPost]
    [Route("create")]
    [AdminAuth]
    public async Task<IActionResult> Create([FromQuery] string name)
    {
        try
        {
            var id = await _groupService.Create(name);
            return Ok(id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error during create new group!");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }
    
    [HttpPost]
    [AdminAuth]
    [Route("{groupName}/add-user")]
    public async Task<IActionResult> AddUser([FromRoute] string groupName, [FromQuery] string clientId)
    {
        try
        {
            await _groupService.AddUser(groupName, clientId);
            return Ok();
        }
        catch (InvalidClientIdException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error during adding user to group!");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }
    
    [HttpPost]
    [AdminAuth]
    [Route("{groupName}/add-secret")]
    public async Task<IActionResult> AddSecret([FromRoute] string groupName, [FromBody] PushSecretDto pushSecretDto)
    {
        try
        {
            var secret = await _groupService.AddSecret(groupName, pushSecretDto.Name, pushSecretDto.Value);

            return Ok(new SecretDto()
            {
                GroupName = groupName,
                Id = secret.Id,
                Name = secret.Name,
                Occupied = secret.Occupied,
                OccupierId = secret.OccupierId,
                Value = secret.Value
            });
        }
        catch (InvalidGroupException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error during adding secret to group!");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

    [HttpDelete]
    [AdminAuth]
    [Route("{groupName}/secrets/{secretName}")]
    public async Task<IActionResult> DeleteSecret([FromRoute] string groupName, [FromRoute] string secretName)
    {
        try
        {
            await _groupService.DeleteSecret(groupName, secretName);
            return NoContent();
        }
        catch (Exception e) when (e is InvalidGroupException or InvalidSecretException)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error during adding secret to group!");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }
}