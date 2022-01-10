using ASP.netCore5NTier.Data;
using ASP.netCore5NTier.Models;
using ASP.netCore5NTier.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.netCore5NTier.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDBContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDBContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            var homeVM = new HomeVM() {
                Products = _db.Product.Include(p =>p.Category).Include(p=>p.ApplicationType),
                Categories =_db.Category
            
            };

            return View(homeVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Details(int id)
        {
            var detailsVM = new DetailsVM() { 
                Product = _db.Product.Include(p=>p.Category).Include(p => p.ApplicationType).Where(p=>p.Id == id ).FirstOrDefault(),
                ExistsInCart = false
            };


            return View(detailsVM);
        }
    }
}
