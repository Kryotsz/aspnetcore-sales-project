using System;
using System.Collections.Generic;
using System.Linq;

namespace SalesWebMVC.Models
{
    public class Department
    {
        // PROPRIEDADES
        public int Id { get; set; }
        public string Name { get; set; }
        // associação departamento e vendedor (o departamento possui vários sellers, por isso é uma lista/coleção/conjunto) - 1 pra N
        public ICollection<Seller> Sellers { get; set; } = new List<Seller>();

        // CONSTRUTORES
        // construtor padrão
        public Department() { }

        // construtor sem as coleções
        public Department(int id, string name)
        {
            Id = id;
            Name = name;
        }

        // MÉTODOS
        // adiciona o vendedor na lista de vendedores do departamento
        public void AddSeller(Seller seller)
        {
            Sellers.Add(seller);
        }

        // calcula a soma das vendas desse departamento
        public double TotalSales(DateTime initial, DateTime final)
        {
            // retorna a soma das vendas de todos os vendedores desse departamento dentro desse intervalo de datas
            return Sellers.Sum(seller => seller.TotalSales(initial, final));
        }
    }
}
