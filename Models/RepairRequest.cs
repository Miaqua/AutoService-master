using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace RazorCrudAppAuth.Models
{
    public class RepairRequest
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime PreferredDate { get; set; }
        public string Status { get; set; } = "Новая"; 

        public int ServiceId { get; set; }

        [ValidateNever]
        public Service Service { get; set; }

        [ValidateNever]
        public string UserId { get; set; }

        [ValidateNever]
        public ApplicationUser User { get; set; }
    }
}
