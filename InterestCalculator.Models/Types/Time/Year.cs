namespace InterestCalculator.Models.Types.Time;

public record struct Year(int Value)
{
    public bool IsLeapYear() => Value % 4 == 0 && (Value % 100 != 0 || Value % 400 == 0);
}
