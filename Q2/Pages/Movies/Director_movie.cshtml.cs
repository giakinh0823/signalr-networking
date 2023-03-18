using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Q2.Hubs;
using Q2.Models;

namespace Q2.Pages.Movies
{
    public class Director_movieModel : PageModel
    {
		private readonly PE_PRN_Fall22B1Context _context;
        private readonly IHubContext<SignalRhub> _signalRHub;

        public List<Director> Directors;
		public List<Movie> Movies;


		[BindProperty(Name = "DirectorID", SupportsGet = true)]
		public int? DirectorID { get; set; }

		public Director_movieModel(PE_PRN_Fall22B1Context context, IHubContext<SignalRhub> signalRHub)
		{
			_context = context;
            _signalRHub = signalRHub;
        }


		public async Task OnGetAsync(int? deleteMovieId)
		{
			
			if(deleteMovieId!=null)
			{
				Movie? movie = await _context
					.Movies.Include(x => x.Stars)
					.Include(x => x.Genres)
					.Where(x => x.Id == deleteMovieId).FirstOrDefaultAsync();
				if(movie!=null)
				{
                    foreach (var item in movie.Stars.ToList())
                    {
                        movie.Stars.Remove(item);
                    }
                    foreach (var item in movie.Genres.ToList())
                    {
                        movie.Genres.Remove(item);
                    }

                    List<Star> stars = await _context.Stars.Include(x => x.Movies).ToListAsync();

                    foreach (var item in stars)
                    {
                        foreach (var i in item.Movies.ToList())
                        {
                            if (i.Id == deleteMovieId)
                            {
                                item.Movies.Remove(i);
                            }
                        }
                    }
                    List<Genre> genres = await _context.Genres.Include(x => x.Movies).ToListAsync();

                    foreach (var item in genres)
                    {
                        foreach (var i in item.Movies.ToList())
                        {
                            if (i.Id == deleteMovieId)
                            {
                                item.Movies.Remove(i);
                            }
                        }
                    }
                    _context.Movies.Remove(movie);
                    await _context.SaveChangesAsync();
                    await _signalRHub.Clients.All.SendAsync("LoadMovies");
                }

            }
            Directors = await _context.Directors.ToListAsync();

            if (DirectorID != null)
            {
                Movies = await _context.Movies.Include(x => x.Producer).Include(x => x.Stars).Where(x => x.Director.Id == DirectorID).ToListAsync();

            }
            else
            {
                Movies = await _context.Movies.Include(x => x.Producer).Include(x => x.Stars).ToListAsync();
            }

        }
    }
}
