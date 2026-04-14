using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RazorCrudAppAuth.Data;
using RazorCrudAppAuth.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RazorCrudAppAuth.Pages.RepairRequests
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public RepairRequest RepairRequest { get; set; } = default!;

        public bool AccessDenied { get; set; } = false;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var repairRequest = await _context.RepairRequests.FirstOrDefaultAsync(r => r.Id == id);

            if (repairRequest == null)
                return NotFound();

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (repairRequest.UserId != currentUserId)
            {
                AccessDenied = true;
                return Page();
            }

            RepairRequest = repairRequest;

            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Title");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var repairRequestInDb = await _context.RepairRequests.AsNoTracking().FirstOrDefaultAsync(r => r.Id == RepairRequest.Id);

            if (repairRequestInDb == null)
                return NotFound();

            if (repairRequestInDb.UserId != currentUserId)
                return Forbid();

            RepairRequest.UserId = currentUserId;

            _context.Attach(RepairRequest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RepairRequestExists(RepairRequest.Id))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToPage("./Index");
        }

        private bool RepairRequestExists(int id)
        {
            return _context.RepairRequests.Any(e => e.Id == id);
        }
    }
}
