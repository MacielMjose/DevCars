using DevCars.API.Entities;
using DevCars.API.InputModels;
using DevCars.API.Persistence;
using DevCars.API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevCars.API.Controllers
{
    [Route("api/customers")]
    public class CustumerController : ControllerBase
    {
        private readonly DevCarsDbContext _dbContext;

        public CustumerController(DevCarsDbContext devCarsDbContext)
        {
            this._dbContext = devCarsDbContext;
        }
        //POST api/customers
        [HttpPost]
        public IActionResult Post([FromBody] AddCustomerInputModel model)
        {
            var customer = new Customer(model.FullName, model.Document, model.BirthDate);
            _dbContext.Customers.Add(customer);
            _dbContext.SaveChanges();

            return Ok(); //trocar para noContent
        }

        //POST api/customers/2/orders
        [HttpPost("{id}/orders")]
        public IActionResult PostOrder(int id, [FromBody] AddOrderInputModel model)
        {
            var extraItems = model.ExtraItems
                .Select(e => new ExtraOrderItem(e.Description, e.Price))
                .ToList();

            var car = _dbContext.Cars.SingleOrDefault(c => c.Id == model.IdCar);
            var order = new Order(model.IdCar, model.IdCustomer, car.Price, extraItems);

            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();

            return CreatedAtAction(
                nameof(PostOrder),
                new { id = order.IdCustomer, orderId = order.Id },
                model
                );
        }

        //GET All Customers
        [HttpGet("")]
        public IActionResult GetCustomers()
        {
            var customers = _dbContext.Customers
                .Select(c => new CustomersViewModel(c.Id , c.FullName))
                .ToList();

            return Ok(customers);
        }

        //GET api/customers/1/orders/3
        [HttpGet("{id}/orders/{orderid}")]
        public IActionResult GetOrder(int id, int orderid)
        {
            var order = _dbContext.Orders
                .Include(o => o.ExtraItems)
                .SingleOrDefault(c => c.Id == orderid);

            if(order == null)
            {
                return NotFound();
            }

            var extraItems = order
                .ExtraItems
                .Select(e => e.Description)
                .ToList();

            var orderViewModel = new OrderDetailsViewModel(order.IdCar, order.IdCustomer,order.TotalCost,extraItems);

            return Ok();
        }
    }
}
