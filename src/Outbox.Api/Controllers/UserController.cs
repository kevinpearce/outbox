using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Outbox.Api.Models;
using Outbox.Application.Interfaces;

namespace Outbox.Api.Controllers;

[ApiController]
[Route("users")]
public class UserController(IUserService userService, IValidator<CreateUserRequest> validator, ILogger<UserController> logger) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        var result = await validator.ValidateAsync(request, cancellationToken);
        if (!result.IsValid)
        {
            logger.LogWarning("Validation failed for CreateUserRequest: {Errors}", result.Errors);
            return BadRequest(new { errors = result.Errors.Select(e => e.ErrorMessage) });
        }

        try
        {
            var userId = await userService.CreateUserAsync(request.Name, cancellationToken);

            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("User {UserId} created", userId);

            return CreatedAtAction(nameof(GetUserById), new { id = userId }, new { id = userId });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create user with name {Name}", request.Name);
            return StatusCode(500, new { error = "An error occurred while creating the user" });
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var user = await userService.GetUserByIdAsync(id, cancellationToken);
            return user == null ? NotFound() : Ok(user);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get user by id {UserId}", id);
            return StatusCode(500, new { error = "An error occurred while retrieving the user" });
        }
    }
}