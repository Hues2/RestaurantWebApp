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
using Stripe;

namespace RestaurantWebApp.Pages
{
    [Authorize]
    public class CheckoutModel : PageModel
    {
        private AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public IList<CheckoutItem> Items { get; set; }
        public OrderHistory Order = new OrderHistory();

        public String myString = "-";
        public decimal Total = 0;
        public long AmountPayable = 0;

        public CheckoutModel(AppDbContext db, UserManager<ApplicationUser> um)
        {
            _db = db;
            _userManager = um;
        }
        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            CheckoutCustomer customer = await _db.CheckoutCustomers.FindAsync(user.Email);
            Items = _db.CheckoutItems.FromSqlRaw(
                "SELECT Meals.ID, Meals.Price, Meals.Name, BasketItems.BasketID, BasketItems.Quantity "+
                "FROM Meals INNER JOIN BasketItems ON Meals.ID = BasketItems.MealID "+
                "WHERE BasketID = {0}", customer.BasketID).ToList();
            Total = 0;
            foreach(var item in Items)
            {
                Total += (item.Quantity) * (item.Price);
                AmountPayable = (long)(Total * 100);
            }
        }

        public async Task Process()
        {
            var currentOrder = _db.OrderHistories.FromSqlRaw("SELECT * FROM OrderHistories").OrderByDescending(b => b.OrderNo).FirstOrDefault();
            if(currentOrder == null)
            {
                Order.OrderNo = 1;
            }
            else
            {
                Order.OrderNo = currentOrder.OrderNo + 1;
            }
            var user = await _userManager.GetUserAsync(User);
            Order.Email = user.Email;
            _db.OrderHistories.Add(Order);

            CheckoutCustomer customer = await _db.CheckoutCustomers.FindAsync(user.Email);

            var basketItems = _db.BasketItems.FromSqlRaw("SELECT * FROM BasketItems WHERE BasketID = {0}", customer.BasketID).ToList();
            foreach(var item in basketItems)
            {
                Data.OrderItem oi = new Data.OrderItem
                {
                    OrderNo = Order.OrderNo,
                    StockID = item.MealID,
                    Quantity = item.Quantity
                };
                _db.OrderItems.Add(oi);
                _db.BasketItems.Remove(item);
            }
            await _db.SaveChangesAsync();
        }


        public async Task<IActionResult> OnPostAddRemoveAsync(int itemID, string value)
        {
            if (value == "+1")
            {
                var user = await _userManager.GetUserAsync(User);
                CheckoutCustomer customer = await _db.CheckoutCustomers.FindAsync(user.Email);

                var item = _db.BasketItems.FromSqlRaw("SELECT * FROM BasketItems WHERE MealID = {0} AND BasketID = {1}", itemID, customer.BasketID).ToList().FirstOrDefault();
                
                item.Quantity += 1;
                _db.Attach(item).State = EntityState.Modified;
                try{
                    await _db.SaveChangesAsync();
                }catch (DbUpdateConcurrencyException e){
                    throw new Exception($"Unable to add item", e);
                }
            }else if (value == "-1")
            {
                var user = await _userManager.GetUserAsync(User);
                CheckoutCustomer customer = await _db.CheckoutCustomers.FindAsync(user.Email);

                var item = _db.BasketItems.FromSqlRaw("SELECT * FROM BasketItems WHERE MealID = {0} AND BasketID = {1}", itemID, customer.BasketID).ToList().FirstOrDefault();

                item.Quantity -= 1;
                _db.Attach(item).State = EntityState.Modified;
                if (item.Quantity == 0)
                {
                    _db.BasketItems.Remove(item);
                }
                try
                {
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    throw new Exception($"Unable to remove item", e);
                }
            }
            return RedirectToPage();
        }

        public IActionResult OnPostCharge(string stripeEmail, string stripeToken, long amount)
        {
            var customers = new CustomerService();
            var charges = new ChargeService();

            var customer = customers.Create(new CustomerCreateOptions { 
            
            Email = stripeEmail,
            Source = stripeToken
            });

            var charge = charges.Create(new ChargeCreateOptions
            {
                Amount = amount,
                Description = "CO5227 Meals Charge",
                Currency = "gbp",
                Customer = customer.Id
            });
            Process().Wait();
            return RedirectToPage("/Index");
        }

    }
}
