namespace Booksworm.Domain.ValueObjects;

/// <summary>
/// Value object representing a book rating from 1 to 5.
/// </summary>
public class Rating
{
    public int Value { get; }

    public Rating(int value)
    {
        if (value < 1 || value > 5)
            throw new ArgumentException("Rating must be between 1 and 5.", nameof(value));

        Value = value;
    }

    public override bool Equals(object? obj) => obj is Rating other && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value.ToString();
}