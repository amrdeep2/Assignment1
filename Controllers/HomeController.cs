using Assignment1.Data;
using Assignment1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Assignment1.Controllers
{
    [Route("events")]
    public class EventManagerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventManagerController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var events = await _context.Events.ToListAsync();
            ViewData["Title"] = "Event Manager";
            ViewData["EventCount"] = events.Count;

            return View(events);
        }

        [HttpGet("manageattendees/{id}")]
        public async Task<IActionResult> ManageAttendees(int id)
        {
            var ev = await _context.Events
                .Include(e => e.Attendees)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (ev == null) return NotFound();

            ViewData["EventName"] = ev.Title;
            return View(ev);
        }
        [HttpGet("details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var ev = await _context.Events
                .Include(e => e.Attendees)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (ev == null) return NotFound();

            return View(ev);
        }

        [HttpPost("manageattendees/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageAttendees(int id, string name, string email)
        {
            var ev = await _context.Events
                .Include(e => e.Attendees)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (ev == null) return NotFound();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
            {
                ViewData["EventName"] = ev.Title;
                ViewData["Error"] = "Name and Email are required.";
                return View(ev);
            }

            var attendee = new Attendee
            {
                Name = name.Trim(),
                Email = email.Trim(),
                EventId = ev.Id
            };

            _context.Attendees.Add(attendee);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ManageAttendees), new { id });
        }
        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event ev, IFormFile? bannerFile)
        {
            if (!ModelState.IsValid)
                return View(ev);

            if (bannerFile != null && bannerFile.Length > 0)
            {
                ev.BannerUrl = "https://via.placeholder.com/800x250?text=" + Uri.EscapeDataString(ev.Title);
            }

            _context.Events.Add(ev);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var ev = await _context.Events.FindAsync(id);
            if (ev == null) return NotFound();

            return View(ev);
        }

        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Event ev, IFormFile? bannerFile)
        {
            if (id != ev.Id) return NotFound();

            if (!ModelState.IsValid)
                return View(ev);

            var existing = await _context.Events.FindAsync(id);
            if (existing == null) return NotFound();

            existing.Title = ev.Title;
            existing.Description = ev.Description;
            existing.Date = ev.Date;
            existing.Location = ev.Location;

            if (bannerFile != null && bannerFile.Length > 0)
            {
                existing.BannerUrl = "https://via.placeholder.com/800x250.png?text=" + Uri.EscapeDataString(existing.Title);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

       
        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ev = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);
            if (ev == null) return NotFound();

            return View(ev);
        }

        [HttpPost("delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ev = await _context.Events.FindAsync(id);
            if (ev == null) return NotFound();

            _context.Events.Remove(ev);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}