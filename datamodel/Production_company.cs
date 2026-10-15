/* 
    company_id INT NOT NULL PRIMARY KEY, 
    name VARCHAR(100) NOT NULL, 
    founding_year INT NOT NULL, 
    country VARCHAR(100), 
    CONSTRAINT company_founding_year CHECK (founding_year BETWEEN 1900 AND YEAR(GETDATE()))
*/

public class Production_company
{
    public int Company_id {get; set; }
    public string Name {get; set; } = string.Empty;
    public int Founding_year {get; set; }
    public string Country {get; set; } = string.Empty;
    public ICollection<Movie> Movie { get; set; } = null!;
    public ICollection<Studio> Studio { get; set; } = null!;
}