using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MVCApp2.Helper;
using MVCApp2.Migrations;
using MVCApp2.Models;
using MVCApp2.Repositorio;
using System.Diagnostics;
using MVCApp2.Filtros;

namespace MVCApp2.Controllers
{
    public class HomeController : Controller 
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IUsuarioRepositorio _usuarioRepositorio;

        private readonly ISessao _sessao;

        private readonly DataContext _context;


        public HomeController(IUsuarioRepositorio usuarioRepositorio, DataContext context, ISessao sessao, ILogger<HomeController> logger)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _context = context;
            _logger = logger;
            _sessao = sessao;
        }


        public IActionResult Search(string query)
        {
            if (query == null)
            {
                // Se a consulta for nula, retorne uma resposta vazia ou uma mensagem de erro adequada
                return View();
            }

            var results = _context.Customers
                .Where(c => c.Name.StartsWith(query.Substring(0, 1)) || c.Address == query)
                .ToList();

            return View("Search", results);
        }



        public IActionResult Home1()
        {
            if(_sessao.BuscarSessaoDoUsuario() != null) return RedirectToAction("Index", "Search");
            return View();
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

        [HttpPost]
        public IActionResult Entrar(LoginModel loginModel, UserModel2 userBd)
        {
            try
            {
                if(loginModel.Login == userBd.Login && loginModel.Senha == userBd.Senha)
                {

                    return View("Custome","Customerss");


                }

                return View("IndexListar");
            }
            catch (Exception erro) 
            {

                TempData["MensagemErro"] = $"Ops, não conseguimos realizar o seu login: {erro.Message}";
                return RedirectToAction("Index");


            }

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