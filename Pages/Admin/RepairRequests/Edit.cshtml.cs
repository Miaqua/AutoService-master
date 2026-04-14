using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RazorCrudAppAuth.Data;
using RazorCrudAppAuth.Models;

namespace RazorCrudAppAuth.Pages.Admin.RepairRequests
{
    public class EditModel : PageModel
    {
        private readonly RazorCrudAppAuth.Data.ApplicationDbContext _context;

        public EditModel(RazorCrudAppAuth.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public RepairRequest RepairRequest { get; set; } = default!;


        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repairRequest = await _context.RepairRequests.FindAsync(id);

            if (repairRequest == null)
            {
                return NotFound();
            }

            RepairRequest = repairRequest;

            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Title");

                    ViewData["StatusList"] = new SelectList(new[]
                    {
                "Новая",
                "В обработке",
                "Выполнено",
                "Отменено"
            });

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName");

            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Title");
                ViewData["StatusList"] = new SelectList(new[]
                {
            "Новая",
            "В обработке",
            "Выполнено",
            "Отменено"
        });
                return Page();
            }


            _context.Attach(RepairRequest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RepairRequestExists(RepairRequest.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }


        private bool RepairRequestExists(int id)
        {
            return _context.RepairRequests.Any(e => e.Id == id);
        }
    }
}
