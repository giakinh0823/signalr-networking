using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Q2.Hubs;
using Q2.Models;

namespace Q2.Pages.Stars
{
    public class UpdateModel : PageModel
    {
        private readonly PE_PRN_Fall22B1Context _context;
        private readonly IHubContext<SignalRhub> _signalRHub;
        public Star? star;

        public UpdateModel(PE_PRN_Fall22B1Context context, IHubContext<SignalRhub> signalRHub)
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
            star = await _context.Stars.Where(x => x.Id == StarID).FirstOrDefaultAsync();
            if (star == null)
            {
                return NotFound();
            }
            return Page();
        }


        [BindProperty]
        public Star StarRequest { get; set; } = default!;


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.Stars == null || StarRequest == null || StarID == null)
            {
                return Page();
            }
            StarRequest.Id = StarID.Value;
            _context.Stars.Update(StarRequest);
            await _context.SaveChangesAsync();
            await _signalRHub.Clients.All.SendAsync("LoadStars");
            return RedirectToPage("./Stars");
        }
    }
}
