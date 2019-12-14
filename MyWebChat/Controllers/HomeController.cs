using MyWebChat.Models;
using MyWebChat.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyWebChat.Controllers
{
    public class HomeController : Controller
    {
        private ModelContext _context;
        public HomeController()
        {
            _context = new ModelContext();
        }
        public ActionResult Index()
        {
            var viewModel = new NewRandomViewModel
            {
                Users = _context.Users.ToList()
            };
            return View(viewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}