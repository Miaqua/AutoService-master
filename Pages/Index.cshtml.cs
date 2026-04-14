using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RazorCrudAppAuth.Data;
using RazorCrudAppAuth.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RazorCrudAppAuth.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext _context;

        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public List<Service> Services { get; set; } = new();
        public int TotalReviews { get; set; }
        public int TotalServices { get; set; }
        public int ActivePromotions { get; set; }

        public async Task OnGetAsync()
        {
            Services = await _context.Services
                .OrderBy(s => s.Id)
                .Take(6)
                .ToListAsync();

            TotalServices = await _context.Services.CountAsync();

            TotalReviews = 247;
            ActivePromotions = 3;
        }

        public string GetServiceIcon(string serviceTitle)
        {
            if (string.IsNullOrWhiteSpace(serviceTitle))
                return "🔧";

            var icons = new Dictionary<string, string>
            {
                ["диагностика"] = "🔍",
                ["двигатель"] = "🔧",
                ["трансмиссия"] = "⚙️",
                ["тормоз"] = "🛑",
                ["подвеска"] = "🔄",
                ["электрика"] = "⚡",
                ["кондиционер"] = "❄️",
                ["шины"] = "🛞",
                ["масло"] = "🛢️",
                ["кузов"] = "🚗",
                ["стекло"] = "🪟",
                ["выхлоп"] = "💨",
                ["техническое"] = "🔧",
                ["ремонт"] = "🔧"
            };

            var lowerTitle = serviceTitle.ToLower();

            foreach (var icon in icons)
            {
                if (lowerTitle.Contains(icon.Key))
                    return icon.Value;
            }

            return "🔧";
        }
    }
}
