using AutoMapper;
using FluentValidation;
using Homemap.ApplicationCore.Interfaces.Services;
using Homemap.ApplicationCore.Models;
using Homemap.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Homemap.WebAPI.Controllers
{
    [Route("api/devices/{deviceId}/state")]
    [ApiController]
    public class DeviceStatesController : ControllerBase
    {
        private readonly IDeviceService _deviceService;

        private readonly IValidator<DeviceStateDto> _validator;

        public DeviceStatesController(IMapper mapper, IDeviceService deviceService, IValidator<DeviceStateDto> validator)
        {
            _deviceService = deviceService;
            _validator = validator;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DeviceStateDto>> GetDeviceStateById(int deviceId, CancellationToken cancellationToken)
        {
            var dtoOrError = await _deviceService.GetDeviceStateByIdAsync(deviceId, cancellationToken);

            if (dtoOrError.IsError)
                return this.ErrorOf(dtoOrError.FirstError);

            return dtoOrError.Value;
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SetDeviceStateById(int deviceId, [FromBody] DeviceStateDto deviceState)
        {
            var validationResult = await _validator.ValidateAsync(deviceState);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
                return BadRequest(ModelState);
            }

            var updatedOrError = await _deviceService.SetDeviceStateByIdAsync(deviceId, deviceState);

            if (updatedOrError.IsError)
                return this.ErrorOf(updatedOrError.FirstError);

            return NoContent();
        }
    }
}
