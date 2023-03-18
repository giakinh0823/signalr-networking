﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorCrudSignalr.Models;

namespace RazorCrudSignalr.Pages.Employees
{
    public class IndexModel : PageModel
    {
        private readonly RazorCrudSignalr.Models.PRN221_Spr22Context _context;

        public IndexModel(RazorCrudSignalr.Models.PRN221_Spr22Context context)
        {
            _context = context;
        }

        public IList<Employee> Employee { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Employees != null)
            {
                Employee = await _context.Employees.ToListAsync();
            }
        }
    }
}
