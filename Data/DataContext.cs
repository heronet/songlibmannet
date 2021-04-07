using Entities;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        { }
        public DbSet<Song> Songs { get; set; }
        public DbSet<EndUser> EndUsers { get; set; }
    }
}