using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using ASP.netCore5NTier.Data;
using ASP.netCore5NTier.Models;
using ASP.netCore5NTier.Models.ViewModels;
using ASP.netCore5NTier.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace ASP.netCore5NTier.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDBContext _db;
        [BindProperty ]
        public ProductUserVM ProductUserVm { get; set; }


        public CartController(ApplicationDBContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<ShoppingCart> shoppingCarts = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                //session exist
                shoppingCarts = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);

            }

            var prodInCart = shoppingCarts.Select(p => p.ProductId).ToList();
            var prodList = _db.Product.Where(p => prodInCart.Contains(p.Id));

            return View(prodList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            
            return RedirectToAction(nameof(Summary));
        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            List<ShoppingCart> shoppingCarts = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                //session exist
                shoppingCarts = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);

            }

            var prodInCart = shoppingCarts.Select(p => p.ProductId).ToList();
            var prodList = _db.Product.Where(p => prodInCart.Contains(p.Id)).ToList();


            ProductUserVm = new ProductUserVM()
            {
                ApplicationUser = _db.ApplicationUser.FirstOrDefault(p=>p.Id == claim.Value),
                Products = prodList
            };

            return View(ProductUserVm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public IActionResult SummaryPost()
        {
           HttpContext.Session.Clear();

            return RedirectToAction(nameof(InquiryConfirmation));
        }


        public IActionResult InquiryConfirmation()
        {

            return View();
        }

        public IActionResult Remove(int id)
        {
            List<ShoppingCart> shoppingCarts = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                //session exist
                shoppingCarts = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);

            }

            shoppingCarts.Remove(shoppingCarts.FirstOrDefault(p => p.ProductId == id));
            HttpContext.Session.Set(WC.SessionCart, shoppingCarts);

            return RedirectToAction(nameof(Index));
        }


    }
}
