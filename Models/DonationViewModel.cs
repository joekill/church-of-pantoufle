using System.ComponentModel.DataAnnotations;

namespace ChurchOfPantoufle.Models;

public class DonationViewModel
{
    [Required(ErrorMessage = "The Radiant One must know who tithes.")]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Select a sacred offering amount.")]
    [Range(1, 10000)]
    public decimal Amount { get; set; }

    [StringLength(500)]
    public string? PrayerIntention { get; set; }

    public string? SelectedTier { get; set; }
}
