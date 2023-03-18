using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Q2.Models;

namespace Q2.Controllers
{
    [Produces("application/json")]
    [Route("api/Movies")]
    public class MovieController : Controller
    {
        private readonly PE_PRN_Fall22B1Context _context;

        public MovieController(PE_PRN_Fall22B1Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetMovies()
        {
            var res = await _context.Movies.ToListAsync();
            return _context.Stars != null ? Ok(res) : NotFound();
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovie([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = await _context.Movies.SingleOrDefaultAsync(m => m.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }
    }
}
