using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using eshop.Persistence.Models;

namespace eshop.Pages.OrderRedirect
{
    public class SuccessModel : PageModel
    {
        private readonly eshop.Persistence.Models.ESHOPContext _context;
        public Order order;

        public SuccessModel(eshop.Persistence.Models.ESHOPContext context)
        {
            _context = context;
        }


        public IActionResult OnGet(int orderID)
        {
            if (HttpContext.Connection.RemoteIpAddress != HttpContext.Connection.LocalIpAddress)
            {
                Redirect("Index");
            }

            order = new Order();
            order = _context.Order.Where(o => o.Id == orderID).FirstOrDefault();
            System.Diagnostics.Debug.WriteLine(order.Id);

            return Page();
        }
    }
}
