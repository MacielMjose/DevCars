using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevCars.API.Entities
{
    public class Customer
    {
        #region EF CTOR
        protected Customer(){}
        #endregion

        public Customer(string fullName, string document, DateTime birthDate)
        {
            FullName = fullName;
            Document = document;
            BirthDate = birthDate;
            Orders = new List<Order>();
        }

        public int Id { get; set; }
        public string FullName { get; private set; }
        public string Document { get; private set; }
        public DateTime BirthDate { get; private set; }
        public List<Order> Orders { get; private set; }

        internal void AddOrder(Order order)
        {
            Orders.Add(order);
        }
    }
}
