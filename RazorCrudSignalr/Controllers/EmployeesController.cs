using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RazorCrudSignalr.Models;

namespace RazorCrudSignalr.Controllers
{
    [Produces("application/json")]
    [Route("api/Employees")] // bỏ đi cũng được vì đã định nghĩa ở program.cs hoặc xài cài này cũng được [Route("api/[controller]")]
    public class EmployeesController : Controller
    {
        private readonly PRN221_Spr22Context _context;

        public EmployeesController(PRN221_Spr22Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            var res = await _context.Employees.ToListAsync();
            return _context.Employees != null ? Ok(res) : NotFound();
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = await _context.Employees.SingleOrDefaultAsync(m => m.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }
    }
}
