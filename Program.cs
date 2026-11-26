using Microsoft.EntityFrameworkCore;
using assignment_3_movievault.Services;
using Scalar.AspNetCore;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;


var builder = WebApplication.CreateBuilder(args);

var mongoConnectionString = builder.Configuration["MongoDB:ConnectionString"];
var mongoCollectionName = builder.Configuration["MongoDB:CollectionName"];

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.MongoDBBson(
        mongoConnectionString!,
        collectionName: mongoCollectionName!)
    .CreateLogger();

builder.Host.UseSerilog();

Log.Information("MovieVault API - Serilog is working!");

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(option =>
{
    option.AddDocumentTransformer((document, context, cancellationToken) =>
    {
       document.Components ??= new();
       document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
       document.Components.SecuritySchemes.Add("Bearer", new OpenApiSecurityScheme
       {
          In = ParameterLocation.Header,
          Description = "Enter token", 
          Name = "Authorization",
          Type = SecuritySchemeType.Http,
          BearerFormat = "JWT",
          Scheme = "bearer" 
       });

       return Task.CompletedTask;
    });
});

builder.Services.AddDbContext<MovieContext>(option =>
{
   option.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]); 
});

builder.Services.AddIdentity<Identity_user, IdentityRole>(option =>
{
   option.Password.RequireDigit = true;
   option.Password.RequireLowercase = true;
   option.Password.RequireUppercase = true;
   option.Password.RequireNonAlphanumeric = true;
   option.Password.RequiredLength = 8;
}).AddEntityFrameworkStores<MovieContext>();

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme =
    option.DefaultChallengeScheme =
    option.DefaultForbidScheme =
    option.DefaultScheme =
    option.DefaultSignInScheme =
    option.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(
                builder.Configuration["JWT:SigningKey"]
                    ?? throw new InvalidOperationException("JWT signing key is not configured.")
            )
        )
    };
});

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<CinemaRevenue>();
builder.Services.AddScoped<MovieActor>();
builder.Services.AddScoped<MovieLocation>();
builder.Services.AddScoped<MovieServices>();
builder.Services.AddScoped<Screenings>();
builder.Services.AddScoped<TicketSales>();
builder.Services.AddScoped<Reviewservice>();
builder.Services.AddSingleton<LogService>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseSerilogRequestLogging(options =>
{
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("username",
            httpContext.User.Identity?.Name ?? "anonymous");

        diagnosticContext.Set("timestamp", DateTime.UtcNow);

        diagnosticContext.Set("requestType",
            httpContext.Request.Method);

        diagnosticContext.Set("route",
            httpContext.Request.Path.ToString());

        diagnosticContext.Set("statusCode",
            httpContext.Response.StatusCode);
    };
});
using var scope = app.Services.CreateScope();

var context = scope.ServiceProvider.GetRequiredService<MovieContext>();

var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Identity_user>>();
var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

IRepository<Actor> repository = new Repository<Actor>(context);
var actors = await repository.GetAsyncAll();

foreach(var actor in actors)
{
    Console.WriteLine($"Actors: , {actor.Name}, Nationality: , {actor.Nationality}");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

}

app.UseAuthentication();
app.UseAuthorization();

await SeedUsers.SeedUser(userManager, roleManager, context, builder.Configuration);

// Move Serilog here. 

app.MapControllers();



//////////////////////////////////

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})




.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
