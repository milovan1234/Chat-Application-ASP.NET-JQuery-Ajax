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
    public class UserController : Controller
    {
        private ModelContext _context;
        public UserController()
        {
            _context = new ModelContext();
            if (Lists.ActiveUsers == null)
                Lists.ActiveUsers = new List<User>();
        }
        public ActionResult ComunicationsPage()
        {
            if (Session["Id"] != null && (Session["UserRank"].ToString() == "User"))
            {
                int userId = Convert.ToInt32(Session["Id"]);
                var viewModel = new NewRandomViewModel
                {
                    Users = _context.Users.Where(u => u.Id != userId).ToList(),
                    ActiveUsers = Lists.ActiveUsers.Where(u => u.Id != userId).ToList()
                };
                return View(viewModel);
            }
            return HttpNotFound();
        }
        public ActionResult YourProfile()
        {
            if (Session["Id"] != null && Session["UserRank"].ToString() == "User")
            {
                int userId = Convert.ToInt32(Session["Id"]);
                if (Session["Id"] != null && (Session["UserRank"].ToString() == "User"))
                {
                    var userInDb = _context.Users.FirstOrDefault(u => u.Id == userId);
                    userInDb.Password = EncDecPassword.DecryptPassword(userInDb.Password);
                    var viewModel = new NewRandomViewModel
                    {
                        User = userInDb
                    };
                    return View(viewModel);
                }
                return HttpNotFound();
            }
            return HttpNotFound();
        }
        public ActionResult SaveData(User user, HttpPostedFileBase UserImage)
        {
            if (Session["Id"] != null && (Session["UserRank"].ToString() == "User"))
            {
                if (!ModelState.IsValid)
                {
                    var viewModel = new NewRandomViewModel
                    {
                        User = user
                    };
                    return View("YourProfile", viewModel);
                }
                var userInDb = _context.Users.FirstOrDefault(u => u.Id == user.Id);
                if (userInDb == null)
                    return HttpNotFound();
                userInDb.FirstName = user.FirstName;
                userInDb.LastName = user.LastName;
                userInDb.Password = EncDecPassword.EncryptPassword(user.Password);
                if (UserImage != null)
                {
                    string filename = Path.GetFileName(UserImage.FileName);
                    userInDb.ImagePath = "/Images/" + filename;
                    filename = Path.Combine(Server.MapPath("~/Images/"), filename);
                    UserImage.SaveAs(filename);
                }
                _context.SaveChanges();
                Session["FirstName"] = userInDb.FirstName;
                Session["ImagePath"] = userInDb.ImagePath;
                Session["UpdateUser"] = "true";
                var model = new NewRandomViewModel
                {
                    User = _context.Users.FirstOrDefault(u => u.Id == user.Id)
                };
                return View("YourProfile", model);
            }
            return HttpNotFound();
        }
        [HttpPut]
        public ActionResult DeleteUserPhoto(int id)
        {
            if (Session["Id"] != null && Session["UserRank"].ToString() == "User")
            {
                var userInDb = _context.Users.FirstOrDefault(u => u.Id == id);
                if (userInDb == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    userInDb.ImagePath = "/Images/LOG-IN.png";
                    Session["ImagePath"] = "/Images/LOG-IN.png";
                    _context.SaveChanges();
                    var viewModel = new NewRandomViewModel
                    {
                        User = userInDb
                    };
                    return View("YourProfile", viewModel);
                }
            }
            return HttpNotFound();
        }
        public JsonResult ActiveUsers()
        {
            if (Session["Id"] != null && Session["UserRank"].ToString() == "User")
            {
                int userId = Convert.ToInt32(Session["Id"]);
                return Json(Lists.ActiveUsers.Where(u => u.Id != userId), JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public JsonResult NonActiveUsers()
        {
            if (Session["Id"] != null && Session["UserRank"].ToString() == "User")
            {
                bool check = false;
                List<User> allUsers = _context.Users.ToList();
                List<User> nonActiveUsers = new List<User>();
                for (int i = 0; i < _context.Users.Count(); i++)
                {
                    check = false;
                    for (int j = 0; j < Lists.ActiveUsers.Count(); j++)
                    {
                        if (allUsers[i].Id == Lists.ActiveUsers[j].Id)
                        {
                            check = true;
                            break;
                        }
                    }
                    if (!check)
                    {
                        nonActiveUsers.Add(allUsers[i]);
                    }
                }
                return Json(nonActiveUsers, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public JsonResult AllUsers()
        {
            int userId = Convert.ToInt32(Session["Id"]);
            return Json(_context.Users.Where(u => u.Id != userId), JsonRequestBehavior.AllowGet);
        }
        public JsonResult AllUsersMessages()
        {
            if (Session["Id"] != null && (Session["UserRank"].ToString() == "User"))
            {
                int userId = Convert.ToInt32(Session["Id"]);
                User userInDb = _context.Users.FirstOrDefault(u => u.Id == userId);
                List<UserNewMessages> listNewMessages = new List<UserNewMessages>();
                List<User> allUsers = _context.Users.Where(u => u.Id != userId).ToList();
                string fileName = "";
                foreach (var user in allUsers)
                {
                    fileName = user.Email.Substring(0, user.Email.IndexOf("@")) + userInDb.Email.Substring(0, userInDb.Email.IndexOf("@")) + ".json";
                    List<Message> listMessages = new List<Message>();
                    if (WriteReadJsonFile.CheckExistsFile(fileName))
                    {
                        WriteReadJsonFile.ReadList<Message>(ref listMessages, fileName);
                        int countMessage = 0;
                        if (listMessages != null)
                        {
                            foreach (var message in listMessages)
                            {
                                if (message.UserToId == userId && !message.ReadMessage)
                                {
                                    countMessage++;
                                }
                            }
                            listNewMessages.Add(new UserNewMessages
                            {
                                UserId = user.Id,
                                CountNewMessages = countMessage
                            });
                        }
                        else
                        {
                            listNewMessages.Add(new UserNewMessages
                            {
                                UserId = user.Id,
                                CountNewMessages = 0
                            });
                        }
                    }
                    else
                    {
                        listNewMessages.Add(new UserNewMessages
                        {
                            UserId = user.Id,
                            CountNewMessages = 0
                        });
                    }

                }
                return Json(listNewMessages, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}