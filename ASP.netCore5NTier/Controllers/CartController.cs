using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ASP.netCore5NTier.Data;
using ASP.netCore5NTier.Data.Repository.IRepository;
using ASP.netCore5NTier.Models;
using ASP.netCore5NTier.Models.ViewModels;
using ASP.netCore5NTier.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace ASP.netCore5NTier.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSender _emailSender;
        private readonly IProductRepository _productRepository;
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IInquiryHeaderRepository _inquiryHeaderRepository;
        private readonly IInquiryDetailRepository _inquiryDetailRepository;

        [BindProperty ]
        public ProductUserVM ProductUserVm { get; set; }


        public CartController( IWebHostEnvironment webHostEnvironment, IEmailSender emailSender,
            IProductRepository productRepository, IApplicationUserRepository applicationUserRepository,
            IInquiryHeaderRepository inquiryHeaderRepository, IInquiryDetailRepository inquiryDetailRepository)
        {
            _webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;
            _productRepository = productRepository;
            _applicationUserRepository = applicationUserRepository;
            _inquiryHeaderRepository = inquiryHeaderRepository;
            _inquiryDetailRepository = inquiryDetailRepository;
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
            var prodList = _productRepository.GetAll(p => prodInCart.Contains(p.Id));

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
            var prodList = _productRepository.GetAll(p => prodInCart.Contains(p.Id)).ToList();


            ProductUserVm = new ProductUserVM()
            {
                ApplicationUser = _applicationUserRepository.FirstOrDefault(p=>p.Id == claim.Value),
                Products = prodList
            };

            return View(ProductUserVm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPost()
        {
            //Get the claims of the User
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            //Conctrusting the Path to get the Inquiry.html template from wwwroot
            var pathToTemplate = _webHostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString() +
                                 "templates" + Path.DirectorySeparatorChar.ToString() + "Inquiry.html";

            var subject = "New Inquiry";
            string HtmlBody = "";
            using (StreamReader sr = System.IO.File.OpenText(pathToTemplate))
            {
                HtmlBody = sr.ReadToEnd();
            }

            //Name: { 0}
            //Email: { 1}
            //Phone: { 2}
            //Products: {3}

            StringBuilder productListSB = new StringBuilder();

            foreach (var prod in ProductUserVm.Products)
            {
                productListSB.Append(
                    $"- Name: {prod.Name} <span style='font-size:14px;'> ID : {prod.Id}</span><br/>");
            }

            string messageBody = string.Format(HtmlBody, ProductUserVm.ApplicationUser.FullName,
                ProductUserVm.ApplicationUser.Email, ProductUserVm.ApplicationUser.PhoneNumber,
                productListSB.ToString());

            await _emailSender.SendEmailAsync(WC.EmailAdmin, subject, messageBody);

            InquiryHeader inquiryHeader = new InquiryHeader()
            {
                ApplicationUserId = claim.Value,
                FullName = ProductUserVm.ApplicationUser.FullName,
                Email = ProductUserVm.ApplicationUser.Email,
                PhoneNumber = ProductUserVm.ApplicationUser.PhoneNumber,
                InquiryDate = DateTime.Now
            };                
            
            _inquiryHeaderRepository.Add(inquiryHeader);
            _inquiryHeaderRepository.Save();//To get hader ID from db for assosiating with new deatil

            foreach (var prod in ProductUserVm.Products)
            {
                InquiryDetail inquiryDetail = new InquiryDetail()
                {
                    InquiryHeaderId = inquiryHeader.Id,
                    ProductId = prod.Id,

                };
                _inquiryDetailRepository.Add(inquiryDetail);

            }
            _inquiryDetailRepository.Save();


            return RedirectToAction(nameof(InquiryConfirmation));
        }


        public IActionResult InquiryConfirmation()
        {
            HttpContext.Session.Clear();

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
