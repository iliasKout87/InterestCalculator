namespace InterestCalculator.Models.Common;

public static class DateOnlyExtensions
{
	public static DateOnly Min(this DateOnly a, DateOnly b) => a.CompareTo(b) < 0 ? a : b;

	public static DateOnly Max(this DateOnly a, DateOnly b) => a.CompareTo(b) > 0 ? a : b;
}

