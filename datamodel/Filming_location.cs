/*
    location_id INT NOT NULL PRIMARY KEY, 
    name VARCHAR(100) NOT NULL,
    city VARCHAR(100) NOT NULL, 
    country VARCHAR(100) NOT NULL, 
    location_type VARCHAR(100) NOT NULL,
    CONSTRAINT filming_location_type CHECK (location_type IN ('street', 'building', 'landscape', 'set'))
*/

public class Filming_location
{
    public int Location_id {get; set; }
    public string Name {get; set; } = string.Empty;
    public string City {get; set; } = string.Empty;
    public string Country {get; set; } = string.Empty;
    public string Location_type {get; set; } = string.Empty;
    public ICollection<Movie_location> Movie_location { get; set; } = null!;

}