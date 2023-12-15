using Microsoft.AspNetCore.Mvc;
using SalesWebMVC.Models;
using SalesWebMVC.Models.ViewModels;
using SalesWebMVC.Services;
using SalesWebMVC.Services.Exceptions;
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

        // GET Create
        // método Create que leva os dados pra view Create
        public IActionResult Create()
        {
            // chama o método que obtém os departamentos ordenados por nome
            var departments = _departmentService.FindAll();
            var viewModel = new SellerFormViewModel { Departments = departments };
            // passa o objeto viewModel que contém todos os departamentos
            return View(viewModel);
        }

        // POST Create
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

        // GET Delete
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

        // POST Delete
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

        // GET Details
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

        // GET Edit
        // o Id é obrigatório, porém utiliza-se como opcional para evitar erros de execução
        public IActionResult Edit(int? id)
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

            // recebe todos os departamentos
            List<Department> departments = _departmentService.FindAll();
            // recebe o vendedor e a lista de departamentos
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments };
            // leva pra view Edit, os dados da viewModel
            return View(viewModel);
        }

        // POST Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Seller seller)
        {
            if (id != seller.Id)
            {
                return BadRequest();
            }

            try
            {
                _sellerService.Update(seller);
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (DbConcurrencyException)
            {
                return BadRequest();
            }
        }
    }
}
