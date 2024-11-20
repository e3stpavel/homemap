using ErrorOr;
using FluentValidation;
using Homemap.ApplicationCore.Interfaces.Services;
using Homemap.ApplicationCore.Models;
using Homemap.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Homemap.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DevicesController : ControllerBase
{
    private readonly IDeviceService _service;

    private readonly IValidator<DeviceDto> _deviceValidator;

    public DevicesController(
        IDeviceService deviceService,
        IValidator<DeviceDto> deviceValidator
    )
    {
        _service = deviceService;
        _deviceValidator = deviceValidator;
    }

    [HttpGet("/api/receivers/{receiverId}/[controller]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllByReceiver(int receiverId)
    {
        var dtoOrError = await _service.GetAllAsync(receiverId);

        if (dtoOrError.IsError)
            return this.ErrorOf(dtoOrError.FirstError);

        return Ok(dtoOrError.Value);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DeviceDto>> GetById(int id)
    {
        ErrorOr<DeviceDto> dtoOrError = await _service.GetByIdAsync(id);

        if (dtoOrError.IsError)
            return this.ErrorOf(dtoOrError.FirstError);

        return dtoOrError.Value;
    }

    [HttpPost("/api/receivers/{receiverId}/[controller]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateDevice(int receiverId, [FromBody] DeviceDto dto)
    {
        var validationResult = await _deviceValidator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);

            return BadRequest(ModelState);
        }

        var dtoOrError = await _service.CreateAsync(receiverId, dto);

        return dtoOrError.MatchFirst(
            createdDevice => CreatedAtAction(nameof(CreateDevice), new { createdDevice.Id }, createdDevice),
            firstError => this.ErrorOf(firstError)
        );
    }

    [HttpPut("{id}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DeviceDto>> Update(int id, [FromBody] DeviceDto dto)
    {
        var validationResult = await _deviceValidator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);

            return BadRequest(ModelState);
        }

        ErrorOr<DeviceDto> dtoOrError = await _service.UpdateAsync(id, dto);

        if (dtoOrError.IsError)
            return this.ErrorOf(dtoOrError.FirstError);

        return dtoOrError.Value;
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        ErrorOr<Deleted> deletedOrError = await _service.DeleteAsync(id);

        if (deletedOrError.IsError)
            return this.ErrorOf(deletedOrError.FirstError);

        return NoContent();
    }
}
