using Assignment1.Models;

namespace Assignment1.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            if (context.Events.Any())
                return;

            var event1 = new Event
            {
                Title = "Tech Meetup",
                Description = "Networking with students and developers.",
                Date = DateTime.Today.AddDays(5),
                Location = "Ottawa",
                BannerUrl = "https://via.placeholder.com/800x250?text=Tech+Meetup"
            };

            var event2 = new Event
            {
                Title = "Career Fair",
                Description = "Meet employers and recruiters.",
                Date = DateTime.Today.AddDays(10),
                Location = "Montreal",
                BannerUrl = "https://via.placeholder.com/800x250?text=Career+Fair"
            };

            var event3 = new Event
            {
                Title = "Hackathon",
                Description = "Build projects with your team.",
                Date = DateTime.Today.AddDays(20),
                Location = "Toronto",
                BannerUrl = "https://via.placeholder.com/800x250?text=Hackathon"
            };

            context.Events.AddRange(event1, event2, event3);
            context.SaveChanges();

            var attendees = new List<Attendee>
            {
                new Attendee { Name = "Amar", Email = "amar@test.com", EventId = event1.Id },
                new Attendee { Name = "Souhail", Email = "souhail@test.com", EventId = event1.Id },

                new Attendee { Name = "Jinal", Email = "jinal@test.com", EventId = event2.Id },
                new Attendee { Name = "Nikita", Email = "nikita@test.com", EventId = event2.Id },

                new Attendee { Name = "Mission", Email = "mission@test.com", EventId = event3.Id },
                new Attendee { Name = "Alvin", Email = "alvin@test.com", EventId = event3.Id }
            };

            context.Attendees.AddRange(attendees);
            context.SaveChanges();
        }
    }
}