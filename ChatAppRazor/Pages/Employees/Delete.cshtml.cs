using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ChatAppRazor.Models;
using ChatAppRazor.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace ChatAppRazor.Pages.Employees
{
    public class DeleteModel : PageModel
    {
        private readonly ChatAppRazor.Models.PRN221_Spr22Context _context;
        private readonly IHubContext<SignalrServer> _signalRHub;

        public DeleteModel(ChatAppRazor.Models.PRN221_Spr22Context context, IHubContext<SignalrServer> signalRHub)
        {
            _context = context;
            _signalRHub = signalRHub;
        }

        [BindProperty]
      public Employee Employee { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FirstOrDefaultAsync(m => m.Id == id);

            if (employee == null)
            {
                return NotFound();
            }
            else 
            {
                Employee = employee;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }
            var employee = await _context.Employees.FindAsync(id);

            if (employee != null)
            {
                Employee = employee;
                _context.Employees.Remove(Employee);
                await _context.SaveChangesAsync();
                await _signalRHub.Clients.All.SendAsync("LoadEmployees");
            }

            return RedirectToPage("./Index");
        }
    }
}
