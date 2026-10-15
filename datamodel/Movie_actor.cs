/* 
    actor_id INT NOT NULL, 
    movie_id INT NOT NULL,
    role VARCHAR(100) NOT NULL,
    character_name VARCHAR(100) NOT NULL,
    PRIMARY KEY (actor_id, movie_id), 
    FOREIGN KEY (actor_id) REFERENCES Actor(actor_id) ON UPDATE CASCADE ON DELETE CASCADE,
    FOREIGN KEY (movie_id) REFERENCES Movie(movie_id) ON UPDATE CASCADE ON DELETE CASCADE,
    CONSTRAINT actor_role CHECK (role IN ('lead', 'supporting', 'cameo'))
*/

public class Movie_actor
{
    public int Actor_id {get; set; }
    public int Movie_id {get; set; }
    public string Role {get; set; } = string.Empty;
    public string Character_name {get; set; } = string.Empty;

    public Movie Movie { get; set; } = null!;
    public Actor Actor { get; set; } = null!;

}