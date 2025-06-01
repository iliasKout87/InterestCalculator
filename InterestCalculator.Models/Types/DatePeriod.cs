namespace InterestCalculator.Models.Types;

public sealed record DatePeriod
{
	public DateOnly StartDate { get; init; }

	public DateOnly EndDate { get; init; }

	public int DurationInDays => EndDate.DayNumber - StartDate.DayNumber + 1;


	public DatePeriod(DateOnly startDate, DateOnly endDate)
	{
		if (StartDate > EndDate)
		{
			throw new ArgumentException("StartDate cannot be greater than EndDate");
		}

		StartDate = startDate;
		EndDate = endDate;
	}
}


