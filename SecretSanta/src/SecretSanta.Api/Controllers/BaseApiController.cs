using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Business;
using SecretSanta.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecretSanta.Business.Dto;
using SecretSanta.Business.Services;

namespace SecretSanta.Api.Controllers
{
    public abstract class BaseApiController<TDto, TInput> : ControllerBase 
        where TInput : class
        where TDto: class, TInput, IEntity
    {
        protected IEntityService<TDto,TInput> Service { get; }

        protected BaseApiController(IEntityService<TDto, TInput> service)
        {
            Service = service ?? throw new ArgumentNullException(nameof(service));
        }

       [HttpGet]
        public async Task<IEnumerable<TDto>> Get() => await Service.FetchAllAsync();

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Get(int id)
        {
            TDto entity = await Service.FetchByIdAsync(id);
            if (entity is null)
            {
                return NotFound();
            }
            return Ok(entity);
        }

        [HttpPut("{id}")]
        public async Task<TDto?> Put(int id, [FromBody] TInput value)
        {
            return await Service.UpdateAsync(id, value);
        }

        [HttpPost]
        public async Task<TDto> Post(TInput value)
        {
            return await Service.InsertAsync(value);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Delete(int id)
        {
            if (await Service.DeleteAsync(id))
            {
                return Ok();
            }
            return NotFound();
        }
    }
}