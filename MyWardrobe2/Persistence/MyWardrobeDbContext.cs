using Microsoft.EntityFrameworkCore;
using MyWardrobe.Models;

namespace MyWardrobe.Persistence
{
    public class MyWardrobeDbContext : DbContext
    {
        public MyWardrobeDbContext (DbContextOptions<MyWardrobeDbContext> options) : base(options) 
        { 
            
        }

        public DbSet<WardrobeItem> WardrobeItems {  get; set; }
       
    }
}
