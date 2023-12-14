using SalesWebMVC.Models.Enums;
using System;

namespace SalesWebMVC.Models
{
    public class SalesRecord
    {
        // PROPRIEDADES
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public SaleStatus Status { get; set; }
        // associação venda e vendedor (a venda só pode ter 1 vendedor) - 1 pra 1
        public Seller Seller { get; set; }

        // CONSTRUTORES
        // construtor padrão
        public SalesRecord() { }

        // construtor com atributos
        public SalesRecord(int id, DateTime date, double amount, SaleStatus status, Seller seller)
        {
            Id = id;
            Date = date;
            Amount = amount;
            Status = status;
            Seller = seller;
        }
    }
}
