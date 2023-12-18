using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Models;
using SalesWebMVC.Services.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public async Task<List<Seller>> FindAllAsync()
        {
            // retorna do banco de dados, todos os vendedores
            return await _context.Seller.ToListAsync();
        }

        // POST
        // método que insere o vendedor no banco de dados
        public async Task InsertAsync(Seller obj)
        {
            // adiciona o vendedor ao banco de dados
            // essa operação é feita somente em memória
            _context.Add(obj);
            // salva as alterações
            // é o SaveChanges que realmente vai acessar o banco de dados
            await _context.SaveChangesAsync();
        }

        // GET by Id
        public async Task<Seller> FindByIdAsync(int id)
        {
            // além de retornar o vendedor, retorna também o departamento associado com ele
            return await _context.Seller.Include(obj => obj.Department).FirstOrDefaultAsync(obj => obj.Id == id);
        }

        // DELETE
        public async Task RemoveAsync(int id)
        {
            try
            {
                // guarda na variável obj, o vendedor encontrado pelo Id
                var obj = await _context.Seller.FindAsync(id);
                // deleta do banco de dados todas as informações desse vendedor
                _context.Seller.Remove(obj);
                // salva as alterações
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new IntegrityException("Can't delete seller because it has sales");
            }
        }

        // UPDATE
        public async Task UpdateAsync(Seller obj)
        {
            bool hasAny = await _context.Seller.AnyAsync(x => x.Id == obj.Id);
            // verifica se NÃO existe um vendedor no banco de dados com esse Id
            if (!hasAny)
            {
                throw new NotFoundException("Id not found");
            }

            try
            {
                // chama o método Update do EntityFramework pra atualizar o objeto
                _context.Update(obj);
                await _context.SaveChangesAsync();
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
