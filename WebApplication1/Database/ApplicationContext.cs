using Microsoft.EntityFrameworkCore;
using System;

namespace Database
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Document> Documents { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=DocumentDB;Trusted_Connection=True");
            }
            base.OnConfiguring(optionsBuilder);

        }
    }
}
