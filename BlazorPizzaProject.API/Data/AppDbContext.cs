using BlazorPizzaProject.Business.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorPizzaProject.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Pizza> Pizzas { get; set; }
    }
}
