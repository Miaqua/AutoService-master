using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorCrudAppAuth.Data;
using RazorCrudAppAuth.Models;
using System.ComponentModel.DataAnnotations;

namespace RazorCrudAppAuth.Pages.Account
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ProfileModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public class UserProfileViewModel
        {
            [Required]
            public string FullName { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Phone]
            public string PhoneNumber { get; set; }
        }

        [BindProperty]
        public UserProfileViewModel Input { get; set; }

        public List<RepairRequest> UserRepairRequests { get; set; } = new();
        public List<Review> UserReviews { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound("Пользователь не найден.");

            await LoadUserData(user);
            return Page();
        }

        private async Task LoadUserData(ApplicationUser user)
        {
            Input = new UserProfileViewModel
            {
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

            UserRepairRequests = await _context.RepairRequests
                .Include(r => r.Service)
                .Where(r => r.UserId == user.Id)
                .OrderByDescending(r => r.PreferredDate)
                .ToListAsync();

            UserReviews = await _context.Reviews
                .Where(r => r.UserId == user.Id)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public int GetCompletedRequests() =>
            UserRepairRequests.Count(r => r.Status?.ToLower() == "завершено");

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                await LoadUserData(user);
                return Page();
            }

            var userToUpdate = await _userManager.GetUserAsync(User);
            if (userToUpdate == null) return NotFound("Пользователь не найден.");

            userToUpdate.FullName = Input.FullName;
            userToUpdate.PhoneNumber = Input.PhoneNumber;

            var result = await _userManager.UpdateAsync(userToUpdate);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Данные успешно обновлены";
                return RedirectToPage();
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            await LoadUserData(userToUpdate);
            return Page();
        }
    }
}
