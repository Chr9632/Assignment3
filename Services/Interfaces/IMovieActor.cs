using assignment_3_movievault.DTO;

public interface IMovieActor
{
    public Task<MovieActorResponseDto> AddMovieActorAsync(int movieId, AddMovieActorDto dto, CancellationToken cancellationToken);
    public Task<bool> RemoveMovieActorAsync(int movieId, int actorId, CancellationToken cancellationToken);

    public Task<IEnumerable<ActorMoviePerformanceResponseDto>> GetMoviePerformanceAsync(int id, CancellationToken cancellationToken);
}