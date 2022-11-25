using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RestaurantWebApp.Data;

namespace RestaurantWebApp.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private AppDbContext _db;
        [BindProperty]
        public Meal aMeal { get; set; }
        public CreateModel(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid) { return Page(); }
            aMeal.Active = true;
            foreach(var file in Request.Form.Files)
            {
                MemoryStream ms = new MemoryStream();
                file.CopyTo(ms);
                aMeal.ImageData = ms.ToArray();
                ms.Close();
                ms.Dispose();
            }
            
            _db.Meals.Add(aMeal);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Admin/Create");  
        }
    }
}
