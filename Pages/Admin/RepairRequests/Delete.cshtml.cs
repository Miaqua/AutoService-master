using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorCrudAppAuth.Data;
using RazorCrudAppAuth.Models;

namespace RazorCrudAppAuth.Pages.Admin.RepairRequests
{
    public class DeleteModel : PageModel
    {
        private readonly RazorCrudAppAuth.Data.ApplicationDbContext _context;

        public DeleteModel(RazorCrudAppAuth.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public RepairRequest RepairRequest { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repairrequest = await _context.RepairRequests.FirstOrDefaultAsync(m => m.Id == id);

            if (repairrequest == null)
            {
                return NotFound();
            }
            else
            {
                RepairRequest = repairrequest;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repairrequest = await _context.RepairRequests.FindAsync(id);
            if (repairrequest != null)
            {
                RepairRequest = repairrequest;
                _context.RepairRequests.Remove(RepairRequest);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
