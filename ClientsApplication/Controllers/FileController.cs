using ClientsApplication.Data;
using ClientsApplication.Helpers;
using ClientsApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace ClientsApplication.Controllers
{
    public class FileController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FileController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult ImportFromXml()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportFromXml(IFormFile xmlFile)
        {
            if (xmlFile != null && xmlFile.Length > 0)
            {
                using (var stream = xmlFile.OpenReadStream())
                {
                    var clients = FileHelper.ParseXml(stream);
                    foreach (var client in clients)
                    {
                        var existingClient = _context.Clients.FirstOrDefault(c => c.ClientID == client.ClientID);
                        if (existingClient != null)
                        {
                            //If the client already exists continue with others 
                            ModelState.AddModelError("", $"Client with ID {client.ClientID} already exists.");
                            continue; 
                        }
                        
                            _context.Clients.Add(client);
                    }
                    _context.SaveChanges();
                }

                return RedirectToAction("DisplayClients", "Client");
            }

            ModelState.AddModelError("", "Please upload a valid XML file.");
            return View();
        }

        

        [HttpGet]
        public ActionResult ExportToJson()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ExportToJson(string sortBy)
        {
            List<Client> clients = _context.Clients.Include(c => c.Addresses).ToList();

            switch (sortBy)
            {
                case "Name":
                    clients = clients.OrderBy(c => c.Name).ToList();
                    break;
                case "BirthDate":
                    clients = clients.OrderBy(c => c.BirthDate).ToList();
                    break;
                default:
                    clients = clients.ToList(); 
                    break;
            }

            //Serialize the list of clients to a JSON 
            var json = JsonConvert.SerializeObject(clients, (Newtonsoft.Json.Formatting)System.Xml.Formatting.Indented);

            // Return the JSON file 
            var fileName = $"Clients_{sortBy}.json";
            var contentType = "application/json";
            var fileBytes = System.Text.Encoding.UTF8.GetBytes(json);

            return File(fileBytes, contentType, fileName);
        }
    }
}
