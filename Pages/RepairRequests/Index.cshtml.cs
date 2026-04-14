using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorCrudAppAuth.Data;
using RazorCrudAppAuth.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace RazorCrudAppAuth.Pages.RepairRequests
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<RepairRequest> RepairRequest { get; set; } = new List<RepairRequest>();

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                RepairRequest = new List<RepairRequest>();
                return;
            }

            RepairRequest = await _context.RepairRequests
                .Include(r => r.Service)
                .Include(r => r.User)
                .Where(r => r.UserId == user.Id)
                .ToListAsync();
        }
    }
}
