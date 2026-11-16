using assignment_3_movievault.DTO;

public interface IScreening
{
    public Task<IEnumerable<ScreeningResponseDto>> GetScreeningMoviesAsync(int movieId, CancellationToken cancellationToken);
    public Task<ScreeningResponseDto> AddScreeningAsync(CreateScreeningDto dto, CancellationToken cancellationToken);
    public Task<IEnumerable<ScreeningResponseDto>> GetScreeningCinemasAsync(int cinemaId, CancellationToken cancellationToken);
    public Task<ScreeningResponseDto?> UpdateScreeningAsync(int id, UpdateScreeningDto dto, CancellationToken cancellationToken);
    public Task<bool> RemoveScreeningAsync(int id, CancellationToken cancellationToken);

    public Task<IEnumerable<UpcomingScreeningResponseDto>> ShowUpcomingScreeningAsync(CancellationToken cancellationToken);
}