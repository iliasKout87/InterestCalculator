using InterestCalculator.Models.Common;
using InterestCalculator.Models.Types;

namespace InterestCalculator.Models.InterestCalculation;

public static class BankInterestRateExtensions
{
	public static InterestPeriod ToInterestPeriod(this BankInterestRate bankInterestRate, DatePeriod interestSchedulePeriod) =>
		new InterestPeriod(
			bankInterestRate.InterestRate,
			new DatePeriod(
				interestSchedulePeriod.StartDate.Max(bankInterestRate.Period.StartDate),
				interestSchedulePeriod.EndDate.Min(bankInterestRate.Period.EndDate)
			)
		);
}

