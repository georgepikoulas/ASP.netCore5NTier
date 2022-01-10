using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using ASP.netCore5NTier.Data;
using ASP.netCore5NTier.Models;
using ASP.netCore5NTier.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ASP.netCore5NTier.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDBContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(ApplicationDBContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
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
                productVM.Product = _db.Product.FirstOrDefault(p => p.Id ==id);
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
        public IActionResult Upsert(ProductVM  productVm)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if (productVm.Product.Id == 0)
                {
                    //Creating
                    var upload = webRootPath + WC.ImagePath;
                    var fileName = Guid.NewGuid().ToString();
                    var extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload,fileName + extension ), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productVm.Product.Image = fileName + extension;
                    _db.Product.Add(productVm.Product);

                }
                else
                {
                    //Updating 
                }
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View();
        }


    }
}
