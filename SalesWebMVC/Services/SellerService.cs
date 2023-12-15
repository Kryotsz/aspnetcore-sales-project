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

        public List<Seller> FindAll()
        {
            // retorna do banco de dados, todos os vendedores
            return _context.Seller.ToList();
        }

        // método que insere o vendedor no banco de dados
        public void Insert(Seller obj)
        {
            // adiciona o vendedor ao banco de dados
            _context.Add(obj);
            // salva as alterações
            _context.SaveChanges();
        }
    }
}
