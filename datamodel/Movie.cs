/* 
    movie_id INT NOT NULL PRIMARY KEY,
    release_date DATETIME NOT NULL, 
    runtime INT NOT NULL CHECK (runtime > 0), 
    age_rating INT NOT NULL, 
    status VARCHAR(100) NOT NULL, 
    title VARCHAR(100) NOT NULL,
    director_id INT NOT NULL,
    company_id INT NOT NULL,
    FOREIGN KEY (director_id) REFERENCES Director(director_id) ON UPDATE CASCADE ON DELETE CASCADE,
    FOREIGN KEY (company_id) REFERENCES Production_company(company_id) ON UPDATE CASCADE ON DELETE CASCADE,
    CONSTRAINT movie_status CHECK(status IN ('planned', 'in production', 'released', 'archived'))
*/

public class Movie
{
    public int Movie_id {get; set; }
    public DateTime Release_date {get; set; }
    public int Runtime {get; set; }
    public int Age_rating {get; set; }
    public string Status {get; set; } = string.Empty;
    public string Title {get; set; } = string.Empty;
    public int Director_id {get; set; }
    public int Company_id {get; set; }
    public ICollection<Movie_actor> Movie_actor { get; set; } = null!;
    public ICollection<Movie_location> Movie_location { get; set; } = null!;
    public Production_record Production_Record { get; set; } = null!;
    public Director Director { get; set; } = null!;
    public ICollection<Screening> Screening { get; set; } = null!;
    public Production_company Production_company { get; set; } = null!;
    public ICollection<Review> Review { get; set; } = null!;
}