using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantWebApp.Data
{
    public class Comment
    {
        [Key]
        public int CommentID { get; set; }
        [StringLength(50), EmailAddress]
        public string Email { get; set; }
        [StringLength(500)]
        public string UserComment { get; set; }
        public int Rating { get; set; }
    }
}
