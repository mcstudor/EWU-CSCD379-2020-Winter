using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Web.Api;

namespace SecretSanta.Web.Controllers
{
    public class GiftController : Controller
    {
        private readonly IHttpClientFactory _HttpClientFactory;

        public GiftController(IHttpClientFactory httpClientFactory)
        {
            _HttpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }
        // GET: User
        public async Task<ActionResult> Index()
        {
            HttpClient httpClient = _HttpClientFactory.CreateClient("SecretSantaApi");
            var client = new GiftClient(httpClient);
            var gifts = await client.GetAllAsync();
            return View(gifts);
        }
    }
}