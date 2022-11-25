using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantWebApp.Data
{
    public class Meal
    {
        [Key]
        public int ID { get; set; }
        [StringLength(25)][Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [StringLength(250)][Required]
        public string Description { get; set; }
        [Required]
        public Boolean Active { get; set; }
        [StringLength(7)][Required]
        public string Type { get; set; }
        [StringLength(250)]
        public string ImageDescription { get; set; }
        public byte[] ImageData { get; set; }
    }
}
