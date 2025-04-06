using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWardrobeStatistics.Persistence;
using MyWardrobeStatistics.Models;
using RabbitMQ.Client;

namespace MyWardrobeStatistics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WardrobeItemStatisticsController(MyWardrobeStatisticsDbContext context) : ControllerBase
    {
        private readonly MyWardrobeStatisticsDbContext _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WardrobeItemStatistics>>> Get()
        {
            var statistics = await _context.WardrobeItemsStatistics.ToListAsync();

            return Ok(statistics);
        }

        [HttpGet("latest-used-item")]
        public async Task<ActionResult<WardrobeItemStatistics>> GetLatestUsedItem()
        {
            var latestUsedItem = await _context.WardrobeItemsStatistics
                .OrderByDescending(x => x.LastTimeUsed)
                .Take(1)
                .ToListAsync();

            return Ok(latestUsedItem);
        }

        [HttpGet("most-used-items")]
        public async Task<ActionResult<WardrobeItemStatistics>> GetMostUsedItems()
        {
            var mostUsedItems = await _context.WardrobeItemsStatistics
                .OrderByDescending(x => x.WardrobeItemUsage)
                .Take(3)
                .ToListAsync();

            return Ok(mostUsedItems);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItem(Guid id)
        {
            var item = await _context.WardrobeItemsStatistics.FindAsync(id);

            _context.WardrobeItemsStatistics.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
