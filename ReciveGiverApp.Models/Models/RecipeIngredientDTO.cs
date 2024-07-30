using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReciveGiverApp.Models.Models
{
    public class RecipeIngredientDTO
    {
        public int RecipeID { get; set; }
        public int IngredientID { get; set; }
        public string Quantity { get; set; } = string.Empty;
    }
}
