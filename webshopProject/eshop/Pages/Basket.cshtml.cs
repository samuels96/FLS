using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using eshop.Persistence.Models;
using eshop.Helpers;
using Microsoft.AspNetCore.Identity;
using eshop.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;

namespace eshop.Pages
{
    public class BasketModel : PageModel
    {
        private readonly eshop.Persistence.Models.ESHOPContext _context;
        public OrderBasket Basket;
        public Decimal Total { get; set; }

        public BasketModel(eshop.Persistence.Models.ESHOPContext context)
        {
            _context = context;
            Basket = new OrderBasket();
        }


        public void OnGet()
        {
            Basket = SessionHelper.GetObjectFromJson<OrderBasket>(HttpContext.Session, "basket") ?? new OrderBasket();
            Total = Basket.BasketProduct.Select(bp => bp.Product.Price * bp.Quantity).Sum();
        }

        private void addProductToBasket(int productID)
        {
            var product = _context.Product.Find(productID);

            Basket = SessionHelper.GetObjectFromJson<OrderBasket>(HttpContext.Session, "basket") ?? new OrderBasket();

            BasketProduct basketProduct = new BasketProduct
            {
                Product = product
            };

            if (Basket.BasketProduct.Where(bp => bp.Product.Id == product.Id).Count() >= 1)
            {
                Basket.BasketProduct.Where(bp => bp.Product.Id == product.Id).First().Quantity += 1;
            }
            else
            {
                basketProduct.Quantity = 1;
                Basket.BasketProduct.Add(basketProduct);
            }
            SessionHelper.SetObjectAsJson(HttpContext.Session, "basket", Basket);

        }
        public IActionResult OnPostAddToBasket(int productID, int quantity = 1)
        {
            addProductToBasket(productID);
            return RedirectToPage("Basket");
        }

        public IActionResult OnGetOrder()
        {
            Basket = SessionHelper.GetObjectFromJson<OrderBasket>(HttpContext.Session, "basket") ?? new OrderBasket();
            if(Basket.BasketProduct.Count() <= 0)
            {
                return Page();
            }
            return RedirectToPage("Order");
        }

        public IActionResult OnGetDelete(int productID)
        {
            Basket = SessionHelper.GetObjectFromJson<OrderBasket>(HttpContext.Session, "basket") ?? new OrderBasket();

            Basket.BasketProduct = Basket.BasketProduct.Where(bp => bp.Product.Id != productID).ToList();

            SessionHelper.SetObjectAsJson(HttpContext.Session, "basket", Basket);
            return RedirectToPage("Basket");
        }

        public IActionResult OnPostUpdate(int[] quantities)
        {
            Basket = SessionHelper.GetObjectFromJson<OrderBasket>(HttpContext.Session, "basket") ?? new OrderBasket();

            var basketEnum = Basket.BasketProduct.GetEnumerator();

            for (var i = 0; i < Basket.BasketProduct.Count(); i++)
            {
                basketEnum.MoveNext();
                basketEnum.Current.Quantity = quantities[i];
            }

            SessionHelper.SetObjectAsJson(HttpContext.Session, "basket", Basket);
            return RedirectToPage("Basket");
        }


    }
}
