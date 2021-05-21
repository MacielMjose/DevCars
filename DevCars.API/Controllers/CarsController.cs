using DevCars.API.Entities;
using DevCars.API.InputModels;
using DevCars.API.Persistence;
using DevCars.API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevCars.API.Controllers
{
    [Route("api/cars")] //[Route("api/[controller]")] 
    public class CarsController : ControllerBase
    {
        private readonly DevCarsDbContext _dbContext;

        public CarsController(DevCarsDbContext devCarsDbContext)
        {
            this._dbContext = devCarsDbContext;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var cars = _dbContext.Cars;

            var carsViewModel = cars
                .Select(c => new CarItemViewModel(c.Id, c.Brand, c.Model, c.Price))
                .ToList();
            //retorna lista de todos os carros disponiveis CarItemViewModel
            return Ok(carsViewModel);
        }

        //api/cars/1
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            //SE O ID CARRO NAO EXISTIR, RETORNA NOT FOUND,
            //ELSE OK

            var car = _dbContext.Cars.SingleOrDefault(c => c.Id == id);

            if(car == null)
            {
                return NotFound();
            }
            //fazer um controler com o carro detalhado e o carro simplificado
            var carDetailViewModel = new CarDetailsViewModel(
                car.Id,
                car.Brand,
                car.Model,
                car.VinCode,
                car.Year,
                car.Price,
                car.Color,
                car.ProductionDate
                );
            
            return Ok(carDetailViewModel);
        }        

        //POST api/car
        //s
        [HttpPost()]
        public IActionResult Post([FromBody] AddCarInputModel model)
        {
            //SE CADASTRO FUNCIONAR, RETORNA CREATED 201
            //SE OS DADOS DE ENTRADA ESTIVEREM INCORRETOS, RETORNA BAD REQUEST 400
            //SE O CADASTRO FUNCIONAR, MAS NAO TIVER UMA API DE CONSULTA, RETORNA 204 NO-CONTENT
            if(model.Model.Length > 50)
            {
                return BadRequest("Modelo não pode ter mais de 50 caracteres");
            }

            var addCar = new Car(
                model.VinCode,
                model.Brand,
                model.Model,
                model.Ano,
                model.Price,
                model.Color,
                model.ProductionDate
                );

            _dbContext.Cars.Add(addCar);
            _dbContext.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = addCar.Id }, model);
        }

        //PUT api/cars/1
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateCarInputModel model)
        {
            //SE ATUALIZACAO FUNCIONAR, RETORNA 204
            //SE OBJETO TIVER INCORRETO, RETORNA 400 BAD REQUEST
            //SE NAO EXISTIR RETORNA NOT FOUND 404
            var car = _dbContext.Cars.SingleOrDefault(c => c.Id == id);

            if(car == null)
            {
                return NotFound();
            }

            car.UpdateCar(model.Color, model.Price);
            _dbContext.SaveChanges();
            
            return NoContent();
        }

        //DELETE api/cars/2
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            //SE NAO EXISTIR RETORNA 404
            //SE FOR COM SUCESSO, RETORNA NO CONTENT (204)
            var car = _dbContext.Cars.SingleOrDefault(c => c.Id == id);
            if(car == null)
            {
                return NotFound();
            }

            car.SetAsSuspended();
            //_dbContext.Cars.Remove(car);
            _dbContext.SaveChanges();

            return NoContent();
        }
    }
}
