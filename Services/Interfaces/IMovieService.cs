using assignment_3_movievault.DTO;
public interface IMovieService
{
    public Task<IEnumerable<MovieResponseDto>> GetAllMovieAsync(CancellationToken cancellationToken);
    public Task <MovieResponseDto?> GetMovieAsync(int id, CancellationToken cancellationToken);
    public Task<MovieResponseDto> AddMovieAsync(CreateMovieDto dto, CancellationToken cancellationToken);
    public Task<MovieResponseDto?> UpdateMovieAsync(int id, UpdateMovieDto dto, CancellationToken cancellationToken);
    public Task<bool> RemoveMovieAsync(int id, CancellationToken cancellationToken);

    public Task<IEnumerable<MovieOverviewResponseDto>> GetMovieOverview(CancellationToken cancellationToken);

    public Task<IEnumerable<MovieDetailsResponseDto>> GetMovieDetailsAsync(int id, CancellationToken cancellationToken);
}