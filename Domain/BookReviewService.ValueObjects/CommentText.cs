using BookReviewService.ValueObjects.Base;
using BookReviewService.ValueObjects.Validators;

namespace BookReviewService.ValueObjects;

public class CommentText(string comment) : ValueObject<string>(new CommentTextValidator(), comment);