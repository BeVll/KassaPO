using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateBase.Models
{
    public class UserContext : DbContext
    {
        public DbSet<Delievery> Delieveries { get; set; }
        public DbSet<Goods> Goods { get; set; }
        public DbSet<Sell> Sells { get; set; }
        public DbSet<User> Users { get; set; }
        public UserContext()
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=BEVL;Initial Catalog=KassaShop;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
    }
}
