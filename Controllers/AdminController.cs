using EasyBuy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasyBuy.Controllers
{
    public class AdminController : Controller
    {
        private MyContexct _context;
        private IWebHostEnvironment _env;
        public AdminController(MyContexct context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            string admin = HttpContext.Session.GetString("admin_session");
            if(admin != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }   
        }
        public IActionResult Login()
        {
            return View(); 
        }
        [HttpPost]
        public IActionResult Login(string adminEmail, string adminPassword)
        {
            var row=_context.tbl_admin.FirstOrDefault(a => a.admin_email == adminEmail);
            if (row != null && row.admin_password == adminPassword)
            {
                HttpContext.Session.SetString("admin_session", row.admin_id.ToString());
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.message = "Incorrect Username of Password";
            }
                return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("admin_session");
            return RedirectToAction("login");
        }

        public IActionResult Profile()
        {
            var admin = HttpContext.Session.GetString("admin_session");
            var row=_context.tbl_admin.Where(a=>a.admin_id==int.Parse(admin)).ToList();
            return View(row);
        }
        [HttpPost]
        public IActionResult Profile(Admin admin)
        {
            _context.tbl_admin.Update(admin);
            _context.SaveChanges();
            return RedirectToAction("Profile");
        }
        [HttpPost]
        public IActionResult ChangeProfileImage(IFormFile admin_image, Admin admin) 
        {
            string ImagePath = Path.Combine(_env.WebRootPath, "admin_image", admin_image.FileName);
            FileStream fs=new FileStream(ImagePath, FileMode.Create);
            admin_image.CopyTo(fs);
            admin.admin_image = admin_image.FileName;
            _context.tbl_admin.Update(admin);
            _context.SaveChanges();
            return RedirectToAction("Profile");
        }
        public IActionResult FetchCustomer()
        {
            return View(_context.tbl_customer.ToList());
        }
        public IActionResult CustomerDetails(int id)
        {
            return View(_context.tbl_customer.FirstOrDefault(c => c.constomer_id == id));
        }
        public IActionResult UpdateCustomer(int id)
        {
            return View(_context.tbl_customer.Find(id));
        }
        [HttpPost]
        public IActionResult UpdateCustomer(Customer cust, IFormFile constomer_image)
        {
            string ImagePath = Path.Combine(_env.WebRootPath, "customer_images", constomer_image.FileName);
            FileStream fs = new FileStream(ImagePath, FileMode.Create);
            constomer_image.CopyTo(fs);
            cust.constomer_image = constomer_image.FileName;
            _context.tbl_customer.Update(cust);
            _context.SaveChanges();
            return RedirectToAction("CustomerDetails", new { id = cust.constomer_id });

        }
        public IActionResult DeletePermission(int id)
        {
            return View(_context.tbl_customer.FirstOrDefault(c => c.constomer_id == id));
        }
        public IActionResult deleteCustomer(int id)
        {
            var cust = _context.tbl_customer.Find(id);
            _context.tbl_customer.Remove(cust);
            _context.SaveChanges();
            return RedirectToAction("FetchCustomer");
        }
        public IActionResult FetchCategory()
        {
            return View(_context.tbl_category.ToList());
        }
        public IActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddCategory(Category cat)
        {
            _context.tbl_category.Add(cat);
            _context.SaveChanges();
            return RedirectToAction("FetchCategory");
        }
        public IActionResult UpdateCategory(int id)
        {
            var category=_context.tbl_category.Find(id);
            return View(category);
        }
        [HttpPost]
        public IActionResult UpdateCategory(Category cat)
        {
            _context.tbl_category.Update(cat);
            _context.SaveChanges();
            return RedirectToAction("FetchCategory");
        }
        public IActionResult DeletePermissionCategory(int id)
        {
            return View(_context.tbl_category.FirstOrDefault(c => c.category_id == id));
        }
        public IActionResult DeleteCategory(int id)
        {
            var category = _context.tbl_category.Find(id);
            _context.tbl_category.Remove(category);
            _context.SaveChanges();
            return RedirectToAction("FetchCategory");           return View();
        }
        public IActionResult FetchProduct()
        {
            return View(_context.tbl_product.ToList()); 
        }
        public IActionResult AddProduct()
        {
            List<Category> categories=_context.tbl_category.ToList();
            ViewData["category"] = categories;
            return View();
        }
        [HttpPost]
        public IActionResult AddProduct(Product prod,IFormFile product_image)
        {
            string imageName=Path.GetFileName(product_image.FileName);
            string imagePath = Path.Combine(_env.WebRootPath, "product_images",imageName);
            FileStream fs=new FileStream(imagePath, FileMode.Create);   
            product_image.CopyTo(fs);
            prod.product_image = imageName;
            _context.tbl_product.Add(prod);
            _context.SaveChanges();
            return RedirectToAction("FetchProduct");
        }
        public IActionResult ProductDetails(int id)
        {
            return View(_context.tbl_product.Include(p=>p.Category).FirstOrDefault(p=>p.product_id==id));
        }
        public IActionResult DeletePermissionProduct(int id)
        {
            return View(_context.tbl_product.FirstOrDefault(p => p.product_id == id));
        }
        public IActionResult DeleteProduct(int id)
        {
            var prod = _context.tbl_product.Find(id);
            _context.tbl_product.Remove(prod);
            _context.SaveChanges();
            return RedirectToAction("FetchProduct"); return View();
        }
        public IActionResult UpdateProduct(int id)
        {
            List<Category> categories = _context.tbl_category.ToList();
            ViewData["category"] = categories;
            
            var prod = _context.tbl_product.Find(id);
            ViewBag.selectedCategoryId = prod.cat_id;
            return View(prod);
        }
        [HttpPost]
        public IActionResult UpdateProduct(Product prod)
        {
            _context.tbl_product.Update(prod);
            _context.SaveChanges();
            return RedirectToAction("FetchProduct");
        }
        [HttpPost]
        public IActionResult ChangeProductImage(IFormFile product_image, Product prod)
        {
            string ImagePath = Path.Combine(_env.WebRootPath, "product_images", product_image.FileName);
            FileStream fs = new FileStream(ImagePath, FileMode.Create);
            product_image.CopyTo(fs);
            prod.product_image = product_image.FileName;
            _context.tbl_product.Update(prod);
            _context.SaveChanges();
            return RedirectToAction("FetchProduct");
        }
        public IActionResult fetchFeedback()
        {    
            return View(_context.tbl_feedback.ToList());
        }
        public IActionResult DeletePermissionFeedback(int id)
        {
            return View(_context.tbl_feedback.FirstOrDefault(f => f.feedback_id == id));
        }
        public IActionResult DeleteFeedback(int id)
        {
            var feed = _context.tbl_feedback.Find(id);
            _context.tbl_feedback.Remove(feed);
            _context.SaveChanges();
            return RedirectToAction("fetchFeedback"); return View();
        }
    }
}
