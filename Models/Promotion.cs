namespace RazorCrudAppAuth.Models
{
    public class Promotion
    {
        public int Id { get; set; }
        public string Title { get; set; }            
        public string Description { get; set; }      
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive => DateTime.UtcNow >= StartDate && DateTime.UtcNow <= EndDate;
    }

}
