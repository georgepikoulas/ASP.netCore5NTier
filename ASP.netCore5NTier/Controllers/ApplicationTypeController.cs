using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP.netCore5NTier.Data;
using ASP.netCore5NTier.Data.Repository;
using ASP.netCore5NTier.Data.Repository.IRepository;
using ASP.netCore5NTier.Models;
using ASP.netCore5NTier.Utility;
using Microsoft.AspNetCore.Authorization;

namespace ASP.netCore5NTier.Controllers
{
    [Authorize(Roles = WC.AdminRole)] 

    public class ApplicationTypeController : Controller
    {
        private readonly IApplicationTypeRepository _applicationTypeRepository;

        public ApplicationTypeController(IApplicationTypeRepository applicationTypeRepository )
        {
            _applicationTypeRepository = applicationTypeRepository;
        }
        public IActionResult Index()
        {
            IEnumerable<ApplicationType> categories = _applicationTypeRepository.GetAll();
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
            _applicationTypeRepository.Add(applicationType);
            _applicationTypeRepository.Save();
            return RedirectToAction("Index");
        }
        //GET - EDIT
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _applicationTypeRepository.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ApplicationType obj)
        {
            if (ModelState.IsValid)
            {
                _applicationTypeRepository.Update(obj);
                _applicationTypeRepository.Save();
                return RedirectToAction("Index");
            }
            return View(obj);

        }

        //GET - DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _applicationTypeRepository.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //POST - DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _applicationTypeRepository.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }
            _applicationTypeRepository.Remove(obj);
            _applicationTypeRepository.Save();
            return RedirectToAction("Index");


        }
    }
}
