using BlazorPizzaProject.API.Services;
using BlazorPizzaProject.Business.DataModels;
using BlazorPizzaProject.Business.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace BlazorPizzaProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PizzaController : ControllerBase
    {
        private readonly IService _service;

        public PizzaController(IService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<Response>> AddPizza(Pizza model)
        {
            if (model != null)
            {
                return Ok(await _service.AddPizza(model));
                
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<ActionResult<List<Pizza>>> GetPizzas()
        {
            var pizzas = await _service.GetPizzas();
            if (pizzas != null)
                return Ok(pizzas);
            return BadRequest(new Response {Success =false,Message = "No data found"});
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Pizza>> GetPizza(int id)
        {
            if (id > 0)
            {
                return Ok(await _service.GetPizza(id));
            }
            return BadRequest("Sorry error occured");
        }
        


    }
}
