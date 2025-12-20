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
            return View();
        }
    }
}
