using BlazorPizzaProject.API.Data;
using BlazorPizzaProject.Business.DataModels;
using BlazorPizzaProject.Business.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorPizzaProject.API.Services
{
    public class Service : IService
    {
        private readonly AppDbContext _context;

        public Service(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Response> AddPizza(Pizza model)
        {
            if (model != null)
            {
                if (model.Id > 0)
                {
                    var result = await GetPizza(model.Id);
                    if (result != null)
                    {
                        result.Name = model.Name;
                        result.Description = model.Description;
                        result.SmallSize = model.SmallSize;
                        result.MediumSize = model.MediumSize;
                        result.LargeSize = model.LargeSize;
                        result.ExtraLargeSize = model.ExtraLargeSize;
                        result.Image = model.Image;
                        await _context.SaveChangesAsync();
                        return new Response { Success = true, Message = $"{model.Name} updated sucessfully" };

                    }
                    return new Response { Success = false, Message = $"Error Occured, {model.Name} not found" };
                }
                else
                {
                    var checkEx = await _context.Pizzas.Where(i => i.Name!.ToLower().Equals(model.Name!.ToLower()))
                        .FirstOrDefaultAsync();
                    if (checkEx == null)
                    {
                        _context.Pizzas.Add(model);
                        await _context.SaveChangesAsync();
                        return new Response();
                    }
                    return new Response { Success = false, Message = "Already added" };
                }
            }
            return new Response { Success = false, Message = "All fields required" };
        }

        public async Task<List<Pizza>> GetPizzas()
        {
            return await _context.Pizzas.ToListAsync();
        }

        public async Task<Pizza> GetPizza(int id)
        {
            var result = await _context.Pizzas.FirstOrDefaultAsync(i=> i.Id == id);
            return result;
        }

        public async Task<Response> DeletePizza(int id)
        {
            var result = await GetPizza(id);
            if (result != null)
            {
                _context.Pizzas.Remove(result);
                await _context.SaveChangesAsync();
                return new Response { Success = true, Message = $"{result.Name} deleted succesfully" };
            }
            return new Response { Success = false, Message = "Error occured! Pizza Not Found!" };
        }

      

        
    }
}
