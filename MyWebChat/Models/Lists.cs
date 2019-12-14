using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebChat.Models
{
    public class Lists
    {
        public static List<User> ActiveUsers { get; set; }
        public static List<User> AllUsers { get; set; }
        public static List<Message> UserMessages { get; set; }
    }
}