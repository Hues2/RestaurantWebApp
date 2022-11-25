using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RestaurantWebApp.Data;

namespace RestaurantWebApp.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        [BindProperty]
        public Meal Meal{get; set;}
        private readonly AppDbContext _db;
        public EditModel(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Meal = await _db.Meals.FindAsync(id);
            if(Meal == null)
            {
                return RedirectToPage("/Admin/Stock"); //Maybe make error page

            }
            return Page();
        }

        public async Task<IActionResult> OnPostSaveAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            foreach (var file in Request.Form.Files)
            {
                MemoryStream ms = new MemoryStream();
                file.CopyTo(ms);
                Meal.ImageData = ms.ToArray();
                ms.Close();
                ms.Dispose();
            }
            _db.Attach(Meal).State = EntityState.Modified;
            try
            {
                await _db.SaveChangesAsync();
            } catch(DbUpdateConcurrencyException e)
            {
                throw new Exception($"Meal (Meal.ID) could not be updated", e);
            }
            return RedirectToPage("/Admin/Menu");
        }
    }
}
