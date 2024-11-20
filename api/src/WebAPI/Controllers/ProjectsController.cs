using ErrorOr;
using FluentValidation;
using Homemap.ApplicationCore.Interfaces.Services;
using Homemap.ApplicationCore.Models;
using Homemap.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Homemap.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _service;

        private readonly IValidator<ProjectDto> _validator;

        public ProjectsController(IProjectService service, IValidator<ProjectDto> validator)
        {
            _service = service;
            _validator = validator;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<ProjectDto>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProjectDto>> GetById(int id)
        {
            ErrorOr<ProjectDto> dtoOrError = await _service.GetByIdAsync(id);

            if (dtoOrError.IsError)
                return this.ErrorOf(dtoOrError.FirstError);

            return dtoOrError.Value;
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProjectDto>> Create([FromBody] ProjectDto dto)
        {
            var validationResult = await _validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);

                return BadRequest(ModelState);
            }

            dto = await _service.CreateAsync(dto);

            return CreatedAtAction(nameof(Create), new { dto.Id }, dto);
        }

        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProjectDto>> Update(int id, [FromBody] ProjectDto dto)
        {
            var validationResult = await _validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);

                return BadRequest(ModelState);
            }

            ErrorOr<ProjectDto> dtoOrError = await _service.UpdateAsync(id, dto);

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
}
