using EasyBuy.Models;
using EasyBuy.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            ViewData["category"] = category;

            List<Product> pro = _context.tbl_product.ToList();
            ViewData["Product"] = pro;

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
            var cust = _context.tbl_customer.FirstOrDefault(c => c.constomer_email == CustomerEmail);
            if (cust != null && cust.constomer_password == CustomerPass)
            {
                HttpContext.Session.SetString("customerSession", cust.constomer_id.ToString());
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
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("customerSession")))
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

        public IActionResult ChangeProfileImage(Customer cust, IFormFile constomer_image)
        {
            string ImagePath = Path.Combine(_env.WebRootPath, "customer_images", constomer_image.FileName);
            FileStream fs = new FileStream(ImagePath, FileMode.Create);
            constomer_image.CopyTo(fs);
            cust.constomer_image = constomer_image.FileName;
            _context.tbl_customer.Update(cust);
            _context.SaveChanges();
            return RedirectToAction("CustomerProfile");
        }
        public IActionResult Feedback()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Feedback(Feedback feed)
        {
            TempData["feedmsg"] = "Thank you for your feedback!!";
            _context.tbl_feedback.Add(feed);
            _context.SaveChanges();
            return RedirectToAction("Feedback");
        }
        public IActionResult FetchAllProducts()
        {
            ProductCategoryVM vm = new ProductCategoryVM()
            {
                Categories = _context.tbl_category.ToList(),
                Products = _context.tbl_product.ToList()
            };

            return View(vm);

        }
        public IActionResult ProductDetails(int id)
        {
            var product = _context.tbl_product
                .Include(p => p.Category)
                .Where(p => p.product_id == id)
                .ToList();

            return View(product);
        }
        public IActionResult AddToCart(int product_id, Cart cart)
        {
            string isLogin=HttpContext.Session.GetString("customerSession");
            if(isLogin!=null)
            {
                cart.prod_id = product_id;
                cart.cust_id = int.Parse(isLogin);
                cart.product_quantity = 1;
                cart.cart_status = 0;
                _context.tbl_cart.Add(cart);
                _context.SaveChanges();
                TempData["CartMsg"] = "Product Succesfully Added in Cart";
                return RedirectToAction("FetchAllProducts");
            }
            else
            {
                return RedirectToAction("CustomerLogin");             
            }
               
        }
    }
}
