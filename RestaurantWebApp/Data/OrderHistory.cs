using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantWebApp.Data
{
    public class OrderHistory
    {
        [Key,Required]
        public int OrderNo {get; set;}
        [Required, StringLength(50)]
        public string Email { get; set; }
    }
}
