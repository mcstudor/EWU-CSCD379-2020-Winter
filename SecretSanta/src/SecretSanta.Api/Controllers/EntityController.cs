using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Business;
using SecretSanta.Data;

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class EntityController<TEntity> : Controller
    where TEntity : EntityBase
    {
        private IEntityService<TEntity> EntityService { get; }

        public EntityController(IEntityService<TEntity> entityService)
        {
            EntityService = entityService ?? throw new System.ArgumentNullException(nameof(entityService));
        }

        [HttpGet]
        public async Task<IEnumerable<TEntity>> Get()
        {
            List<TEntity> entitys = await EntityService.FetchAllAsync();
            return entitys;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TEntity>> Get(int id)
        {
            if (await EntityService.FetchByIdAsync(id) is TEntity entity)
            {
                return Ok(entity);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<TEntity> Post(TEntity value)
        {
            return await EntityService.InsertAsync(value);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<TEntity>> Put(int id, TEntity value)
        {
            if (await EntityService.UpdateAsync(id, value) is TEntity entity)
            {
                return entity;
            }

            return NotFound();
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            if (await EntityService.DeleteAsync(id))
            {
                return Ok();
            }

            return NotFound();
        }
    }
}