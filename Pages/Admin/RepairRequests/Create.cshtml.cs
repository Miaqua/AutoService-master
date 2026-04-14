using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorCrudAppAuth.Data;
using RazorCrudAppAuth.Models;

namespace RazorCrudAppAuth.Pages.Admin.RepairRequests
{
    public class CreateModel : PageModel
    {
        private readonly RazorCrudAppAuth.Data.ApplicationDbContext _context;

        public CreateModel(RazorCrudAppAuth.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Id");
        ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public RepairRequest RepairRequest { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.RepairRequests.Add(RepairRequest);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
