namespace BookReviewService.Domain.Exceptions;

public class InvalidLikeValueException(int value, int min, int max)
    : ArgumentOutOfRangeException(nameof(value), value, $"Like value must be between {min} and {max}. Got {value}.");