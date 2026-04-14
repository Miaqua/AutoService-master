using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorCrudAppAuth.Data;
using RazorCrudAppAuth.Models;
using System.Linq;

namespace RazorCrudAppAuth.Pages.RepairRequests
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public RepairRequest RepairRequest { get; set; } = new RepairRequest();

        public IActionResult OnGet()
        {
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Title");

            if (User.Identity?.IsAuthenticated == true)
            {
                var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                if (user != null)
                {
                    RepairRequest.UserId = user.Id;
                    RepairRequest.User = user;
                    RepairRequest.CustomerName = user.FullName ?? string.Empty;
                    RepairRequest.PhoneNumber = user.PhoneNumber ?? string.Empty;
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                foreach (var error in errors)
                {
                    Console.WriteLine("Model Error: " + error);
                }
                return Page();
            }

            if (User.Identity?.IsAuthenticated == true)
            {
                var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                if (user != null)
                {
                    RepairRequest.UserId = user.Id;
                    RepairRequest.User = user;
                }
            }

            if (string.IsNullOrWhiteSpace(RepairRequest.Status))
            {
                RepairRequest.Status = "Новая";
            }

            _context.RepairRequests.Add(RepairRequest);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
