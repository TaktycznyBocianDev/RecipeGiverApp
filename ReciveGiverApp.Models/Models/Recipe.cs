using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReciveGiverApp.Models.Models
{
    public class Recipe
    {
        public int RecipeId { get; set; }
        public string RecipeName { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string Instructions { get; set; } = string.Empty;
        public string FitPorada { get; set; } = string.Empty;
        public int Kilocalories { get; set; }
        public List<Ingredient> ingredients { get; set; } = new List<Ingredient> { };

    }
}
