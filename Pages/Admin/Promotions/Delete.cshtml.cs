using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorCrudAppAuth.Data;
using RazorCrudAppAuth.Models;

namespace RazorCrudAppAuth.Pages.Admin.Promotions
{
    public class DeleteModel : PageModel
    {
        private readonly RazorCrudAppAuth.Data.ApplicationDbContext _context;

        public DeleteModel(RazorCrudAppAuth.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Promotion Promotion { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var promotion = await _context.Promotions.FirstOrDefaultAsync(m => m.Id == id);

            if (promotion == null)
            {
                return NotFound();
            }
            else
            {
                Promotion = promotion;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var promotion = await _context.Promotions.FindAsync(id);
            if (promotion != null)
            {
                Promotion = promotion;
                _context.Promotions.Remove(Promotion);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
