using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Q2.Hubs;
using Q2.Models;

namespace Q2.Pages.Stars
{
    public class DeleteModel : PageModel
    {
        private readonly PE_PRN_Fall22B1Context _context;
        private readonly IHubContext<SignalRhub> _signalRHub;

		public DeleteModel(PE_PRN_Fall22B1Context context, IHubContext<SignalRhub> signalRHub)
		{
			_context = context;
			_signalRHub = signalRHub;
		}

		[BindProperty(Name = "StarID", SupportsGet = true)]
		public int? StarID { get; set; }
		public async Task<IActionResult> OnGetAsync()
        {
            if (StarID == null || _context.Stars == null)
            {
                return NotFound();
            }
            var star = await _context.Stars.FindAsync(StarID);

            if (star != null)
            {
                Star StarDelete = _context.Stars.Where(x => x.Id == StarID).FirstOrDefault();
                if (StarDelete != null)
                {

					_context.Stars.Remove(StarDelete);
					await _context.SaveChangesAsync();
					await _signalRHub.Clients.All.SendAsync("LoadStars");
				}
			}

			return RedirectToPage("./Stars");
		}
    }
}
