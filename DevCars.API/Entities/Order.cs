﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevCars.API.Entities
{
    public class Order
    {
        #region EF CTOR
        protected Order(){}
        #endregion

        public Order(int idCar, int idCustomer,decimal price, List<ExtraOrderItem> items)
        {
            IdCar = idCar;
            IdCustomer = idCustomer;
            TotalCost = items.Sum(i => i.Price) + price;
            ExtraItems = items;
        }

        public int Id { get; private set; }
        public int IdCar { get; private set; }
        public Car Car { get; set; }
        public int IdCustomer { get; private set; }
        public Customer Customer { get; set; }
        public decimal TotalCost { get; private set; }
        public List<ExtraOrderItem> ExtraItems { get; private set; }
    }

    public class ExtraOrderItem
    {
        #region EF CTOR
        public ExtraOrderItem() {}
        #endregion

        public ExtraOrderItem(string description, decimal price)
        {
            Description = description;
            Price = price;
        }

        public int Id { get; private set; }
        public string Description { get; set; }
        public decimal Price { get; private set; }
        public int IdOrder { get; private set; }
    }
}