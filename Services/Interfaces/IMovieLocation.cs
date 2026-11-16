
using assignment_3_movievault.DTO;

public interface IMovieLocation
{
    public Task<MovieLocationResponseDto> AddMovieLocationAsync(int movieId, AddMovieLocationDto dto, CancellationToken cancellationToken);
    public Task<bool> RemoveMovieLocationAsync(int movieId, int locationId, CancellationToken cancellationToken);
}