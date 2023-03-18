using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Q2.Hubs;
using Q2.Models;

namespace Q2.Pages.Stars
{
    public class StarModel : PageModel
    {
        private readonly PE_PRN_Fall22B1Context _context;
        private readonly IHubContext<SignalRhub> _signalRHub;
        public List<Star> stars;

        public StarModel(PE_PRN_Fall22B1Context context, IHubContext<SignalRhub> signalRHub)
        {
            _context = context;
            _signalRHub = signalRHub;
        }

        public async Task OnGetAsync(int? starID)
        {
            if (starID != null)
            {
                stars = await _context.Stars.Where(x => x.Id == starID).ToListAsync();
            }
            else stars = await _context.Stars.ToListAsync();
       
        }
    }
}