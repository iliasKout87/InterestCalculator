namespace InterestCalculator.Models.Types;

public sealed record BankInterestRateSchedule
{
    public IReadOnlyList<BankInterestRate> BankInterestRates { get; init; }

    private BankInterestRateSchedule(IReadOnlyList<BankInterestRate> bankInterestRates)
    {
        for (int i = 1; i < bankInterestRates.Count; i++)
        {
            if (bankInterestRates[i - 1].Period.EndDate.AddDays(1) != bankInterestRates[i].Period.StartDate)
                throw new ArgumentException("Interest rate periods must be consecutive.");
        }

        BankInterestRates = bankInterestRates;
    }

	public static BankInterestRateSchedule Create(IEnumerable<BankInterestRate> bankInterestRates)
    {
        var sorted = bankInterestRates
            .OrderBy(p => p.Period.StartDate)
            .ToList();

        return new BankInterestRateSchedule(sorted);
    }
}




