using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MyWebChat.Models
{
    public class ModelContext : DbContext
    {
        public ModelContext() : base("MyWebAppDB")
        {
        }
        public DbSet<User> Users { get; set; }
    }
}