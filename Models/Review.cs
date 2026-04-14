using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using RazorCrudAppAuth.Models;
using System.ComponentModel.DataAnnotations;

public class Review
{
    public int Id { get; set; }
    public string Text { get; set; }

    [Range(1, 5, ErrorMessage = "Оценка должна быть в диапазоне от 1 до 5")]
    public int Rating { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsApproved { get; set; } = false;

    [ValidateNever]
    public string UserId { get; set; }

    [ValidateNever]
    public ApplicationUser User { get; set; }
}
