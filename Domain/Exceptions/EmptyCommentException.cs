namespace Booksworm.Domain.Exceptions;

public class EmptyCommentException : ArgumentException
{
    public EmptyCommentException()
        : base("Comment cannot be empty.")
    {
    }
}