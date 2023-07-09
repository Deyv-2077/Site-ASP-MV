using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCApp2.Filtros;
using MVCApp2.Models;

namespace MVCApp2.Controllers
{
    [PaginaParaUsuarioLogado]

    public class ProductsController : Controller
    {
        private readonly DataContext _context;

        public ProductsController(DataContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> IndexProducts()
        {
            return _context.Products != null ?
                        View(await _context.Products.ToListAsync()) :
                        Problem("Entity set 'DataContext.Products'  is null.");
        }

        public IActionResult ImprimirNumeros()
        {
            List<string> numeros = new List<string>();
            for (int i = 0; i <= 100; i++)
            {
                if (i == 5 || i == 3)
                {
                    numeros.Add("Fizbuzz");
                }
                else if (i % 3 == 1)
                {
                    numeros.Add("Fizz");
                }
                else if (i % 5 == 0)
                {
                    numeros.Add("Buzz");
                }
                else
                {
                    numeros.Add(i.ToString());
                }
            }


            int[] numeros2 = { 1, 2, 3, 3, 4, 4, 4, 6 };

            Dictionary<int, int> ocorrencias = new Dictionary<int, int>();

            foreach (int numero in numeros2)
            {

                if (ocorrencias.ContainsKey(numero))
                {
                    ocorrencias[numero]++;
                }
                else
                {
                    ocorrencias[numero] = 1;
                }

            }

            foreach (var ocorrencia in ocorrencias)
            {
                if (ocorrencia.Value > 1)
                {
                    Console.WriteLine($"O número {ocorrencia.Key} aparece {ocorrencia.Value} vezes.");
                }
            }

            ViewBag.Ocorrencias = ocorrencias;


            return View();



        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Address,Valor,Quantidade")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexProducts));
            }
            return View(product);
        }
    



        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }


