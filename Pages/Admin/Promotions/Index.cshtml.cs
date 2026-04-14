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
    public class IndexModel : PageModel
    {
        private readonly RazorCrudAppAuth.Data.ApplicationDbContext _context;

        public IndexModel(RazorCrudAppAuth.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Promotion> Promotion { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Promotion = await _context.Promotions.ToListAsync();
        }
    }
}
