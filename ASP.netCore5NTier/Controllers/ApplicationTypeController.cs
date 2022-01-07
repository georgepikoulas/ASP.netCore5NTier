using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP.netCore5NTier.Data;
using ASP.netCore5NTier.Models;

namespace ASP.netCore5NTier.Controllers
{
    public class ApplicationTypeController : Controller
    {
        private readonly ApplicationDBContext _db;


        public ApplicationTypeController(ApplicationDBContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<ApplicationType> categories = _db.ApplicationType;
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
        public IActionResult Create(ApplicationType applicationType)
        {
            _db.ApplicationType.Add(applicationType);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
