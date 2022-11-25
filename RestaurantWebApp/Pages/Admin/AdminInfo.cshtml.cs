using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RestaurantWebApp.Data;

namespace RestaurantWebApp.Pages.Admin
{
    public class AdminInfoModel : PageModel
    {
        public string data = "";
        private AppDbContext _db;
        public IList<CheckoutCustomer> CustomerList { get; private set; }
        public IList<OrderHistory> OrderHistories { get; private set; }
        public IList<OrderItem> OrderItems { get; private set; }
        public IList<Meal> Meals { get; private set; }
        public IList<Comment> Comments { get; private set; }
        [BindProperty]
        public string Search { get; set; }

        private readonly UserManager<ApplicationUser> _userManager;

        public AdminInfoModel(AppDbContext db, UserManager<ApplicationUser> um)
        {
            _db = db;
            _userManager = um;
        }
        public void OnGet()
        {
            CustomerList = _db.CheckoutCustomers.FromSqlRaw("SELECT * FROM CheckoutCustomers").ToList();
            OrderHistories = _db.OrderHistories.FromSqlRaw("SELECT * FROM OrderHistories").ToList();
            OrderItems = _db.OrderItems.FromSqlRaw("SELECT * FROM OrderItems").ToList();
            Meals = _db.Meals.FromSqlRaw("SELECT * FROM Meals").ToList();
            Comments = _db.Comments.FromSqlRaw("SELECT * FROM Comments").ToList();
        }
        
        public async Task<IActionResult> OnPostDeleteUserAsync(string customerEmail)
        {
            IList<OrderHistory> orderNumber = _db.OrderHistories.FromSqlRaw("SELECT * FROM OrderHistories WHERE Email = {0}", customerEmail).ToList();
            
            if (orderNumber.Count == 0)
            {
                var user = _userManager.FindByEmailAsync(customerEmail);
                CheckoutCustomer customer = await _db.CheckoutCustomers.FindAsync(customerEmail);
                _db.CheckoutCustomers.Remove(customer);

                // MAYBE ADD IF STATEMENT, IF CUSTOMER DOESNT HAVE ORDERS CAN DELETE, IF NOT CANT DELETE BECAUSE WE WANT TO KEEP THE ORDERS IN THE INFO PAGE

                var userToBeDeleted = await _userManager.FindByEmailAsync(customerEmail);
                if (userToBeDeleted == null)
                {
                    throw new Exception($"Unable to delete user");
                }
                await _userManager.DeleteAsync(userToBeDeleted);

                try
                {
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    throw new Exception($"Unable to delete user", e);
                }
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteCommentAsync(int commentID)
        {
            var comment = _db.Comments.FromSqlRaw("SELECT * FROM Comments WHERE CommentID = {0}", commentID).ToList().FirstOrDefault();
            if (comment == null)
            {
                throw new Exception($"Unable to delete user");
            }
            _db.Attach(comment).State = EntityState.Modified;
            _db.Comments.Remove(comment);
            
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new Exception($"Unable to delete user", e);
            }
            return RedirectToPage();

        }


    }
}
