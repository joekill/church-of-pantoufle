using System.ComponentModel.DataAnnotations;

namespace ChurchOfPantoufle.Models;

public class Testimonial
{
    public int Id { get; set; }

    [Required(ErrorMessage = "The Warren requires your name, dear Kit.")]
    [StringLength(100)]
    public string AuthorName { get; set; } = string.Empty;

    [StringLength(100)]
    public string? WarrenName { get; set; }

    [Required(ErrorMessage = "Share your testimony with the Herd, blessed one.")]
    [StringLength(2000)]
    public string Content { get; set; } = string.Empty;

    public DateTime DateSubmitted { get; set; } = DateTime.UtcNow;

    public bool IsApproved { get; set; }
}
