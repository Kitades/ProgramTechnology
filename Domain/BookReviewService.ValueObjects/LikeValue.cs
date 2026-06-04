using BookReviewService.ValueObjects.Base;
using BookReviewService.ValueObjects.Validators;

namespace BookReviewService.ValueObjects;

public class LikeValue(int value) : ValueObject<int>(new LikeValueValidator(), value);