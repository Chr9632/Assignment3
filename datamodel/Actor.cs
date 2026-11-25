/* 
    actor_id INT NOT NULL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    date_birth DATETIME NOT NULL,
    nationality VARCHAR(100) NOT NULL,
    gender CHAR(1) NOT NULL, 
    CONSTRAINT actor_datebirth CHECK (date_birth < GETDATE()),
    CONSTRAINT actor_gender CHECK(gender IN ('M', 'F'))
*/ 

public class Actor
{
    public int Actor_id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime Date_birth { get; set; }
    public string Nationality { get; set; } = string.Empty;
    public char Gender { get; set; }
    public ICollection<Movie_actor> Movie_actor { get; set; } = null!;
    public string? Identity_user_id { get; set; }
    public Identity_user? Identity_user { get; set; }
}