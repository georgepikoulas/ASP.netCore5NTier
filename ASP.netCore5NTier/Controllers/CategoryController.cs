using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP.netCore5NTier.Data;
using ASP.netCore5NTier.Data.Repository.IRepository;
using ASP.netCore5NTier.Models;
using ASP.netCore5NTier.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ASP.netCore5NTier.Controllers
{

    [Authorize(Roles = WC.AdminRole)]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;


        public CategoryController(ICategoryRepository categoryRepository )
        {
            _categoryRepository = categoryRepository;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> categories = _categoryRepository.GetAll();
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
                _categoryRepository.Add(category);
                _categoryRepository.Save();
                return RedirectToAction(nameof(Index));
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

            var obj = _categoryRepository.FirstOrDefault(p => p.Id == id);

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
                _categoryRepository.Update(category);
                _categoryRepository.Save();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        //Get Delete 
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _categoryRepository.Find(id.GetValueOrDefault());

            if (obj == null)
                return NotFound();

            return View(obj);
        }

        //POST edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _categoryRepository.Find(id.GetValueOrDefault());
            if (obj == null)
                return NotFound();

            _categoryRepository.Remove(obj);
            _categoryRepository.Save();
            return RedirectToAction("Index");

        }
    }
}
