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
    public class GiftController : Controller
    {
        private IGiftService GiftService { get; }

        public GiftController(IGiftService giftService)
        {
            GiftService = giftService ?? throw new System.ArgumentNullException(nameof(giftService));
        }

        [HttpGet]
        public async Task<IEnumerable<Gift>> Get()
        {
            List<Gift> gifts = await GiftService.FetchAllAsync();
            return gifts;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Gift>> Get(int id)
        {
            if (await GiftService.FetchByIdAsync(id) is Gift gift)
            {
                return Ok(gift);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<Gift> Post(Gift value)
        {
            return await GiftService.InsertAsync(value);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Gift>> Put(int id, Gift value)
        {
            if (await GiftService.UpdateAsync(id, value) is Gift gift)
            {
                return gift;
            }

            return NotFound();
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            if (await GiftService.DeleteAsync(id))
            {
                return Ok();
            }

            return NotFound();
        }

    }
}