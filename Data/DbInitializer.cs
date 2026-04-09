/*fsfsfs*/

using Assignment1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Assignment1.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            await context.Database.MigrateAsync();

            // Roles
            string[] roles = { "Organizer", "Attendee" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Organizer user
            string organizerEmail = "organizer@test.com";
            string organizerPassword = "Password123!";

            var organizer = await userManager.FindByEmailAsync(organizerEmail);
            if (organizer == null)
            {
                organizer = new IdentityUser
                {
                    UserName = organizerEmail,
                    Email = organizerEmail,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(organizer, organizerPassword);
                await userManager.AddToRoleAsync(organizer, "Organizer");
            }

            // Attendee user
            string attendeeEmail = "attendee@test.com";
            string attendeePassword = "Password123!";

            var attendeeUser = await userManager.FindByEmailAsync(attendeeEmail);
            if (attendeeUser == null)
            {
                attendeeUser = new IdentityUser
                {
                    UserName = attendeeEmail,
                    Email = attendeeEmail,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(attendeeUser, attendeePassword);
                await userManager.AddToRoleAsync(attendeeUser, "Attendee");
            }

            // Seed events only once
            if (!context.Events.Any())
            {
                var event1 = new Event
                {
                    Title = "Tech Meetup",
                    Description = "Networking with students and developers.",
                    Date = DateTime.Today.AddDays(5),
                    Location = "Ottawa",
                    BannerUrl = "https://picsum.photos/800/250?random=1"
                };

                var event2 = new Event
                {
                    Title = "Career Fair",
                    Description = "Meet employers and recruiters.",
                    Date = DateTime.Today.AddDays(10),
                    Location = "Montreal",
                    BannerUrl = "https://picsum.photos/800/250?random=2"
                };

                var event3 = new Event
                {
                    Title = "Hackathon",
                    Description = "Build projects with your team.",
                    Date = DateTime.Today.AddDays(20),
                    Location = "Toronto",
                    BannerUrl = "https://picsum.photos/800/250?random=3"
                };

                context.Events.AddRange(event1, event2, event3);
                await context.SaveChangesAsync();

                context.Attendees.AddRange(
                    new Attendee { Name = "Amar", Email = "amar@test.com", EventId = event1.Id },
                    new Attendee { Name = "Souhail", Email = "souhail@test.com", EventId = event1.Id },
                    new Attendee { Name = "Jinal", Email = "jinal@test.com", EventId = event2.Id },
                    new Attendee { Name = "Nikita", Email = "nikita@test.com", EventId = event2.Id },
                    new Attendee { Name = "Mission", Email = "mission@test.com", EventId = event3.Id },
                    new Attendee { Name = "Alvin", Email = "alvin@test.com", EventId = event3.Id }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}