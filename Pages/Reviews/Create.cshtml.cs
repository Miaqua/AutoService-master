using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorCrudAppAuth.Data;
using RazorCrudAppAuth.Models;

namespace RazorCrudAppAuth.Pages.Reviews
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult OnGet()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public Review Review { get; set; } = default!;

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

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                ModelState.AddModelError(string.Empty, "Пользователь не найден.");
                return Page();
            }

            Review.UserId = currentUser.Id;
            Review.User = currentUser;
            Review.CreatedAt = DateTime.UtcNow;
            Review.Rating = Math.Clamp(Review.Rating, 1, 5);

            _context.Reviews.Add(Review);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
