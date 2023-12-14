using System;
using System.Collections.Generic;
using System.Linq;

namespace SalesWebMVC.Models
{
    public class Seller
    {
        // PROPRIEDADES
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public double BaseSalary { get; set; }
        // associação vendedor e departamento (o seller só pode ter 1 departamento, por isso é uma propriedade normal e não uma lista) - 1 pra 1
        public Department Department { get; set; }
        // associacao vendedor e registro de vendas (1 vendedor pode ter várias vendas) - 1 pra N
        public ICollection<SalesRecord> Sales { get; set; } = new List<SalesRecord>();

        // CONSTRUTORES
        // construtor padrão
        public Seller() { }

        // construtor sem as coleções
        public Seller(int id, string name, string email, DateTime birthDate, double baseSalary, Department department)
        {
            Id = id;
            Name = name;
            Email = email;
            BirthDate = birthDate;
            BaseSalary = baseSalary;
            Department = department;
        }

        // MÉTODOS
        // adiciona a venda ao vendedor
        public void AddSales(SalesRecord sr)
        {
            Sales.Add(sr);
        }

        // remove a venda do vendedor
        public void RemoveSales(SalesRecord sr)
        {
            Sales.Remove(sr);
        }

        // calcula a soma das vendas desse vendedor
        public double TotalSales(DateTime initial, DateTime final)
        {
            // retorna a soma das vendas que estão dentro do período das datas recebidas por parâmetro
            return Sales.Where(sr => sr.Date >= initial && sr.Date <= final).Sum(sr => sr.Amount);
        }
    }
}
