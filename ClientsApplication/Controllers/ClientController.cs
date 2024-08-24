using ClientsApplication.Data;
using ClientsApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ClientsApplication.Controllers
{
    public class ClientController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientController(ApplicationDbContext context)
        {
            _context = context;
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
            if (ModelState.IsValid)
            {
                _context.Clients.Add(client);
                _context.SaveChanges(); // Save the changes to the database

                return RedirectToAction("DisplayClients");
            }

            return View(client);
        }

        [HttpGet]
        public ActionResult DisplayClients()
        {
            // Retrieve the list of clients from database
            var clients = _context.Clients.Include(c => c.Addresses).ToList(); 

            return View(clients);
        }
    }
}
