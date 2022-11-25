using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantWebApp.Data
{
    public class BasketItem
    {
        [Required]
        public int MealID { get; set; }
        [Required]
        public int BasketID { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
