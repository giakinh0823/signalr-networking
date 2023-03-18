using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Q2.Models;

namespace Q2.Controllers
{
    [Produces("application/json")]
    [Route("api/Stars")]
    public class StarController : Controller
    {
        private readonly PE_PRN_Fall22B1Context _context;

        public StarController(PE_PRN_Fall22B1Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetStars()
        {
            var res = await _context.Stars.ToListAsync();
            return _context.Stars != null ? Ok(res) : NotFound();
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetStar([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = await _context.Stars.SingleOrDefaultAsync(m => m.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }
    }
}
