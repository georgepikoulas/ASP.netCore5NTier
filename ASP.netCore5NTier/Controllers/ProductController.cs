using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP.netCore5NTier.Data;
using ASP.netCore5NTier.Models;
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
            IEnumerable<SelectListItem> CategoryDropDown = _db.Category.Select(p => new SelectListItem
            {

                Text = p.Name,
                Value = p.Id.ToString()
            });

            ViewBag.CategoryDropDown = CategoryDropDown;

            var product = new Product();
            if (id == null)
            {
                //this is for create
                return View(product);
            }
            else
            {
                product = _db.Product.FirstOrDefault(p => p.Id == product.Id);
                if (product == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(product);
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

        //Get Edit 
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _db.Category.FirstOrDefault(p => p.Id == id);

            if (obj == null)
                return NotFound();

            return View(obj);
        }

        //POST edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _db.Category.Update(category);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }

        //Get Delete 
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _db.Category.Find(id);

            if (obj == null)
                return NotFound();

            return View(obj);
        }

        //POST edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Category.Find(id);
            if (obj == null)
                return NotFound();

            _db.Category.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}
