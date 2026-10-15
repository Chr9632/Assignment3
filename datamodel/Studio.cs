/*
    studio_id INT NOT NULL PRIMARY KEY,
    name VARCHAR(100) NOT NULL, 
    city VARCHAR(100) NOT NULL, 
    country VARCHAR(100) NOT NULL, 
    number_sound_stages INT NOT NULL CHECK(number_sound_stages > 0), 
    max_stage_capacity INT NOT NULL CHECK(max_stage_capacity > 0),
    company_id INT NOT NULL, 
    FOREIGN KEY (company_id) REFERENCES Production_company(company_id) ON UPDATE CASCADE ON DELETE CASCADE
*/

public class Studio
{
    public int Studio_id {get; set; }
    public string Name {get; set; } = string.Empty;
    public string City {get; set; } = string.Empty;
    public string Country {get; set; } = string.Empty;
    public int Number_sound_stages {get; set; }
    public int Max_stage_capacity {get; set; }
    public int Company_id {get; set; }
    public Production_company Production_company { get; set; } = null!;
}