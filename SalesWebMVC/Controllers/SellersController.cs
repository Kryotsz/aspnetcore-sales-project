using Microsoft.AspNetCore.Mvc;
using SalesWebMVC.Models;
using SalesWebMVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMVC.Controllers
{
    public class SellersController : Controller
    {
        // declarar dependência para o SellerService
        private readonly SellerService _sellerService;

        // construtor para injetar a dependência
        public SellersController(SellerService sellerService)
        {
            _sellerService = sellerService;
        }

        public IActionResult Index()
        {
            // chama o método que retorna a lista de Sellers
            var list = _sellerService.FindAll();
            // passa a lista para a view
            return View(list);
        }

        public IActionResult Create()
        {
            return View();
        }

        // anotation pra definir que é um método POST
        [HttpPost]
        // anotation que previne ataques CSRF
        [ValidateAntiForgeryToken]
        public IActionResult Create(Seller seller)
        {
            // chama o método pra inserir o vendedor no banco de dados
            _sellerService.Insert(seller);
            // retorna pro index
            // o "nameof" permite que, mesmo que a action Index tenha o nome alterado ele ainda reconheça o index
            return RedirectToAction(nameof(Index));
        }
    }
}
