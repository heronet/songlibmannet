using Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Data
{
    public class DataContext : IdentityDbContext<EndUser>
    {
        public DataContext(DbContextOptions options) : base(options)
        { }
        public DbSet<Song> Songs { get; set; }
    }
}