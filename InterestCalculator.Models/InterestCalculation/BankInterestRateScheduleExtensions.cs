using InterestCalculator.Models.Types;

namespace InterestCalculator.Models.InterestCalculation;

public static class BankInterestRateScheduleExtensions
{
	public static InterestSchedule ToInterestSchedule(this BankInterestRateSchedule bankInterestRateSchedule, DatePeriod datePeriod, Capital capital) =>
		bankInterestRateSchedule.ToInterestPeriodSchedule(datePeriod).ToInterestSchedule(capital);

	public static InterestPeriodSchedule ToInterestPeriodSchedule(this BankInterestRateSchedule bankInterestRateSchedule, DatePeriod datePeriod) =>
		new InterestPeriodSchedule(
			bankInterestRateSchedule.BankInterestRates
				.SkipWhile(bankInterestRate => bankInterestRate.Period.EndDate < datePeriod.StartDate)
				.TakeWhile(bankInterestRate => bankInterestRate.Period.StartDate <= datePeriod.EndDate)
				.Select(bankInterestRate => bankInterestRate.ToInterestPeriod(datePeriod))
				.ToList()
		);
}

