using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorPizzaProject.Business.DataModels
{
    public class MyOrdersModel
    {
        public int PizzaId { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public string? SizeName { get; set; }
        public double Price { get; set; }
    }
}
