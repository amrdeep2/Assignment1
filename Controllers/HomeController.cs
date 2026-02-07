using Microsoft.AspNetCore.Mvc;
using Assignment1.Models;
using System.Linq;

namespace Assignment1.Controllers
{
    public class EventManagerController : Controller
    {
        // Hardcoded events to simulate database persistence
        private static List<Event> _events = new List<Event>
        {
            new Event
            {
                id = 1,
                title = "Tech Meetup",
                Date = DateTime.Today.AddDays(3),
                Location = "Ottawa",
                Attendees = new List<Attendee>()
            },
            new Event
            {
                id = 2,
                title = "Career Fair",
                Date = DateTime.Today.AddDays(10),
                Location = "Montreal",
                Attendees = new List<Attendee>()
            },
            new Event
            {
                id = 3,
                title = "Hackathon",
                Date = DateTime.Today.AddDays(20),
                Location = "Toronto",
                Attendees = new List<Attendee>()
            }
        };

        // GET: /EventManager
        public IActionResult Index()
        {
            ViewData["Title"] = "Event Manager";
            ViewData["EventCount"] = _events.Count;

            return View(_events);
        }

        // GET: /EventManager/ManageAttendees/1
        [HttpGet]
        public IActionResult ManageAttendees(int id)
        {
            var ev = _events.FirstOrDefault(e => e.id == id);
            if (ev == null) return NotFound();

            // Make sure list exists (safety)
            ev.Attendees ??= new List<Attendee>();

            // Event name/title
            ViewData["EventName"] = ev.title;

            return View(ev);
        }

        // POST: /EventManager/ManageAttendees/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ManageAttendees(int id, string name, string email)
        {
            var ev = _events.FirstOrDefault(e => e.id == id);
            if (ev == null) return NotFound();

            ev.Attendees ??= new List<Attendee>();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
            {
                ViewData["EventName"] = ev.title;
                ViewData["Error"] = "Name and Email are required.";
                return View(ev);
            }

            var attendee = new Attendee();
            attendee.setName(name.Trim());
            attendee.setEmail(email.Trim());

            // Use your method
            ev.addList(attendee);

            return RedirectToAction(nameof(ManageAttendees), new { id });
        }
    }
}
