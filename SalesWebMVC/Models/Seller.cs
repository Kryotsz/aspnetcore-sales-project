using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SalesWebMVC.Models
{
    public class Seller
    {
        // PROPRIEDADES
        public int Id { get; set; }
        public string Name { get; set; }

        // transforma o texto do email pra link de email
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        // utiliza a annotation Display pra formatar o título que vai aparecer na coluna
        [Display(Name = "Birth Date")]
        // formata a data
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Base Salary")]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public double BaseSalary { get; set; }
        // associação vendedor e departamento (o seller só pode ter 1 departamento, por isso é uma propriedade normal e não uma lista) - 1 pra 1
        public Department Department { get; set; }
        // vai guardar o Id do departamento, que não pode ser nulo
        public int DepartmentId { get; set; }
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
