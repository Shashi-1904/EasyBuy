using EasyBuy.Models;
using Microsoft.AspNetCore.Mvc;

namespace EasyBuy.Controllers
{
    public class CustomerController : Controller
    {
        private MyContexct _context;
        private IWebHostEnvironment _env;
        public CustomerController(MyContexct context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            List<Category> category = _context.tbl_category.ToList();
            ViewData["category"]=category;
            ViewBag.CheckSession = HttpContext.Session.GetString("customerSession");
            return View();
        }
        public IActionResult CustomerLogin()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CustomerLogin(string CustomerEmail, string CustomerPass)
        {
            var cust = _context.tbl_customer.FirstOrDefault(c=> c.constomer_email==CustomerEmail);
            if (cust!=null && cust.constomer_password==CustomerPass)
            {
                HttpContext.Session.SetString("customerSession",cust.constomer_id.ToString());
                TempData["SuccessMsg"] = $"Login successful! Welcome {cust.constomer_name}";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.msg = "Incorrect Email or Password!!";
                return View();
            }   
        }
        public IActionResult CustomerRegister()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CustomerRegister(Customer cust)
        {
            _context.tbl_customer.Add(cust);
            _context.SaveChanges();
            return RedirectToAction("CustomerLogin");
        }
        public IActionResult CustomerLogout()
        {
            HttpContext.Session.Remove("customerSession");
            return RedirectToAction("CustomerLogin");
        }
        public IActionResult CustomerProfile()
        {
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("customerSession")))
            {
                return RedirectToAction("CustomerLogin");
            }
            else
            {
                var customerId = HttpContext.Session.GetString("customerSession");
                var row = _context.tbl_customer.Where(c => c.constomer_id == int.Parse(customerId)).ToList();
                return View(row);
            }
               
        }
        [HttpPost]
        public IActionResult UpdateProfile(Customer cust)
        {
            _context.tbl_customer.Update(cust);
            _context.SaveChanges();
            TempData["ProfileUpdateSuccessMsg"] = "Profile Updated Sucessfully!!!";
            return RedirectToAction("CustomerProfile");
        }

    }
}
