using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Web.Api;

namespace SecretSanta.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IHttpClientFactory _HttpClientFactory;

        public UserController(IHttpClientFactory httpClientFactory)
        {
            _HttpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }
        // GET: User
        public async Task<ActionResult> Index()
        {
            HttpClient httpClient = _HttpClientFactory.CreateClient("SecretSantaApi");
            var client = new UserClient(httpClient);
            var users = await client.GetAllAsync();
            return View(users);
        }
    }
}