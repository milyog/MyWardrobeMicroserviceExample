using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MyWardrobeStatistics.Models;

namespace MyWardrobeStatistics.Persistence
{
    public class MyWardrobeStatisticsDbContext : DbContext
    {
        public MyWardrobeStatisticsDbContext(DbContextOptions<MyWardrobeStatisticsDbContext> options) : base(options)
        {

        }

        public DbSet<WardrobeItemStatistics> WardrobeItemsStatistics { get; set; }
    }
}