        // GET: Products/Vender/5
        //[Route("Products/Vender/{id}")]
        [HttpGet]
        public async Task<IActionResult> Vender(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            product.Quantidade = 0;

            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address,Valor,Quantidade")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(IndexProducts));
            }
            return View(product);
        }


        [HttpPost, ActionName("VenderConfirmado")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VenderConfirmado(int id, [Bind("Id,Name,Address,Valor,Quantidade")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingProduct = await _context.Products.FindAsync(product.Id);

                if (product.Quantidade > existingProduct.Quantidade)
                {
                    ModelState.AddModelError("Quantidade", "A quantidade desejada não está disponível.");
                    return View(product);
                }

                existingProduct.Quantidade -= product.Quantidade;

                await _context.SaveChangesAsync();

                decimal valorUnitario = decimal.Parse(Request.Form["Valor"]);
                var produtoNaSacola = $"{existingProduct.Name}\nValor: R$ {valorUnitario}\nQuantidade: {product.Quantidade}";

                // Obter os produtos da sacola do cookie
                string produtosNaSacola = HttpContext.Request.Cookies["ProdutosNaSacola"];
                List<string> produtos = produtosNaSacola?.Split(';').ToList() ?? new List<string>();

                bool produtoExistente = false; // Variável para verificar se o item já existe na sacola

                for (int i = 0; i < produtos.Count; i++)
                {
                    var detalhes = produtos[i].Split('\n');
                    if (detalhes.Length >= 3)
                    {
                        var itemNome = detalhes[0];
                        if (itemNome == existingProduct.Name)
                        {
                            // O produto com o mesmo nome já existe na sacola
                            var itemValorString = detalhes[1].Replace("Valor: R$ ", "").Replace(".", ",");
                            var itemQuantidadeString = detalhes[2].Replace("Quantidade: ", "");

                            decimal itemValor;
                            int itemQuantidade;
                            if (decimal.TryParse(itemValorString, out itemValor) && int.TryParse(itemQuantidadeString, out itemQuantidade))
                            {
                                int novaQuantidade = itemQuantidade + product.Quantidade;
                                decimal novoValor = itemValor + (valorUnitario * product.Quantidade);

                                produtos[i] = $"{existingProduct.Name}\nValor: R$ {novoValor}\nQuantidade: {novaQuantidade}";

                                produtoExistente = true;
                                break;
                            }
                        }
                    }
                }

                if (!produtoExistente)
                {
                    produtos.Add(produtoNaSacola);
                }

                // Armazenar a lista de produtos da sacola em um cookie
                HttpContext.Response.Cookies.Append("ProdutosNaSacola", string.Join(";", produtos));

                return RedirectToAction("Sacola", "Products");
            }

            return View(product);
        }






        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdicionarItemSacola()
        {
            var nome = Request.Form["Name"];
            var valorString = Request.Form["Valor"];
            var quantidadeString = Request.Form["Quantidade"];

            // Obter os produtos da sacola do cookie
            string produtosNaSacola = HttpContext.Request.Cookies["ProdutosNaSacola"];
            List<string> produtos = produtosNaSacola?.Split(';').ToList() ?? new List<string>();

            bool produtoExistente = false; // Variável para verificar se o item já existe na sacola

            for (int i = 0; i < produtos.Count; i++)
            {
                var detalhes = produtos[i].Split('\n');
                if (detalhes.Length >= 3)
                {
                    var itemNome = detalhes[0];
                    if (itemNome == nome)
                    {
                        // O produto com o mesmo nome já existe na sacola
                        var itemValorString = detalhes[1].Replace("Valor: R$ ", "").Replace(".", ",");
                        var itemQuantidadeString = detalhes[2].Replace("Quantidade: ", "");

                        decimal itemValor;
                        int itemQuantidade;
                        if (decimal.TryParse(itemValorString, out itemValor) && int.TryParse(itemQuantidadeString, out itemQuantidade))
                        {
                            int novaQuantidade = itemQuantidade + int.Parse(quantidadeString);
                            decimal novoValor = itemValor + (decimal.Parse(valorString) * int.Parse(quantidadeString));

                            produtos[i] = $"{nome}\nValor: R$ {novoValor}\nQuantidade: {novaQuantidade}";

                            produtoExistente = true;
                            break;
                        }
                    }
                }
            }

            if (!produtoExistente)
            {
                var produtoNaSacola = $"{nome}\nValor: R$ {valorString}\nQuantidade: {quantidadeString}";

                produtos.Insert(0, produtoNaSacola);
            }

            // Armazenar a lista de produtos da sacola em um cookie
            HttpContext.Response.Cookies.Append("ProdutosNaSacola", string.Join(";", produtos));

            return RedirectToAction("Sacola");
        }




        public IActionResult Sacola()
        {
            string produtosNaSacola = HttpContext.Request.Cookies["ProdutosNaSacola"];
            List<string> produtos = produtosNaSacola?.Split(';').ToList() ?? new List<string>();

            // Ordenar a lista de produtos da sacola pelo nome do produto
            produtos.Sort();

            return View(produtos);
        }





        /*  [HttpPost, ActionName("VenderConfirmado")]
          [ValidateAntiForgeryToken]
          public async Task<IActionResult> VenderConfirmado(int id, [Bind("Id,Name,Address,Valor,Quantidade")] Product product)
          {
              if (id != product.Id)
              {
                  return NotFound();
              }

              if (ModelState.IsValid)
              {
                  try
                  {
                      //Pegar os valores de todas as colunas no banco 
                      var existingProduct = await _context.Products.FindAsync(id);
                      //Guardando a quantidade existente da coluna Quantidade no banco de dados na variável
                      int quantidadeNoBanco = existingProduct.Quantidade;

                      //product.Quantidade é o que foi digitado no form-group
                      int subtrairQuantidade = quantidadeNoBanco - product.Quantidade;
                      //Agora pegando o resultado da subtração e passando para a propriedade Quantidade
                      existingProduct.Quantidade = subtrairQuantidade;
                      //Agora mandando o objeto atualizar no banco de dados
                      if (existingProduct.Quantidade < 0)
                      {
                          return NotFound();

                      }
                      else
                      {
                          _context.Update(existingProduct);
                          await _context.SaveChangesAsync();


                          var produtoNaSacola = new Product
                          {
                              Name = existingProduct.Name,
                              Valor = existingProduct.Valor,
                              Quantidade = product.Quantidade
                          };

                          TempData["ProdutoNaSacola"] = produtoNaSacola;

                          return RedirectToAction("Sacola");
                      }
                  }
                  catch (DbUpdateConcurrencyException)
                  {
                      if (!ProductExists(product.Id))
                      {
                          return NotFound();
                      }
                      else
                      {
                          throw;
                      }
                  }
                  return RedirectToAction(nameof(IndexProducts));
              }
              return View(product);
          }

          */

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'DataContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexProducts));
        }

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
