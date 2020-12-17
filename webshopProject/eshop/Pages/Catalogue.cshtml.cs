using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using eshop.Persistence.Models;
using JW;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using eshop.Helpers;

namespace eshop.Pages.Catalogue
{
    public class Products : PageModel
    {
        private readonly eshop.Persistence.Models.ESHOPContext _context;

        public Pager Pager { get; set; }

        public SelectList PageSizeList { get; set; }
        public List<string> CategoryNameList { get; set; }
        public SelectList BrandNameList { get; set; }
        public int PageSize { get; set; }
        public string SelectedCategoryName { get; set; }
        public string BrandName { get; set; }
        public IEnumerable<CatalogueProductView> ProductsCatalogue { get;set; }

        public Products(eshop.Persistence.Models.ESHOPContext context)
        {
            _context = context;

            PageSizeList = new SelectList(new[] {10, 20, 50, 100, 200, 500, 1000 });
            PageSize = 10;

            List<string> tmpCategoryNameList = new List<string>() { "Any" };
            tmpCategoryNameList.AddRange(_context.Category.Select(c => c.Name).ToList());

            CategoryNameList = tmpCategoryNameList;

            BrandNameList = new SelectList(new[] { "" });

            ProductsCatalogue = new List<CatalogueProductView>();

            Pager = new Pager(0);
        }
        private void refreshSelectList()
        {
            List<String> tmpBrandNameList = new List<string>() { "Any" };

            if (SelectedCategoryName == "Any")
                tmpBrandNameList.AddRange(_context.Brand.Select(b => b.Name).ToList());
            else
                tmpBrandNameList.AddRange(_context.CatalogueProductView
                    .Where(cpv => cpv.Category == SelectedCategoryName)
                    .Select(cpv => cpv.BrandName)
                    .ToList());

            BrandNameList = new SelectList(tmpBrandNameList);
        }


        public async Task<IActionResult> OnGetAsync(int p = 1)
        {
            BrandName = SessionHelper.GetObjectFromJson<String>(HttpContext.Session, "BrandName") ?? "Any";
            SelectedCategoryName = SessionHelper.GetObjectFromJson<String>(HttpContext.Session, "SelectedCategoryName") ?? "Any";

            refreshSelectList();

            if (SelectedCategoryName != "Any")
            {
		string sqlBrand = "CALL get_products_by_brand_name({0});";
		string sqlCat = "CALL get_products_by_category({0});";
				
                if (BrandName != "Any")
                {
                    ProductsCatalogue = await _context.CatalogueProductView.FromSqlRaw(sqlBrand, BrandName)
                        .ToListAsync();
                    ProductsCatalogue = ProductsCatalogue.Where(cp => cp.Category == SelectedCategoryName);
                }
                else
                {
                    ProductsCatalogue = await _context.CatalogueProductView.FromSqlRaw(sqlCat, SelectedCategoryName)
                        .ToListAsync();
                }
            }
            else {
                if (BrandName != "Any")
                {
                    ProductsCatalogue = await _context.CatalogueProductView.FromSqlRaw(sqlBrand, BrandName)
                        .ToListAsync();
                }
                else
                {
                    ProductsCatalogue = await _context.CatalogueProductView
                        .ToListAsync();
                }
            }

            var tmpPageSize = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "PageSize");
            if (tmpPageSize > 0)
                PageSize = tmpPageSize;

            if (ProductsCatalogue.Count() > 0)
            {
                Pager = new Pager(ProductsCatalogue.Count(), p, PageSize);
                if (ProductsCatalogue.Count() >= Pager.PageSize)
                {
                    ProductsCatalogue = ProductsCatalogue.Skip((Pager.CurrentPage - 1) * Pager.PageSize).Take(Pager.PageSize);
                }
            }
            else
                Pager = new Pager(0);

            return Page();
        }

        public void OnPostSelectedCategoryNameAsync(string categoryName)
        {
            SessionHelper.SetObjectAsJson(HttpContext.Session, "SelectedCategoryName", categoryName);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "BrandName", "Any");
        }

        public async Task<IActionResult> OnPostAsync(int pageSize, string brandName)
        {
            SessionHelper.SetObjectAsJson(HttpContext.Session, "PageSize", pageSize);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "BrandName", brandName);
            return (await OnGetAsync());
        }
    }
} 
