
using Microsoft.AspNetCore.Identity;
using SharpCompress.Compressors.ZStandard.Unsafe;

public class SeedUsers
{
    public static async Task SeedUser(UserManager<Identity_user> userManager, RoleManager<IdentityRole> roleManager, MovieContext context, IConfiguration configuration)
    {
        string[] roles = {"Actor", "Director", "Reviewer", "Admin"};

        foreach(var role in roles)
        {
            if(!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        const string adminEmail = "admin@movievault.com";

        if(await userManager.FindByNameAsync(adminEmail) == null)
        {
            var user = new Identity_user
            {
                Name = "Admin User",
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(
                user,
                configuration["SeedUsers:AdminPassword"]
                    ?? throw new InvalidOperationException("SeedUsers:AdminPassword is not configured."));

            if(result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }

        const string actorEmail = "LD@movievault.com";

        if(await userManager.FindByNameAsync(actorEmail) == null)
        {
            var userActor = new Identity_user
            {
                Name = "Leonardo DiCaprio", 
                UserName = actorEmail,
                Email = actorEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(
                userActor,
                configuration["SeedUsers:ActorPassword"]
                    ?? throw new InvalidOperationException("SeedUsers:ActorPassword is not configured."));

            if(result.Succeeded)
            {
                await userManager.AddToRoleAsync(userActor, "Actor");
                var actor = await context.Actors.FindAsync(1);
                if(actor != null)
                {
                    actor.Identity_user_id = userActor.Id;
                }
            }
        }

        const string directorEmail = "CN@movievault.com";

        if(await userManager.FindByNameAsync(directorEmail) == null)
        {
            var userDirector = new Identity_user
            {
                Name = "Christopher Nolan", 
                UserName = directorEmail,
                Email = directorEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(
                userDirector,
                configuration["SeedUsers:DirectorPassword"]
                    ?? throw new InvalidOperationException("SeedUsers:DirectorPassword is not configured."));

            if(result.Succeeded)
            {
                await userManager.AddToRoleAsync(userDirector, "Director");
                
                var director = await context.Directors.FindAsync(1);
                if(director != null)
                {
                    director.Identity_user_id = userDirector.Id;
                }
            }
        }

        // Alex Morgan
        const string reviewer1Email = "AM@movievault.com";

        if(await userManager.FindByNameAsync(reviewer1Email) == null)
        {
            var user1 = new Identity_user
            {
                Name = "Alex Morgan", 
                UserName = reviewer1Email,
                Email = reviewer1Email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(
                user1,
                configuration["SeedUsers:ReviewerAlexPassword"]
                    ?? throw new InvalidOperationException("SeedUsers:ReviewerAlexPassword is not configured."));

            if(result.Succeeded)
            {
                await userManager.AddToRoleAsync(user1, "Reviewer");
                
                var reviewer1 = await context.Reviewers.FindAsync(1);
                if(reviewer1 != null)
                {
                    reviewer1.Identity_user_id = user1.Id;
                }
            }
        }

        // Jamie Taylor
        const string reviewer2Email = "JT@movievault.com";

        if(await userManager.FindByNameAsync(reviewer2Email) == null)
        {
            var user2 = new Identity_user
            {
                Name = "Jamie Taylor", 
                UserName = reviewer2Email,
                Email = reviewer2Email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(
                user2,
                configuration["SeedUsers:ReviewerJamiePassword"]
                    ?? throw new InvalidOperationException("SeedUsers:ReviewerJamiePassword is not configured."));

            if(result.Succeeded)
            {
                await userManager.AddToRoleAsync(user2, "Reviewer");
                
                var reviewer2 = await context.Reviewers.FindAsync(2);
                if(reviewer2 != null)
                {
                    reviewer2.Identity_user_id = user2.Id;
                }
            }
        }

        // Sam Rivera
        const string reviewer3Email = "SR@movievault.com";

        if(await userManager.FindByNameAsync(reviewer3Email) == null)
        {
            var user3 = new Identity_user
            {
                Name = "Sam Rivera", 
                UserName = reviewer3Email,
                Email = reviewer3Email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(
                user3,
                configuration["SeedUsers:ReviewerSamPassword"]
                    ?? throw new InvalidOperationException("SeedUsers:ReviewerSamPassword is not configured."));

            if(result.Succeeded)
            {
                await userManager.AddToRoleAsync(user3, "Reviewer");
                
                var reviewer3 = await context.Reviewers.FindAsync(3);
                if(reviewer3 != null)
                {
                    reviewer3.Identity_user_id = user3.Id;
                }
            }
        }

        var simulatorUsername = configuration["Simulator:Authentication:Username"];
        var simulatorPassword = configuration["Simulator:Authentication:Password"];

        if(string.IsNullOrWhiteSpace(simulatorUsername)
            != string.IsNullOrWhiteSpace(simulatorPassword))
        {
            throw new InvalidOperationException(
                "Both simulator authentication username and password must be configured.");
        }

        if(!string.IsNullOrWhiteSpace(simulatorUsername)
            && !string.IsNullOrWhiteSpace(simulatorPassword))
        {
            if(new[]
            {
                adminEmail,
                actorEmail,
                directorEmail,
                reviewer1Email,
                reviewer2Email,
                reviewer3Email
            }.Contains(simulatorUsername, StringComparer.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    "The simulator account must be separate from the demonstration users.");
            }

            var simulatorUser = await userManager.FindByNameAsync(simulatorUsername);
            if(simulatorUser == null)
            {
                simulatorUser = new Identity_user
                {
                    Name = "MovieVault Ticket Simulator",
                    UserName = simulatorUsername,
                    Email = simulatorUsername,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(simulatorUser, simulatorPassword);
                if(!result.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Could not create simulator account: {string.Join("; ", result.Errors.Select(error => error.Description))}");
                }
            }

            if(!await userManager.IsInRoleAsync(simulatorUser, "Admin"))
            {
                var result = await userManager.AddToRoleAsync(simulatorUser, "Admin");
                if(!result.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Could not assign the Admin role to the simulator account: {string.Join("; ", result.Errors.Select(error => error.Description))}");
                }
            }
        }

        await context.SaveChangesAsync();

    }

}