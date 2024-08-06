using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReciveGiverApp.Models.Models
{
    public class DailyMeals
    {
        public int DailyMealID { get; set; }
        public DateTime DayDate { get; set; }
        public int CategoryID { get; set; }
        public int RecipeID { get; set; }
    }
}
