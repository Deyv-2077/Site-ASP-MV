using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MVCApp2.Models;
using System.Diagnostics;

namespace MVCApp2.Controllers
{
    public class HomeController : Controller 
    {
        private readonly ILogger<HomeController> _logger;

        private readonly DataContext _context;


        public HomeController(DataContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }


        public IActionResult Search(string query)
        {
            if (query == null)
            {
                // Se a consulta for nula, retorne uma resposta vazia ou uma mensagem de erro adequada
                return View();
            }

            var results = _context.Customers
                .Where(c => c.Name == query || c.Address == query)
                .ToList();

            return View("Search", results);
        }







        public IActionResult IndexListar()
        {
            List<Customer> customers = _context.Customers.ToList();
            return View(customers);
        }


        public IActionResult Index()
        {
            return View();
        }

       
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult IndexCustomers()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}