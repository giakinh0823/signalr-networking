using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Q2.Hubs;
using Q2.Models;

namespace Q2.Pages.Stars
{
    public class AddModel : PageModel
    {
        private readonly PE_PRN_Fall22B1Context _context;
        private readonly IHubContext<SignalRhub> _signalRHub;
        public List<Star> stars;

        public AddModel(PE_PRN_Fall22B1Context context, IHubContext<SignalRhub> signalRHub)
        {
            _context = context;
            _signalRHub = signalRHub;
        }

        public IActionResult OnGet()
        {
            return Page();
        }


        [BindProperty]
        public Star Star { get; set; } = default!;

        
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.Stars == null || Star == null)
            {
                return Page();
            }

            _context.Stars.Add(Star);
            await _context.SaveChangesAsync();
            await _signalRHub.Clients.All.SendAsync("LoadStars");
            return RedirectToPage("./Stars");
        }
    }
}
