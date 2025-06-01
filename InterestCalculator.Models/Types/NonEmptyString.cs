namespace InterestCalculator.Models.Types;

public record NonEmptyString(string Value)
{
	public string Value { get; init; } =
		!string.IsNullOrEmpty(Value)
		? Value.Trim()
		: throw new ArgumentException("Value cannot be null or empty", nameof(Value));

	public static implicit operator string(NonEmptyString value) => value.Value;
	public static implicit operator NonEmptyString(string value) => new(value);
}

