﻿using Microsoft.AspNetCore.Mvc;
using SalesWebMVC.Models;
using SalesWebMVC.Models.ViewModels;
using SalesWebMVC.Services;
using SalesWebMVC.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public async Task<IActionResult> Index()
        {
            // chama o método que retorna a lista de Sellers
            var list = await _sellerService.FindAllAsync();
            // passa a lista para a view
            return View(list);
        }

        // GET Create
        // método Create que leva os dados pra view Create
        public async Task<IActionResult> Create()
        {
            // chama o método que obtém os departamentos ordenados por nome
            var departments = await _departmentService.FindAllAsync();
            var viewModel = new SellerFormViewModel { Departments = departments };
            // passa o objeto viewModel que contém todos os departamentos
            return View(viewModel);
        }

        // POST Create
        // anotation pra definir que é um método POST
        [HttpPost]
        // anotation que previne ataques CSRF
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Seller seller)
        {
            // validação a nivel de servidor, caso o usuário desative o javascript
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel);
            }
            // chama o método pra inserir o vendedor no banco de dados
            await _sellerService.InsertAsync(seller);
            // retorna pro index
            // o "nameof" permite que, mesmo que a action Index tenha o nome alterado ele ainda reconheça o index
            return RedirectToAction(nameof(Index));
        }

        // GET Delete
        // recebe um int opcional
        // método leva leva o item a ser deletado pra view Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            // como esse id é um nullable (pode ser nulo), tem que utilizar o id.Value para pegar o valor caso exista
            var obj = await _sellerService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            return View(obj);
        }

        // POST Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        // método Delete, que por ser POST, vai deletar os dados
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // chama o método que irá deletar o item desse Id
                await _sellerService.RemoveAsync(id);
                // depois de deletar, retorna pra Index
                return RedirectToAction(nameof(Index));
            }
            catch (IntegrityException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        // GET Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = await _sellerService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            return View(obj);
        }

        // GET Edit
        // o Id é obrigatório, porém utiliza-se como opcional para evitar erros de execução
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = await _sellerService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            // recebe todos os departamentos
            List<Department> departments = await _departmentService.FindAllAsync();
            // recebe o vendedor e a lista de departamentos
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments };
            // leva pra view Edit, os dados da viewModel
            return View(viewModel);
        }

        // POST Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Seller seller)
        {
            // validação a nivel de servidor, caso o usuário desative o javascript
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel);
            }

            if (id != seller.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id mismatch" });
            }

            try
            {
                await _sellerService.UpdateAsync(seller);
                return RedirectToAction(nameof(Index));
            }
            // dá pra usar o super tipo da exception para não precisar escrever 2 vezes a mesma coisa
            catch (ApplicationException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
            //catch (NotFoundException e)
            //{
            //    return RedirectToAction(nameof(Error), new { message = e.Message });
            //}
            //catch (DbConcurrencyException e)
            //{
            //    return RedirectToAction(nameof(Error), new { message = e.Message });
            //}
        }

        // GET Error
        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                // o símbolo "??" é um operador de coalescência nula, que faz com que, caso o Current seja nulo, ele coloca o "HttpContext.TraceIdentifier" no lugar
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            return View(viewModel);
        }
    }
}
