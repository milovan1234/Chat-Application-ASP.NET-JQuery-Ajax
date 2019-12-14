using MyWebChat.Models;
using MyWebChat.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyWebChat.Controllers
{
    public class LoginController : Controller
    {
        private ModelContext _context;
        public LoginController()
        {
            _context = new ModelContext();
            if (Lists.ActiveUsers == null)
                Lists.ActiveUsers = new List<User>();
            if (Lists.AllUsers == null)
                Lists.AllUsers = new List<User>();

        }
        public ActionResult LoginPage()
        {
            if (Session["Id"] == null)
                return View();
            else
                return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var userInDb = _context.Users.FirstOrDefault(u => u.Email == email);
            if (userInDb == null)
            {
                Session["ErrorMessage"] = "Account not existing. Go to registration.";
                return RedirectToAction("LoginPage", "Login");
            }
            else
            {
                var pass = EncDecPassword.DecryptPassword(userInDb.Password);
                userInDb = password == pass ? userInDb : null;
                if (userInDb == null)
                {
                    Session["ErrorMessage"] = "The password is incorrect. Check the 'caps lock'.";
                    return RedirectToAction("LoginPage","Login");
                }
                else
                {
                    Session["Id"] = userInDb.Id;
                    Session["ImagePath"] = userInDb.ImagePath;
                    Session["FirstName"] = userInDb.FirstName;
                    Session["UserRank"] = userInDb.UserRank;
                    Session["Email"] = userInDb.Email;
                    Session["ErrorMessage"] = null;
                    //bool check = true;
                    //for (int i = 0; i < Lists.ActiveUsers.Count(); i++)
                    //{
                    //    if (Lists.ActiveUsers[i].Id == Convert.ToInt32(Session["Id"]))
                    //        check = false;
                    //}
                    //if(check)
                    Lists.ActiveUsers.Add(userInDb);
                    return RedirectToAction("Index", "Home");
                }
            }
        }
        public ActionResult Logout()
        {
            if (Session["Id"] != null && Session["UserRank"].ToString() == "User")
            {
                int userId = Convert.ToInt32(Session["Id"]);
                var userInDb = _context.Users.Single(u => u.Id == userId);
                userInDb.LastLogin = DateTime.Now;
                _context.SaveChanges();
                for (int i = 0; i < Lists.ActiveUsers.Count(); i++)
                {
                    if (Lists.ActiveUsers[i].Id == userInDb.Id)
                    {
                        Lists.ActiveUsers.RemoveAt(i);
                        break;
                    }
                }
                Session.Abandon();
                return RedirectToAction("Index", "Home");
            }
            return HttpNotFound();
        }
    }
}