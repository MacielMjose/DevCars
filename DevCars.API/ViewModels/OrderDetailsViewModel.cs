using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevCars.API.ViewModels
{
    public class OrderDetailsViewModel
    {
        public OrderDetailsViewModel(int idCostumer, int idCar, decimal totalCost, List<string> extraItems)
        {
            IdCostumer = idCostumer;
            IdCar = idCar;
            TotalCost = totalCost;
            ExtraItems = extraItems;
        }

        public int IdCostumer { get; set; }
        public int IdCar { get; set; }
        public decimal TotalCost { get; set; }
        public List<string> ExtraItems { get; set; }
    }
}
