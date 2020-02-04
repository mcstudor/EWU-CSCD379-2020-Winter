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
    public class GroupController : Controller
    {
        private IGroupService GroupService { get; }

        public GroupController(IGroupService groupService)
        {
            GroupService = groupService ?? throw new System.ArgumentNullException(nameof(groupService));
        }

        [HttpGet]
        public async Task<IEnumerable<Group>> Get()
        {
            List<Group> groups = await GroupService.FetchAllAsync();
            return groups;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Group>> Get(int id)
        {
            if (await GroupService.FetchByIdAsync(id) is Group group)
            {
                return Ok(group);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<Group> Post(Group value)
        {
            return await GroupService.InsertAsync(value);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Group>> Put(int id, Group value)
        {
            if (await GroupService.UpdateAsync(id, value) is Group group)
            {
                return group;
            }

            return NotFound();
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            if (await GroupService.DeleteAsync(id))
            {
                return Ok();
            }

            return NotFound();
        }

    }
}