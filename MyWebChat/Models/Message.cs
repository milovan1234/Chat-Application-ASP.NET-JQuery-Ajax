using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebChat.Models
{
    public class Message
    {
        public int Id { get; set; }
        public User UserFrom { get; set; }
        public int UserFromId { get; set; }
        public User UserTo { get; set; }
        public int UserToId { get; set; }
        public string MessageText { get; set; }
        public DateTime DateTimeSend { get; set; }
        public bool ReadMessage { get; set; }
    }
}