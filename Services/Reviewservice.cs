using assignment_3_movievault.DTO;
using Microsoft.EntityFrameworkCore;


public class Reviewservice : IReviewservice
{
    private readonly MovieContext _context;
    private readonly IRepository<Review> _repository;

    public Reviewservice(MovieContext context, IRepository<Review> repository)
    {
        _context = context;
        _repository = repository;
    }

    public async Task<IEnumerable<ReviewResponseDto>> GetAllReviewsAsync(CancellationToken cancellationToken)
    {
        return await _context.Set<Review>()
        .AsNoTracking()
        .Select(r => new ReviewResponseDto
        {
            Review_id = r.Review_id,
            Headline = r.Headline, 
            Review_text = r.Review_text,
            Rating = r.Rating,
            Published_at = r.Published_at,
            Movie_id = r.Movie_id,
            Reviewer = new ReviewerResponseDto
            {
                Reviewer_id = r.Reviewer.Reviewer_id,
                Name = r.Reviewer.Name,
                Media_outlet = new MediaOutletResponseDto
                {
                    Media_outlet_id = r.Reviewer.Media_outlet.Media_outlet_id,
                    Name = r.Reviewer.Media_outlet.Name,
                    Outlet_type = r.Reviewer.Media_outlet.Outlet_type,
                    Website = r.Reviewer.Media_outlet.Website
                }
            }
        }).ToListAsync(cancellationToken);
    }
    public async Task<IEnumerable<ReviewResponseDto>> GetReviewsByIdAsync(int movieId, CancellationToken cancellationToken)
    {
        var reviews = await _context.Set<Review>().AsNoTracking()
        .Where(r => r.Movie_id == movieId)
        .Select(r => new ReviewResponseDto
        {
            Review_id = r.Review_id,
            Headline = r.Headline, 
            Review_text = r.Review_text,
            Rating = r.Rating,
            Published_at = r.Published_at,
            Movie_id = r.Movie_id,
            Reviewer = new ReviewerResponseDto
            {
                Reviewer_id = r.Reviewer.Reviewer_id,
                Name = r.Reviewer.Name,
                Media_outlet = new MediaOutletResponseDto
                {
                    Media_outlet_id = r.Reviewer.Media_outlet.Media_outlet_id,
                    Name = r.Reviewer.Media_outlet.Name,
                    Outlet_type = r.Reviewer.Media_outlet.Outlet_type,
                    Website = r.Reviewer.Media_outlet.Website
                }
            }
        }).ToListAsync(cancellationToken);

        if (reviews.Count == 0)
        {
            return [];
        }

        return reviews;
    }

    public async Task<IEnumerable<ReviewResponseDto>> GetReviewsByMediaOutletAsync(int mediaOutletId, CancellationToken cancellationToken)
    {
        var reviews = await _context.Set<Review>()
        .AsNoTracking()
        .Where(r => r.Reviewer.Media_outlet_id == mediaOutletId)
        .Select(r => new ReviewResponseDto
        {
            Review_id = r.Review_id,
            Headline = r.Headline, 
            Review_text = r.Review_text,
            Rating = r.Rating,
            Published_at = r.Published_at,
            Movie_id = r.Movie_id,
            Reviewer = new ReviewerResponseDto
            {
                Reviewer_id = r.Reviewer.Reviewer_id,
                Name = r.Reviewer.Name,
                Media_outlet = new MediaOutletResponseDto
                {
                    Media_outlet_id = r.Reviewer.Media_outlet.Media_outlet_id,
                    Name = r.Reviewer.Media_outlet.Name,
                    Outlet_type = r.Reviewer.Media_outlet.Outlet_type,
                    Website = r.Reviewer.Media_outlet.Website
                }
            }
        }).ToListAsync(cancellationToken);

        if (reviews.Count == 0)
        {
            return [];
        }

        return reviews;
    }

    public async Task<ReviewResponseDto> AddReviewAsync(CreateReviewDto dto, int reviewerId, CancellationToken cancellationToken)
    {
        var reviewer = await _context.Reviewers
            .AsNoTracking()
            .Where(r => r.Reviewer_id == reviewerId)
            .Select(r => new ReviewerResponseDto
            {
                Reviewer_id = r.Reviewer_id,
                Name = r.Name,
                Media_outlet = new MediaOutletResponseDto
                {
                    Media_outlet_id = r.Media_outlet.Media_outlet_id,
                    Name = r.Media_outlet.Name,
                    Outlet_type = r.Media_outlet.Outlet_type,
                    Website = r.Media_outlet.Website
                }
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (reviewer == null)
        {
            throw new InvalidOperationException($"Reviewer {reviewerId} does not exist.");
        }

        var addReview = new Review
        {
            Headline = dto.Headline,
            Review_text = dto.Review_text,
            Rating = dto.Rating,
            Movie_id = dto.Movie_id,
            Reviewer_id = reviewerId,
            Published_at = DateTime.UtcNow
        };

        await _repository.AddAsync(addReview);
        await _context.SaveChangesAsync(cancellationToken);

        return new ReviewResponseDto
        {
            Review_id = addReview.Review_id,
            Headline = addReview.Headline,
            Review_text = addReview.Review_text,
            Rating = addReview.Rating,
            Published_at = addReview.Published_at,
            Movie_id = addReview.Movie_id,
            Reviewer = reviewer
        };
    }
    public async Task<ReviewResponseDto?> UpdateReviewAsync(int id, UpdateReviewDto dto, CancellationToken cancellationToken)
    {
        var review = await _context.Reviews
            .Include(r => r.Reviewer)
            .ThenInclude(r => r.Media_outlet)
            .FirstOrDefaultAsync(r => r.Review_id == id, cancellationToken);

        if(review == null)
        {
            return null;
        }

        review.Headline = dto.Headline;
        review.Review_text = dto.Review_text;
        review.Rating = dto.Rating;
        review.Movie_id = dto.Movie_id;

        await _context.SaveChangesAsync(cancellationToken);

        return new ReviewResponseDto
        {
            Review_id = review.Review_id,
            Headline = review.Headline, 
            Review_text = review.Review_text,
            Rating = review.Rating,
            Published_at = review.Published_at,
            Movie_id = review.Movie_id,
            Reviewer = new ReviewerResponseDto
            {
                Reviewer_id = review.Reviewer.Reviewer_id,
                Name = review.Reviewer.Name,
                Media_outlet = new MediaOutletResponseDto
                {
                    Media_outlet_id = review.Reviewer.Media_outlet.Media_outlet_id,
                    Name = review.Reviewer.Media_outlet.Name,
                    Outlet_type = review.Reviewer.Media_outlet.Outlet_type,
                    Website = review.Reviewer.Media_outlet.Website
                }
            }
        };
    }
    public async Task<bool> DeleteReviewAsync(int id, CancellationToken cancellationToken)
    {
        var review = await _repository.GetByIdAsync(id, cancellationToken);
        if(review == null)
        {
            return false;
        }

        _repository.Delete(review);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}