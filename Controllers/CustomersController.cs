using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MVCApp2.Models;
using MVCApp2.Repositorio;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Drawing;
using MVCApp2.Helper;

namespace MVCApp2.Controllers
{
    public class CustomersController : Controller
    {
        private readonly DataContext _context;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ILogger<CustomersController> _logger;
        private readonly ISessao _sessao;
      

        public CustomersController(DataContext context, ILogger<CustomersController> logger, IUsuarioRepositorio usuarioRepositorio, ISessao sessao)
        {
            _context = context;


            _logger = logger;


            _usuarioRepositorio = usuarioRepositorio;

            _sessao = sessao;



        }

        // GET: Customers
        public async Task<IActionResult> IndexCustomers()
        {
              return _context.Customers != null ? 
                          View(await _context.Customers.ToListAsync()) :
                          Problem("Entity set 'DataContext.Customers'  is null.");
        }


        // UserModel2 userBd)
        //  public async Task<IActionResult> Entrar(LoginModel loginModel, UserModel2 userBd)

        // Solução Login C#, JavaScript e Json (Trazer resultados do BD, tela Login)
        //Este método identifica que ao fazer o login, os valores digitados em login e senha serão
        //comparados com os valores que existem no BD e irá trazer o resultado.
        [HttpPost]
      /*  public ActionResult Entrar(LoginModel loginModel, UserModel2 userBd)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(loginModel.Login) && !string.IsNullOrEmpty(loginModel.Senha))
                {
                    string connectionString = "Server=localhost; Database=MVCApp2; Encrypt=False; Trusted_Connection=true;";
                    string query = "SELECT Login, Senha FROM Users WHERE Login = @Login AND Senha = @Senha;";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Login", loginModel.Login);
                            command.Parameters.AddWithValue("@Senha", loginModel.Senha);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                string login = "";
                                string senha = "";

                                if (reader.Read())
                                {
                                    login = reader.GetString(0); // Obtém o valor da coluna "Login"
                                    senha = reader.GetString(1); // Obtém o valor da coluna "Senha"
                                }

                                // Criar um objeto anônimo com os resultados
                                var result = new
                                {
                                    Login = login,
                                    Senha = senha
                                };

                                // Retornar os resultados em formato JSON
                                return Json(result);
                            }
                        }
                    }
                }
            }

            return Json(new { Success = false, Message = "Por favor, preencha o login e a senha." });
        }

        */

        // Solução Login C#, JavaScript e Json (Permitir Login)
          //Este método identifica que ao fazer o login, os valores digitados em login e senha serão
            //comparados com os valores que existem no BD e se identificar os valores iguais nas colunas, o login será efetuado
        //mas se tiver valores nulos ou vazios o login não será permitido.
    public IActionResult Entrar(LoginModel loginModel, UserModel2 userBd)
    {
        if (ModelState.IsValid)
        {
            if (!string.IsNullOrEmpty(loginModel.Login) && !string.IsNullOrEmpty(loginModel.Senha))
            {
                    string connectionString = "Server = localhost; Database = MVCApp2; Encrypt = False; Trusted_Connection = true;";
                string query = "SELECT COUNT(*) FROM Users WHERE login = @Login AND senha = @Senha;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Login", loginModel.Login);
                        command.Parameters.AddWithValue("@Senha", loginModel.Senha);

                        int count = (int)command.ExecuteScalar();

                        if (count > 0)
                        {
                                //_sessao.CriarSessaoDoUsuario(loginModel);
                            return RedirectToAction("IndexProducts", "Products");
                        }
                    }
                }

                return View("Home1");
            }
        }

        return View("SearchResults");
    }
        ////////
        

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
       // public async Task<IActionResult> Create([Bind("Id,Name,Login,Senha,Email,Perfil,DataCadastro,DataAtualizacao")] UserModel2 user)
        public UserModel2 Create([Bind("Id,Name,Login,Senha,Email,Perfil,DataCadastro,DataAtualizacao")] UserModel2 user)
        {

            user.DataCadastro = DateTime.Now;
            user.DataAtualizacao = DateTime.Now;

            _context.Users.Add(user);
                    _context.SaveChanges();
                    return user;
            

        }



        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Address")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Customers == null)
            {
                return Problem("Entity set 'DataContext.Customers'  is null.");
            }
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
          return (_context.Customers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
