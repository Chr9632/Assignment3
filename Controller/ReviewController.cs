
using System.Security.Claims;
using assignment_3_movievault.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("[controller]")]

public class ReviewController : ControllerBase
{
    private readonly Reviewservice _reviewservice;
    private readonly MovieContext _context;

    public ReviewController(Reviewservice reviewservice, MovieContext context)
    {
        _reviewservice = reviewservice;
        _context = context;
    }

    private async Task<List<int>?> GetSpecificReviewAsync(CancellationToken cancellationToken)
    {
        if (User.IsInRole("Admin"))
        {
            return null;
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return new List<int>();
        }

        return await _context.Reviews
            .Where(r => r.Reviewer.Identity_user_id == userId)
            .Select(r => r.Review_id)
            .ToListAsync(cancellationToken);
    }

    private async Task<int?> GetReviewerIdAsync(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return null;
        }

        return await _context.Reviewers
            .Where(r => r.Identity_user_id == userId)
            .Select(r => (int?)r.Reviewer_id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    private async Task<IActionResult?> CheckReviewAccessAsync(int id, CancellationToken cancellationToken)
    {
        var reviewExists = await _context.Reviews.AnyAsync(r => r.Review_id == id, cancellationToken);
        if (!reviewExists)
        {
            return NotFound("Review not found");
        }

        var reviewIds = await GetSpecificReviewAsync(cancellationToken);
        if (reviewIds != null && !reviewIds.Contains(id))
        {
            return Forbid();
        }

        return null;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetReviewsAllAsync(CancellationToken cancellationToken)
    {
        var reviews = await _reviewservice.GetAllReviewsAsync(cancellationToken);
        return Ok(reviews);
    }

    [AllowAnonymous]
    [HttpGet("{id:int}", Name = "GetReviewById")]
    public async Task<IActionResult> GetReviewAsync(int id, CancellationToken cancellationToken)
    {
        var review = await _reviewservice.GetReviewsByIdAsync(id, cancellationToken);
        if(review == null)
        {
            return NotFound("Review not found");
        }

        return Ok(review);
    }

    [AllowAnonymous]
    [HttpGet("/movies/{movieId:int}/reviews")]
    public async Task<IActionResult> GetReviewsForMovieAsync(int movieId, CancellationToken cancellationToken)
    {
        var reviews = await _reviewservice.GetReviewsByIdAsync(movieId, cancellationToken);
        return Ok(reviews);
    }

    [AllowAnonymous]
    [HttpGet("/media-outlets/{mediaOutletId:int}/reviews")]
    public async Task<IActionResult> GetReviewsByMediaOutletAsync(int mediaOutletId, CancellationToken cancellationToken)
    {
        var reviews = await _reviewservice.GetReviewsByMediaOutletAsync(mediaOutletId, cancellationToken);
        return Ok(reviews);
    }

    [Authorize(Roles = "Admin, Reviewer")]
    [HttpPost]
    public async Task<IActionResult> AddReviewsAsync(CreateReviewDto dto, CancellationToken cancellationToken)
    {
        int reviewerId;
        if (User.IsInRole("Admin"))
        {
            if (!dto.Reviewer_id.HasValue)
            {
                return BadRequest("An Admin must provide the reviewer for the review.");
            }

            reviewerId = dto.Reviewer_id.Value;
        }
        else
        {
            if (dto.Reviewer_id.HasValue)
            {
                return BadRequest("Reviewers cannot select another reviewer.");
            }

            var currentReviewerId = await GetReviewerIdAsync(cancellationToken);
            if (!currentReviewerId.HasValue)
            {
                return Forbid();
            }

            reviewerId = currentReviewerId.Value;
        }

        var reviewerExists = await _context.Reviewers.AnyAsync(
            r => r.Reviewer_id == reviewerId,
            cancellationToken);
        if (!reviewerExists)
        {
            return BadRequest("Reviewer not found.");
        }

        var addReview = await _reviewservice.AddReviewAsync(dto, reviewerId, cancellationToken);
        return CreatedAtRoute("GetReviewById", new { id = addReview.Review_id }, addReview);
    }

    [Authorize(Roles = "Admin, Reviewer")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateReviewsAsync(int id, UpdateReviewDto dto, CancellationToken cancellationToken)
    {
        var accessResult = await CheckReviewAccessAsync(id, cancellationToken);
        if (accessResult != null)
        {
            return accessResult;
        }

        var review = await _reviewservice.UpdateReviewAsync(id, dto, cancellationToken);

        if(review == null)
        {
            return NotFound("Review not found");
        }

        return Ok(review);
    }

    [Authorize(Roles = "Admin, Reviewer")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveReviewAsync(int id, CancellationToken cancellationToken)
    {
        var accessResult = await CheckReviewAccessAsync(id, cancellationToken);
        if (accessResult != null)
        {
            return accessResult;
        }

        var review = await _reviewservice.DeleteReviewAsync(id, cancellationToken);

        if(!review)
        {
            return NotFound("Review not found");
        }

        return NoContent();
    }

}