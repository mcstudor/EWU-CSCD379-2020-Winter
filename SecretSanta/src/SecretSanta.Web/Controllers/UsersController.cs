﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Web.Api;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SecretSanta.Web.Controllers
{
    public class UsersController : Controller
    {
        private IHttpClientFactory ClientFactory { get; }
        private const string ApiName = "SecretSantaApi";
        public UsersController(IHttpClientFactory clientFactory)
        {
            if(clientFactory is null) throw new ArgumentNullException(nameof(clientFactory));
            ClientFactory = clientFactory;
        }


        public async Task<IActionResult> Index()
        {
            HttpClient httpClient = ClientFactory.CreateClient(ApiName);
            ICollection<User> users = await new UserClient(httpClient).GetAllAsync();
            return View(users);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(UserInput userInput)
        {
            ActionResult result = View(userInput);

            if (ModelState.IsValid)
            {
                HttpClient httpClient = ClientFactory.CreateClient(ApiName);

                var client = new UserClient(httpClient);
                var createdUser = await client.PostAsync(userInput);

                result = RedirectToAction(nameof(Index));
            }

            return result;
        }

        public async Task<ActionResult> Edit(int id)
        {
            HttpClient httpClient = ClientFactory.CreateClient(ApiName);

            var client = new UserClient(httpClient);
            var fetchedUser = await client.GetAsync(id);

            return View(fetchedUser);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id, UserInput userInput)
        {
            HttpClient httpClient = ClientFactory.CreateClient(ApiName);

            var client = new UserClient(httpClient);
            var updatedUser = await client.PutAsync(id, userInput);

            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> Delete(int id)
        {
            HttpClient httpClient = ClientFactory.CreateClient(ApiName);

            var client = new UserClient(httpClient);

            await client.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
