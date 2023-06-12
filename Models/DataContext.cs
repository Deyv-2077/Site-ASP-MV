using Microsoft.EntityFrameworkCore;
using System.Security.Policy;
using System;
using MVCApp2.Models;


namespace MVCApp2.Models
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options):base(options)  
        {




        }


        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }

    }
}
