/*
    cinema_id INT NOT NULL PRIMARY KEY,
    name VARCHAR(100) NOT NULL, 
    city VARCHAR(100) NOT NULL, 
    street_address VARCHAR(100) NOT NULL,
    opening_year INT NOT NULL CHECK(opening_year <= YEAR(GETDATE()))
*/

public class Cinema
{
    public int Cinema_id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Street_address { get; set; } = string.Empty; 
    public int Opening_year { get; set; }
    public ICollection<Screen> Screen { get; set; } = null!;

}