
using InterestCalculator.Models.Types;

namespace InterestCalculator.Models.InterestCalculation;

public static class InterestScheduleGeneration
{
	public static InterestSchedule GenerateInterestSchedule(BankInterestRateSchedule bankInterestRateSchedule, DatePeriod datePeriod, Capital capital) =>
		bankInterestRateSchedule.ToInterestPeriodSchedule(datePeriod).ToInterestSchedule(capital);


}
