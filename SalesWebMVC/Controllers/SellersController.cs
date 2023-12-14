using Microsoft.AspNetCore.Mvc;
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
    }
}
