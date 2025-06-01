using InterestCalculator.Models.Types;

namespace InterestCalculator.Models.InterestCalculation;

public static class InterestPeriodScheduleExtensions
{
	public static InterestSchedule ToInterestSchedule(this InterestPeriodSchedule interestPeriodSchedule, Capital capital) =>
		new InterestSchedule(
			interestPeriodSchedule.InterestPeriods
				.Select(interestPeriod => interestPeriod.ToInterest(capital))
				.ToList()
		);
}

