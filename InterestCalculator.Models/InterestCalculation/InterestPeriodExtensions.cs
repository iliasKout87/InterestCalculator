using InterestCalculator.Models.Types;
using InterestCalculator.Models.Types.Time;

namespace InterestCalculator.Models.InterestCalculation;

public static class InterestPeriodExtensions
{
	public static Interest ToInterest(this InterestPeriod interestPeriod, Capital capital)
	{
		if (interestPeriod.Period.StartDate.Year == interestPeriod.Period.EndDate.Year)
		{
			var year = new Year(interestPeriod.Period.StartDate.Year);
			var daysInYear = year.IsLeapYear() ? 366 : 365;

			return new Interest(
				new InterestAmount(new Money(interestPeriod.InterestRate.Rate * capital.Money.Amount * interestPeriod.Period.DurationInDays / daysInYear, capital.Money.Currency)),
				interestPeriod
			);
		}

		var years = Enumerable.Range(
			interestPeriod.Period.StartDate.Year,
			interestPeriod.Period.EndDate.Year - interestPeriod.Period.StartDate.Year + 1
		);

		var yearlyPeriods = years.Select(year =>
		{
			var startDate = year == interestPeriod.Period.StartDate.Year
				? interestPeriod.Period.StartDate
				: new DateOnly(year, 1, 1);

			var endDate = year == interestPeriod.Period.EndDate.Year
				? interestPeriod.Period.EndDate
				: new DateOnly(year, 12, 31);

			return new InterestPeriod(
				interestPeriod.InterestRate,
				new DatePeriod(startDate, endDate)
			);
		});

		var yearlyInterests = yearlyPeriods.Select(period => period.ToInterest(capital));

		var totalInterestAmount = new InterestAmount(
			new Money(
				yearlyInterests.Sum(interest => interest.Amount.Money.Amount),
				capital.Money.Currency
			)
		);

		return new Interest(totalInterestAmount, interestPeriod);
	}
}

