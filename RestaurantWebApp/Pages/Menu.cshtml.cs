using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RestaurantWebApp.Data;

namespace RestaurantWebApp.Pages
{
    public class MenuModel : PageModel
    {
        private AppDbContext _db;
        public IList<Meal> Meal { get; private set; }
        [BindProperty]
        public string Search { get; set; }
        
        private readonly UserManager<ApplicationUser> _userManager;

        public MenuModel(AppDbContext db, UserManager<ApplicationUser> um)
        {
            _db = db;
            _userManager = um;
        }
        public void OnGet()
        {
            Meal = _db.Meals.FromSqlRaw("SELECT * FROM Meals WHERE Active = 1").ToList();
        }


        public IActionResult OnPostSearch()
        {
            if(Search == null)
            {
                Search = "";
            }
            Search = Search.Replace("\'", ""); // If users type a single quote mark, it will essentially get deleted
            Meal = _db.Meals.FromSqlRaw("SELECT * FROM Meals WHERE Name LIKE '%" + Search + "%' AND Active = 1").ToList();
            return Page();
        }        
        
        public async Task<IActionResult> OnPostAddAsync(int itemID)
        {
            var user = await _userManager.GetUserAsync(User);
            CheckoutCustomer customer = await _db.CheckoutCustomers.FindAsync(user.Email);

            var item = _db.BasketItems.FromSqlRaw("SELECT * FROM BasketItems WHERE MealID = {0} AND BasketID = {1}", itemID, customer.BasketID).ToList().FirstOrDefault();
            if (item == null)
            {
                BasketItem newItem = new BasketItem
                {
                    BasketID = customer.BasketID,
                    MealID = itemID,
                    Quantity = 1
                };
                _db.BasketItems.Add(newItem);
               
                await _db.SaveChangesAsync();
            }
            else
            {
                item.Quantity += 1;
                _db.Attach(item).State = EntityState.Modified;
                try
                {
                    await _db.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException e)
                {
                    throw new Exception($"Unable to add item to basket", e);
                }
            }

            return RedirectToPage();
        }
    }
}
