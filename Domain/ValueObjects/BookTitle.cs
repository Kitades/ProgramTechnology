namespace Booksworm.Domain.ValueObjects;

/// <summary>
/// Value object representing the title of a book.
/// </summary>
public class BookTitle
{
    public string Value { get; }

    public BookTitle(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Book title cannot be empty.", nameof(value));
        if (value.Length > 200)
            throw new ArgumentException("Book title cannot exceed 200 characters.", nameof(value));

        Value = value;
    }

    public override bool Equals(object? obj) => obj is BookTitle other && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;
}