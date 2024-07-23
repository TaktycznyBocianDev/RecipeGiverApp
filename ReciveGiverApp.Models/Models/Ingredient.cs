using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReciveGiverApp.Models.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string IngredientName { get; set; } = string.Empty;
        public string Quantity { get; set; } = string.Empty;

    }
}
