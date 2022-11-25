using System;
using System.Collections.Generic;
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
    public class MenuModel : PageModel
    {

        private AppDbContext _db;
        public IList<Meal> Meal { get; private set; }
        [BindProperty]
        public string Search { get; set; }
        public MenuModel(AppDbContext db)
        {
            _db = db;
        }
        public void OnGet()
        {
            Meal = _db.Meals.FromSqlRaw("SELECT * FROM Meals ORDER BY Active DESC").ToList();
        }

        public IActionResult OnPostSearch()
        {
            Search = Search.Replace("\'", ""); // If users type a single quote mark, it will essentially get deleted
            Meal = _db.Meals.FromSqlRaw("SELECT * FROM Meals WHERE Name LIKE '%" + Search + "%' ORDER BY Active DESC").ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostActivateAsync(int mealID)
        {
            var meal = await _db.Meals.FindAsync(mealID);
            if (meal.Active == true) {
                meal.Active = false;
            } else if(meal.Active == false)
            {
                meal.Active = true;
            }
            _db.Attach(meal).State = EntityState.Modified;
            try
            {
                await _db.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException e)
            {
                throw new Exception($"Meal (Meal.ID) could not be updated", e);
            } 
            return RedirectToPage();
        }
    }
}
