using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ChatAppRazor.Models;
using ChatAppRazor.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace ChatAppRazor.Pages.Employees
{
    public class CreateModel : PageModel
    {
        private readonly ChatAppRazor.Models.PRN221_Spr22Context _context;
        private readonly IHubContext<SignalrServer> _signalRHub;

        public CreateModel(ChatAppRazor.Models.PRN221_Spr22Context context, IHubContext<SignalrServer> signalRHub)
        {
            _context = context;
            _signalRHub = signalRHub;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Employee Employee { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Employees == null || Employee == null)
            {
                return Page();
            }

            _context.Employees.Add(Employee);
            await _context.SaveChangesAsync();
            await _signalRHub.Clients.All.SendAsync("LoadEmployees");
            return RedirectToPage("./Index");
        }
    }
}
