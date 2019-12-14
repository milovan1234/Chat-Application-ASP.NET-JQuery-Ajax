using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebChat.Models
{
    public class UserNewMessages
    {
        public int UserId { get; set; }
        public int CountNewMessages { get; set; }
    }
}