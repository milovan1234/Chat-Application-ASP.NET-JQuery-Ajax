using MyWebChat.Models;
using MyWebChat.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyWebChat.Controllers
{
    public class RegisterController : Controller
    {
        private ModelContext _context;
        public RegisterController()
        {
            _context = new ModelContext();
        }
        public ActionResult RegisterPage()
        {
            if (Session["Id"] == null)
                return View();
            else
                return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult Save(User user, HttpPostedFileBase ImageFile)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new NewRandomViewModel
                {
                    User = user
                };
                return View("RegisterPage", viewModel);
            }
            if (user.Id == 0)
            {
                if (ImageFile != null)
                {
                    string filename = Path.GetFileName(ImageFile.FileName);
                    user.ImagePath = "/Images/" + filename;
                    filename = Path.Combine(Server.MapPath("~/Images/"), filename);
                    ImageFile.SaveAs(filename);
                }
                else
                {
                    user.ImagePath = "/Images/LOG-IN.png";
                }
                user.UserRank = "User";
                user.Password = EncDecPassword.EncryptPassword(user.Password);
                user.LastLogin = DateTime.Now;
                _context.Users.Add(user);
                Session["Register"] = "true";
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}