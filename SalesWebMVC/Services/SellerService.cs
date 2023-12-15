using SalesWebMVC.Models;
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
            return _context.Seller.FirstOrDefault(obj => obj.Id == id);
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
    }
}
