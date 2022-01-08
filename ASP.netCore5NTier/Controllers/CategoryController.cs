using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP.netCore5NTier.Data;
using ASP.netCore5NTier.Models;
using Microsoft.EntityFrameworkCore;

namespace ASP.netCore5NTier.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDBContext _db;


        public CategoryController(ApplicationDBContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> categories = _db.Category;
            return View(categories);
        }

        //Get Category 
        public IActionResult Create()
        {
            
            return View();
        }

        //POST create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
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

        //POST create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _db.Category.Update( category);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }
    }
}
