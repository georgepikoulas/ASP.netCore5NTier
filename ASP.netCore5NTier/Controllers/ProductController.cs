using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP.netCore5NTier.Data;
using ASP.netCore5NTier.Models;
using ASP.netCore5NTier.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ASP.netCore5NTier.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDBContext _db;


        public ProductController(ApplicationDBContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> products = _db.Product;
            foreach (var item in products)
            {
                item.Category = _db.Category.FirstOrDefault(p => p.Id == item.Id);
            }

            return View(products);
        }

        //Get Upsert 
        public IActionResult Upsert(int? id)
        {
            //IEnumerable<SelectListItem> CategoryDropDown = _db.Category.Select(p => new SelectListItem
            //{

            //    Text = p.Name,
            //    Value = p.Id.ToString()
            //});

            //ViewBag.CategoryDropDown = CategoryDropDown;

            //var product = new Product();
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategoryListItems = _db.Category.Select(p => new SelectListItem
                {

                    Text = p.Name,
                    Value = p.Id.ToString()
                })
            };

            if (id == null)
            {
                //this is for create
                return View(productVM);
            }
            else
            {
                productVM.Product = _db.Product.FirstOrDefault(p => p.Id == productVM.Product.Id);
                if (productVM == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(productVM);
                }
            }

        }

        //POST upsert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category category)
        {
            if (ModelState.IsValid)
            {
                _db.Category.Add(category);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }


    }
}
