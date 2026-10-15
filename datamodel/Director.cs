/* 
    director_id INT NOT NULL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    date_birth DATETIME NOT NULL,
    nationality VARCHAR(100) NOT NULL,
    CONSTRAINT director_datebirth CHECK (date_birth < GETDATE())
*/

public class Director
{
    public int Director_id {get; set; }
    public string Name {get; set; } = string.Empty;
    public DateTime Date_birth {get; set; }
    public string Nationality {get; set; } = string.Empty;
    public ICollection<Movie> Movie { get; set; } = null!;
    
}