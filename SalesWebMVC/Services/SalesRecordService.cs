using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMVC.Services
{
    public class SalesRecordService
    {
        private readonly SalesWebMVCContext _context;

        public SalesRecordService(SalesWebMVCContext context)
        {
            _context = context;
        }

        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            // objeto IQueryable para poder adicionar diversos filtros com LINQ
            var result = from obj in _context.SalesRecord select obj;
            // se a data mínima foi informada
            if (minDate.HasValue)
            {
                // a data tem que ser maior que a data mínima
                result = result.Where(x => x.Date >= minDate.Value);
            }

            if (maxDate.HasValue)
            {
                // a data tem que ser menor que a data máxima
                result = result.Where(x => x.Date <= maxDate.Value);
            }

            return await result
                .Include(x => x.Seller) // join com a tabela de Sellers
                .Include(x => x.Seller.Department) // join com a tabela de Departments
                .OrderByDescending(x => x.Date) // ordena por data de forma decrescente
                .ToListAsync();
        }
    }
}
