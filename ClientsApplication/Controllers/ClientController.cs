using ClientsApplication.Data;
using ClientsApplication.Models;
using ClientsApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientsApplication.Controllers
{
    public class ClientController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ClientsApiService _clientsApiService;

        private readonly ILogger<ClientController> _logger;

        public ClientController(ApplicationDbContext context, ClientsApiService clientsApiService, ILogger<ClientController> logger)
        {
            _context = context;
            _clientsApiService = clientsApiService;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Add()
        {
            try
            {
                _logger.LogInformation("GET /Client/Add Request recieved...");

                var client = new Client();
                return View(client);
            }
            catch(Exception ex)
            {
                _logger.LogError("Error occured: " + ex.Message + " at " + ex.StackTrace);

                ModelState.AddModelError("", "An error occurred while trying to load the Add Client page");
                return View();
            }
        }

        [HttpPost]
        public ActionResult Add(Client client)
        {
            try
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
            }
            catch(Exception ex)
            {
                _logger.LogError("Error occured: " + ex.Message + " at " + ex.StackTrace);

                ModelState.AddModelError("", "An error occurred while adding new client");
            }

            return View(client);
        }

        [HttpGet]
        public async Task<IActionResult> DisplayClients(string searchQuery)
        {
            try
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
            catch(Exception ex)
            {
                _logger.LogError("Error occured: " + ex.Message + " at " + ex.StackTrace);

                ModelState.AddModelError("", "An error occurred while displaying clients");
                return View(new List<Client>());
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var client = await _clientsApiService.GetClientByIdAsync(id);
                if (client == null)
                {
                    return NotFound();
                }
                return View(client);
            }
            catch(Exception ex)
            {
                _logger.LogError("Error occured: " + ex.Message + " at " + ex.StackTrace);

                ModelState.AddModelError("", "An error occurred while trying to load the Edit Client page");
                return View();
            }
        }

        // POST: Client/Edit/{id}
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Client client)
        {
            try
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
            }
            catch(Exception ex)
            {
                _logger.LogError("Error occured: " + ex.Message + " at " + ex.StackTrace);

                ModelState.AddModelError("", "An error occurred while editing client");
            }
            return View(client);
        }
    }
}
