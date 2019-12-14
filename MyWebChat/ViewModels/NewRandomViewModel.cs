using MyWebChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebChat.ViewModels
{
    public class NewRandomViewModel
    {
        public User User { get; set; }
        /*All Users*/
        public List<User> Users { get; set; }
        public List<User> ActiveUsers { get; set; }
        /*Messages*/
        public Message Message { get; set; }
        public List<Message> UserMessages { get; set; }
        public string Filename { get; set; }
    }
}