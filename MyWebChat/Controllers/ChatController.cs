using MyWebChat.Models;
using MyWebChat.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyWebChat.Controllers
{
    public class ChatController : Controller
    {
        private ModelContext _context;
        public ChatController()
        {
            _context = new ModelContext();
        }
        public ActionResult ChatPage(int userToId)
        {
            if (Session["Id"] != null && Session["UserRank"].ToString() == "User")
            {                
                int userFromId = Convert.ToInt32(Session["Id"]);                
                if (userFromId == userToId)
                    return HttpNotFound();
                User userFrom = _context.Users.FirstOrDefault(u => u.Id == userFromId);
                User userTo = _context.Users.FirstOrDefault(u => u.Id == userToId);
                string fileName = userFrom.Email.Substring(0, userFrom.Email.IndexOf("@")) + userTo.Email.Substring(0, userTo.Email.IndexOf("@")) + ".json";
                List<Message> listMessages = new List<Message>();
                WriteReadJsonFile.ReadList<Message>(ref listMessages, fileName);
                if (listMessages != null)
                {
                    foreach (var m in listMessages)
                    {
                        m.UserFrom = _context.Users.Single(u => u.Id == m.UserFromId);
                        m.UserTo = _context.Users.Single(u => u.Id == m.UserToId);
                        if (m.UserToId == userFromId)
                            m.ReadMessage = true;
                    }
                }
                string fileName1 = fileName;
                string fileName2 = userTo.Email.Substring(0, userTo.Email.IndexOf("@")) + userFrom.Email.Substring(0, userFrom.Email.IndexOf("@")) + ".json";
                WriteReadJsonFile.WriteList(listMessages, fileName1,fileName2);
                var viewModel = new NewRandomViewModel
                {
                    Message = new Message
                    {
                        UserFromId = userFromId,
                        UserFrom = userFrom,
                        UserToId = userToId,
                        UserTo = userTo
                    },
                    UserMessages = listMessages,
                    Filename = fileName
                };
                return View(viewModel);
            }
            return HttpNotFound();
        }
        public JsonResult GetMessages(string fileName,int userToId)
        {
            if (Session["Id"] != null && Session["UserRank"].ToString() == "User")
            {
                int userFromId = Convert.ToInt32(Session["Id"]);
                List<Message> listMessages = new List<Message>();
                User userFrom = _context.Users.FirstOrDefault(u => u.Id == userFromId);
                User userTo = _context.Users.FirstOrDefault(u => u.Id == userToId);
                string fileName1 = userFrom.Email.Substring(0, userFrom.Email.IndexOf("@")) + userTo.Email.Substring(0, userTo.Email.IndexOf("@")) + ".json";
                string fileName2 = userTo.Email.Substring(0, userTo.Email.IndexOf("@")) + userFrom.Email.Substring(0, userFrom.Email.IndexOf("@")) + ".json";
                if (fileName1 == fileName)
                {
                    WriteReadJsonFile.ReadList(ref listMessages, fileName);
                    if (listMessages != null)
                    {
                        foreach (var m in listMessages)
                        {
                            m.UserFrom = _context.Users.Single(u => u.Id == m.UserFromId);
                            m.UserTo = _context.Users.Single(u => u.Id == m.UserToId);
                            if (m.UserToId == userFromId)
                                m.ReadMessage = true;
                        }
                        WriteReadJsonFile.WriteList(listMessages, fileName1, fileName2);
                    }
                    return Json(listMessages, JsonRequestBehavior.AllowGet);
                }
                return Json("", JsonRequestBehavior.AllowGet);
            }
            return
                Json("", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SendMessage(Message userMessage)
        {
            if (Session["Id"] != null && Session["UserRank"].ToString() == "User")
            {
                List<Message> listMessages = new List<Message>();
                userMessage.DateTimeSend = DateTime.Now;
                userMessage.ReadMessage = false;
                var userFrom = _context.Users.Single(u => u.Id == userMessage.UserFromId);
                var userTo = _context.Users.Single(u => u.Id == userMessage.UserToId);
                string fileName1 = userFrom.Email.Substring(0, userFrom.Email.IndexOf("@")) + userTo.Email.Substring(0, userTo.Email.IndexOf("@")) + ".json";
                string fileName2 = userTo.Email.Substring(0, userTo.Email.IndexOf("@")) + userFrom.Email.Substring(0, userFrom.Email.IndexOf("@")) + ".json"; ;

                WriteReadJsonFile.ReadList<Message>(ref listMessages, fileName1);
                if (listMessages == null)
                    listMessages = new List<Message>();
                listMessages.Add(userMessage);
                WriteReadJsonFile.WriteList<Message>(listMessages, fileName1, fileName2);
                if (listMessages != null)
                {
                    foreach (var m in listMessages)
                    {
                        m.UserFrom = _context.Users.Single(u => u.Id == m.UserFromId);
                        m.UserTo = _context.Users.Single(u => u.Id == m.UserToId);
                    }
                }
                var viewModel = new NewRandomViewModel
                {
                    Message = new Message
                    {
                        UserFrom = userFrom,
                        UserTo = userTo,
                        UserToId = userTo.Id,
                        UserFromId = userFrom.Id
                    },
                    UserMessages = listMessages,
                    Filename = fileName1
                };
                return View("ChatPage", viewModel);
            }            
            return HttpNotFound();
        }
        public ActionResult ViewUserProfile(int userToId)
        {
            int userFromId = Convert.ToInt32(Session["Id"]);
            if (Session["Id"] != null && Session["UserRank"].ToString() == "User" && userToId != userFromId)
            {
                User userInDb = _context.Users.FirstOrDefault(u => u.Id == userToId);
                var viewModel = new NewRandomViewModel
                {
                    User = userInDb
                };
                return View(viewModel);
            }
            return HttpNotFound();
        }
    }
}