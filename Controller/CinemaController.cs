using assignment_3_movievault.DTO;
using assignment_3_movievault.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class CinemaController : ControllerBase
{
    private readonly CinemaRevenue _cinemarevenue;

    public CinemaController(CinemaRevenue cinemarevenue)
    {
        _cinemarevenue = cinemarevenue;
    }

    [Authorize(Roles = "Actor, Director, Reviewer, Admin")]
    [HttpGet("/cinemas/revenue")]
    public async Task<IActionResult> GetCinemaRevenue(CancellationToken cancellationToken)
    {
        var cinemaRevenue = await _cinemarevenue.GetCinemaRevenueAsync(cancellationToken);
        return Ok(cinemaRevenue);
    }
}