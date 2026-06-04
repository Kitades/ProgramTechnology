using BookReviewService.Domain.Repositories.Abstractions.Base;

namespace BookReviewService.Domain.Repositories.Abstractions;

public interface IReviewBookRepository : IRepository<ReviewBook, Guid>
{
    Task<IEnumerable<ReviewBook>> GetByBookIdAsync(Guid bookId, CancellationToken cancellationToken);
    Task<IEnumerable<ReviewBook>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
}