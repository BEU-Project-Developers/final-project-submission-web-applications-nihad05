using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyHosting> CompanyHostings { get; set; }
        public DbSet<Domain> Domains { get; set; }
        public DbSet<HostingPackage> HostingPackages { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Order> Orders { get; set; }
        //public DbSet<OrderItems> OrderItems { get; set; }
        public DbSet<Person> Persons { get; set; }
    }
}