using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using eshop.Persistence.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using eshop.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace eshop.Pages
{
    public class OrderModel : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; }
        private readonly ESHOPContext _context;

        public OrderModel(ESHOPContext context)
        {
            _context = context;
            Input = new InputModel();
        }

        public class InputModel
        {

            [Required]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [Display(Name = "Delivery Address")]
            public string Address { get; set; }
        }

        public void OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                Input.Email = User.Identity.Name;
            }
            var userCustomerInfo = _context.Customer.Where(c => c.Email == User.Identity.Name);

            if (userCustomerInfo.Count() >= 1)
            {
                Input.FirstName = userCustomerInfo.First().FirstName;
                Input.LastName = userCustomerInfo.First().LastName;
                Input.Address = userCustomerInfo.First().Address;
            }
        }

        public async Task<IActionResult> OnPostSubmit()
        {
            var userCustomerInfo = _context.Customer.Where(c => c.Email == User.Identity.Name);
            var basket = SessionHelper.GetObjectFromJson<OrderBasket>(HttpContext.Session, "basket");

            if (Input.Email != null && 
                Input.FirstName != null && 
                Input.LastName != null && 
                Input.Address != null && 
                basket != null)
            {
                var order = new Order();

                if(userCustomerInfo.Count() == 0)
                {
                    Customer newCustomer = new Customer();
                    newCustomer.Email = Input.Email;
                    newCustomer.FirstName = Input.FirstName;
                    newCustomer.LastName = Input.LastName;
                    newCustomer.Address = Input.Address;

                    order.Customer = newCustomer;
                }
                else
                    order.Customer = userCustomerInfo.FirstOrDefault();

                order.IsPaid = true;
                order.Status = "Shipped";

                basket.OrderId = order.Id;
                order.DeliveryAddress = Input.Address;
                order.Date = DateTime.Now;

                _context.Order.Add(order);
                await _context.SaveChangesAsync();

                return Redirect("OrderRedirect/Success?orderID=" + order.Id);
            }

            return Redirect("OrderRedirect/Fail");
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            return Page();
        }
    }
}
