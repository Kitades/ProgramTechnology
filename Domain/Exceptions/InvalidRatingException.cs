namespace Booksworm.Domain.Exceptions;

public class InvalidRatingException : ArgumentOutOfRangeException
{
    public int ProvidedValue { get; }

    public InvalidRatingException(int value)
        : base(nameof(value), value, "Rating must be between 1 and 5.")
    {
        ProvidedValue = value;
    }
}