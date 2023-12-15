using Microsoft.AspNetCore.Mvc;
using SalesWebMVC.Models;
using SalesWebMVC.Models.ViewModels;
using SalesWebMVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMVC.Controllers
{
    public class SellersController : Controller
    {
        // declarar dependências
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;

        // construtor para injetar a dependência
        public SellersController(SellerService sellerService, DepartmentService departmentService)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }

        public IActionResult Index()
        {
            // chama o método que retorna a lista de Sellers
            var list = _sellerService.FindAll();
            // passa a lista para a view
            return View(list);
        }

        // método Create que leva os dados pra view Create
        public IActionResult Create()
        {
            // chama o método que obtém os departamentos ordenados por nome
            var departments = _departmentService.FindAll();
            var viewModel = new SellerFormViewModel { Departments = departments };
            // passa o objeto viewModel que contém todos os departamentos
            return View(viewModel);
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

        // Delete GET
        // recebe um int opcional
        // método leva leva o item a ser deletado pra view Delete
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // como esse id é um nullable (pode ser nulo), tem que utilizar o id.Value para pegar o valor caso exista
            var obj = _sellerService.FindById(id.Value);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // método Delete, que por ser POST, vai deletar os dados
        public IActionResult Delete(int id)
        {
            // chama o método que irá deletar o item desse Id
            _sellerService.Remove(id);
            // depois de deletar, retorna pra Index
            return RedirectToAction(nameof(Index));
        }

        // Details GET
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = _sellerService.FindById(id.Value);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }
    }
}
