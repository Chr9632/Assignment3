using Microsoft.AspNetCore.Identity;

public class Identity_user : IdentityUser
{
    public string Name { get; set; } = string.Empty;
    public Actor? Actor { get; set; }
    public Director? Director { get; set; }
    public Reviewer? Reviewer { get; set; }
}
