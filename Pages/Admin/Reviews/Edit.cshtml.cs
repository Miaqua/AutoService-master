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

namespace RazorCrudAppAuth.Pages.Admin.Reviews
{
    public class EditModel : PageModel
    {
        private readonly RazorCrudAppAuth.Data.ApplicationDbContext _context;

        public EditModel(RazorCrudAppAuth.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Review Review { get; set; } = default!;

        public SelectList? UsersSelectList { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews.FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }
            Review = review;
            UsersSelectList = new SelectList(_context.Users, "Id", "Id");
            ViewData["UserId"] = UsersSelectList;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string? action)
        {
            if (!ModelState.IsValid)
            {
                UsersSelectList = new SelectList(_context.Users, "Id", "Id");
                ViewData["UserId"] = UsersSelectList;
                return Page();
            }

            var reviewToUpdate = await _context.Reviews.FindAsync(Review.Id);
            if (reviewToUpdate == null)
            {
                return NotFound();
            }

            reviewToUpdate.Text = Review.Text;
            reviewToUpdate.Rating = Review.Rating;
            reviewToUpdate.CreatedAt = Review.CreatedAt;
            reviewToUpdate.UserId = Review.UserId;

            if (action == "save")
            {
                reviewToUpdate.IsApproved = Review.IsApproved;
            }
            else if (action == "approve")
            {
                reviewToUpdate.IsApproved = true;
            }
            else if (action == "reject")
            {
                reviewToUpdate.IsApproved = false;
            }
            else
            {
                reviewToUpdate.IsApproved = Review.IsApproved;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(reviewToUpdate.Id))
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

        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }
    }
}
