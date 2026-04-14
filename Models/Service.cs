using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace RazorCrudAppAuth.Models
{
    public class Service
    {
        public int Id { get; set; }
        public string Title { get; set; }          
        public string Description { get; set; }       
        public decimal Price { get; set; }

        [ValidateNever]
        public ICollection<RepairRequest> RepairRequests { get; set; }
    }

}
