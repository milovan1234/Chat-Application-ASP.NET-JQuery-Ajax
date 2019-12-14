using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyWebChat.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Frist Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Your Email")]
        [AccountExisting]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Your Passowrd")]
        public string Password { get; set; }
        [Display(Name = "Your Image")]
        public string ImagePath { get; set; }  
        public string UserRank { get; set; }
        public DateTime LastLogin { get; set; }
    }
}