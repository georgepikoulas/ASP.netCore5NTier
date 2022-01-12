using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ASP.netCore5NTier.Data;
using ASP.netCore5NTier.Models;
using ASP.netCore5NTier.Utility;
using Microsoft.AspNetCore.Http;

namespace ASP.netCore5NTier.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDBContext _db;

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
