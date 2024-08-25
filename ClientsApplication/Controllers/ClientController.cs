using ClientsApplication.Data;
using ClientsApplication.Models;
using ClientsApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientsApplication.Controllers
{
    public class ClientController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ClientsApiService _clientsApiService;

        public ClientController(ApplicationDbContext context, ClientsApiService clientsApiService)
        {
            _context = context;
            _clientsApiService = clientsApiService;
        }

        [HttpGet]
        public ActionResult Add()
        {
            var client = new Client();
          
            return View(client);
        }

        [HttpPost]
        public ActionResult Add(Client client)
        {
            var existingClient = _context.Clients.FirstOrDefault(c => c.ClientID == client.ClientID);
            if (existingClient != null)
            {
                ModelState.AddModelError("ClientID", "This ClientID already exists.");
            }
            if (ModelState.IsValid)
            {
                _context.Clients.Add(client);
                _context.SaveChanges(); // Save the changes to the database

                return RedirectToAction("DisplayClients");
            }

            return View(client);
        }

        [HttpGet]
        public async Task<IActionResult> DisplayClients(string searchQuery)
        {
            List<Client> clients;
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                clients = await _clientsApiService.SearchClientsAsync(string.Empty);
            }
            else
            {
                clients = await _clientsApiService.SearchClientsAsync(searchQuery);
            }
            return View(clients);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var client = await _clientsApiService.GetClientByIdAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // POST: Client/Edit/{id}
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Client client)
        {
            if (id != client.ClientID)
            {
                return BadRequest("Client ID mismatch.");
            }

            if (ModelState.IsValid)
            {
                var result = await _clientsApiService.UpdateClientAsync(id, client);
                if (result)
                {
                    return RedirectToAction(nameof(DisplayClients));
                }
                else
                {
                    ModelState.AddModelError("", "Error updating client.");
                }
            }
            return View(client);
        }
    }
}
