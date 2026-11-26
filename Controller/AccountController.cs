

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly MovieContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AccountController> _logger;
    private readonly UserManager<Identity_user> _usermanager;
    private readonly SignInManager<Identity_user> _signinmanager;

    public AccountController(
        MovieContext context, 
        IConfiguration configuration, 
        ILogger<AccountController> logger, 
        UserManager<Identity_user> usermanager, 
        SignInManager<Identity_user> signinmanager 
        )
    {
        _context = context;
        _configuration = configuration;
        _logger = logger;
        _usermanager = usermanager;
        _signinmanager = signinmanager;
    }

    [AllowAnonymous]
    [HttpPost("/api/auth/login")]
    public async Task<IActionResult> Login(LoginDto input)
    {
       
        if(ModelState.IsValid)
        {
            var user = await _usermanager.FindByNameAsync(input.Name);

            if(user == null || string.IsNullOrEmpty(input.Password) || !await _usermanager.CheckPasswordAsync(user, input.Password))
            {
                return Unauthorized("Invalid login attempt");
            }

            else
            {
                var signingCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["JWT:SigningKey"])),
                    SecurityAlgorithms.HmacSha256
                );

                var roles = await _usermanager.GetRolesAsync(user);

                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

                var jwtObject = new JwtSecurityToken(
                    issuer: _configuration["JWT:Issuer"],
                    audience: _configuration["JWT:Audience"], 
                    claims: claims,
                    expires: DateTime.Now.AddSeconds(300), 
                    signingCredentials: signingCredentials
                );

                var jwtString = new JwtSecurityTokenHandler().WriteToken(jwtObject);
                return StatusCode(StatusCodes.Status200OK, jwtString);
            }
            
        }

        return BadRequest(ModelState);
    }
}