using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Q2.Models;

namespace Q2.Controllers
{
    [Route("api/Movies")] 
    public class MoviesController : Controller
    {

        private readonly PE_PRN_Fall22B1Context _context;

        public MoviesController(PE_PRN_Fall22B1Context context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetMovies(int? directorId)
        {
            List<Movie>? movies = null;
            if (directorId != null)
            {
                movies = await _context.Movies.Include(x => x.Producer).Include(x => x.Stars).Where(x => x.Director.Id == directorId).ToListAsync();
            }
            else
            {
                movies = await _context.Movies.ToListAsync();
            }
            return movies != null ? Ok(movies) : NotFound();
        }
    }
}
