using ChurchOfPantoufle.Models;

namespace ChurchOfPantoufle.Services;

public interface IDonationService
{
    Task<bool> ProcessDonationAsync(DonationViewModel donation);
}

public class MockDonationService : IDonationService
{
    public Task<bool> ProcessDonationAsync(DonationViewModel donation)
    {
        // Mock implementation — always succeeds.
        // Replace with Stripe/PayPal integration when ready.
        return Task.FromResult(true);
    }
}
