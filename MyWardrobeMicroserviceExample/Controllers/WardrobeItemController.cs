using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWardrobe.Models;
using MyWardrobe.Persistence;

namespace MyWardrobe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WardrobeItemController(MyWardrobeDbContext context) : Controller
    {
        private readonly MyWardrobeDbContext _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WardrobeItem>>> GetWardrobeItem()
        {
            return await _context.WardrobeItems.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<WardrobeItem>> CreateWardrobeItem(WardrobeItem request)
        {
            var wardrobeItem = new WardrobeItem(
                Guid.NewGuid(),
                request.Category,
                request.Subcategory,
                request.WardrobeItemUsage,
                request.LastTimeUsed
                );

            _context.WardrobeItems.Add(wardrobeItem);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                "GetWardrobeItem",
                new { id = wardrobeItem.Id },
                wardrobeItem);
        }

        [HttpPut]
        public async Task<ActionResult<WardrobeItem>> UpdateWardrobeItem(Guid id)
        {
            var request = _context.WardrobeItems.AsNoTracking().FirstOrDefault(x => x.Id == id);

            request.WardrobeItemUsage++;

            var wardrobeItem = new WardrobeItem(
                id,
                request.Category,
                request.Subcategory,
                request.WardrobeItemUsage,
                request.LastTimeUsed
                );

            _context.WardrobeItems.Update(wardrobeItem);
            await _context.SaveChangesAsync();

            var newEvent = new Send();

            newEvent.PublishWardrobeItemUsedEvent(
                id,
                request.Category,
                request.Subcategory,
                request.WardrobeItemUsage,
                request.LastTimeUsed
                );

            return NoContent();
        }


    }   
}
