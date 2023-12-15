using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Models;
using SalesWebMVC.Services.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace SalesWebMVC.Services
{
    public class SellerService
    {
        private readonly SalesWebMVCContext _context;

        public SellerService(SalesWebMVCContext context)
        {
            _context = context;
        }

        // GET
        public List<Seller> FindAll()
        {
            // retorna do banco de dados, todos os vendedores
            return _context.Seller.ToList();
        }

        // POST
        // método que insere o vendedor no banco de dados
        public void Insert(Seller obj)
        {
            // adiciona o vendedor ao banco de dados
            _context.Add(obj);
            // salva as alterações
            _context.SaveChanges();
        }

        // GET by Id
        public Seller FindById(int id)
        {
            // além de retornar o vendedor, retorna também o departamento associado com ele
            return _context.Seller.Include(obj => obj.Department).FirstOrDefault(obj => obj.Id == id);
        }

        // DELETE
        public void Remove(int id)
        {
            // guarda na variável obj, o vendedor encontrado pelo Id
            var obj = _context.Seller.Find(id);
            // deleta do banco de dados todas as informações desse vendedor
            _context.Seller.Remove(obj);
            // salva as alterações
            _context.SaveChanges();
        }

        // UPDATE
        public void Update(Seller obj)
        {
            // verifica se NÃO existe um vendedor no banco de dados com esse Id
            if (!_context.Seller.Any(x => x.Id == obj.Id))
            {
                throw new NotFoundException("Id not found");
            }

            try
            {
                // chama o método Update do EntityFramework pra atualizar o objeto
                _context.Update(obj);
                _context.SaveChanges();
            }
            // intercepta uma exceção de nível de acesso a dados
            catch (DbUpdateConcurrencyException e)
            {
                // relança a exceção como nível de serviço, para que a controller só precise lidar com exceções da camada de serviço
                throw new DbConcurrencyException(e.Message);
            }
        }
    }
}
