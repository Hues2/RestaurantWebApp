using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RestaurantWebApp.Data;


namespace RestaurantWebApp.Pages
{
    [Authorize]
    public class ContactModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private AppDbContext _db;
        [BindProperty]
        public Comment aComment { get; set; }
        public ContactModel(AppDbContext db, UserManager<ApplicationUser> um)
        {
            _db = db;
            _userManager = um;
        }


        public async Task<IActionResult> OnPostSendAsync()
        {
            
            var user = await _userManager.GetUserAsync(User);
            CheckoutCustomer customer = await _db.CheckoutCustomers.FindAsync(user.Email);
            aComment.Email = customer.Email;
            _db.Comments.Add(aComment);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Contact");
        }
    }
}
