using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using ASP.netCore5NTier.Data;
using ASP.netCore5NTier.Data.Repository;
using ASP.netCore5NTier.Data.Repository.IRepository;
using ASP.netCore5NTier.Models;
using ASP.netCore5NTier.Models.ViewModels;
using ASP.netCore5NTier.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ASP.netCore5NTier.Controllers
{
    [Authorize(Roles = WC.AdminRole)]

    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IProductRepository productRepository, IWebHostEnvironment webHostEnvironment)
        {
            _productRepository = productRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> products = _productRepository.GetAll(includeProperties:$"{nameof(Category)},{nameof(ApplicationType)}"  );

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
                CategoryListItems = _productRepository.GetAllDropdownList(WC.CategoryName),
                ApplicationTypeSelectList = _productRepository.GetAllDropdownList(WC.ApplicationTypeName),
            };

            if (id == null)
            {
                //this is for create
                return View(productVM);
            }
            else
            {
                productVM.Product = _productRepository.Find(id.GetValueOrDefault());
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
                    _productRepository.Add(productVm.Product);

                }
                else
                {
                    //Updating 
                    var getProductDb = _productRepository.FirstOrDefault(p => p.Id == productVm.Product.Id , isTracking:false);

                    if (files.Count > 0)
                    {
                        var upload = webRootPath + WC.ImagePath;
                        var fileName = Guid.NewGuid().ToString();
                        var extension = Path.GetExtension(files[0].FileName);

                        var oldfile = Path.Combine(upload, getProductDb.Image);
                        if (System.IO.File.Exists(oldfile))
                        {
                            System.IO.File.Delete(oldfile);
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        productVm.Product.Image = fileName + extension;

                    }
                    else
                    {
                        productVm.Product.Image = getProductDb.Image;
                    }

                    _productRepository.Update(productVm.Product);
                }
                _productRepository.Save();

                return RedirectToAction("Index");
            }

            productVm.CategoryListItems = _productRepository.GetAllDropdownList(WC.CategoryName);
            productVm.ApplicationTypeSelectList = _productRepository.GetAllDropdownList(WC.ApplicationTypeName);

            return View(productVm);
        }

        //Get Delete 
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {

                return NotFound();
            }

            var product = _productRepository.FirstOrDefault(p => p.Id == id, includeProperties: $"{nameof(Category)},{nameof(ApplicationType)}");

            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var product = _productRepository.Find(id.GetValueOrDefault());
            if (product == null)
                return NotFound();
            var upload = _webHostEnvironment.WebRootPath + WC.ImagePath;

            var oldfile = Path.Combine(upload, product.Image);

            if (System.IO.File.Exists(oldfile))
            {
                System.IO.File.Delete(oldfile);
            }

            _productRepository.Remove(product);
            _productRepository.Save();
            return RedirectToAction("Index");
        }




    }
}
