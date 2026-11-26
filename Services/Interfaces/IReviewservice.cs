

using assignment_3_movievault.DTO;

public interface IReviewservice
{
    public Task<IEnumerable<ReviewResponseDto>> GetAllReviewsAsync(CancellationToken cancellationToken);
    public Task<IEnumerable<ReviewResponseDto>> GetReviewsByIdAsync(int movieId, CancellationToken cancellationToken);
    public Task<IEnumerable<ReviewResponseDto>> GetReviewsByMediaOutletAsync(int mediaOutletId, CancellationToken cancellationToken);

    public Task<ReviewResponseDto> AddReviewAsync(CreateReviewDto dto, int reviewerId, CancellationToken cancellationToken);
    public Task<ReviewResponseDto?> UpdateReviewAsync(int id, UpdateReviewDto dto, CancellationToken cancellationToken);
    public Task<bool> DeleteReviewAsync(int id, CancellationToken cancellationToken);
}