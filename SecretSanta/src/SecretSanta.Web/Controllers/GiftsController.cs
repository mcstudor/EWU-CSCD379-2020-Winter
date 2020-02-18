using Microsoft.AspNetCore.Mvc;
using SecretSanta.Web.Api;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SecretSanta.Web.Controllers
{
    public class GiftsController : Controller
    {
        private IHttpClientFactory ClientFactory { get; }
        private const string ApiName = "SecretSantaApi";
        public GiftsController(IHttpClientFactory clientFactory)
        {
            if (clientFactory is null) throw new ArgumentNullException(nameof(clientFactory));
            ClientFactory = clientFactory;
        }


        public async Task<IActionResult> Index()
        {
            HttpClient httpClient = ClientFactory.CreateClient(ApiName);
            ICollection<Gift> gifts = await new GiftClient(httpClient).GetAllAsync();
            return View(gifts);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(GiftInput giftInput)
        {
            ActionResult result = View(giftInput);

            if (ModelState.IsValid)
            {
                HttpClient httpClient = ClientFactory.CreateClient(ApiName);

                var client = new GiftClient(httpClient);
                var createdGift = await client.PostAsync(giftInput);

                result = RedirectToAction(nameof(Index));
            }

            return result;
        }

        public async Task<ActionResult> Edit(int id)
        {
            HttpClient httpClient = ClientFactory.CreateClient(ApiName);

            var client = new GiftClient(httpClient);
            var fetchedGift = await client.GetAsync(id);

            return View(fetchedGift);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id, GiftInput giftInput)
        {
            HttpClient httpClient = ClientFactory.CreateClient(ApiName);

            var client = new GiftClient(httpClient);
            var updatedGift = await client.PutAsync(id, giftInput);

            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> Delete(int id)
        {
            HttpClient httpClient = ClientFactory.CreateClient(ApiName);

            var client = new GiftClient(httpClient);

            await client.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}