using BlazorPizzaProject.Business.DataModels;
using BlazorPizzaProject.Business.Models;

namespace BlazorPizzaProject.API.Services
{
    public interface IService
    {
        Task<Response> AddPizza(Pizza model);
        Task<List<Pizza>> GetPizzas();
        Task<Pizza> GetPizza(int id);
        Task<Response> DeletePizza(int id);
    }
}
