using System;

namespace SalesWebMVC.Services.Exceptions
{
    public class DbConcurrencyException : ApplicationException
    {
        public DbConcurrencyException()
        {
        }

        public DbConcurrencyException(string message) : base(message) { }
    }
}
