using Assignment1.Data;
using Assignment1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Assignment1.Controllers
{
    public class EventManagerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventManagerController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var events = await _context.Events.ToListAsync();
            ViewData["EventCount"] = events.Count;
            return View(events);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var ev = await _context.Events
                .Include(e => e.Attendees)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (ev == null) return NotFound();

            return View(ev);
        }

        [Authorize(Roles = "Organizer")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Organizer")]
        public async Task<IActionResult> Create(Event ev, IFormFile? bannerFile)
        {
            if (!ModelState.IsValid)
                return View(ev);

            if (bannerFile != null && bannerFile.Length > 0)
            {
                ev.BannerUrl = "https://picsum.photos/800/250?random=" + Guid.NewGuid();
            }
            else if (string.IsNullOrWhiteSpace(ev.BannerUrl))
            {
                ev.BannerUrl = "https://picsum.photos/800/250?random=" + Guid.NewGuid();
            }

            _context.Events.Add(ev);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Organizer")]
        public async Task<IActionResult> Edit(int id)
        {
            var ev = await _context.Events.FindAsync(id);
            if (ev == null) return NotFound();

            return View(ev);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Organizer")]
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
                existing.BannerUrl = "https://picsum.photos/800/250?random=" + Guid.NewGuid();
            }
            else if (string.IsNullOrWhiteSpace(existing.BannerUrl))
            {
                existing.BannerUrl = "https://picsum.photos/800/250?random=" + Guid.NewGuid();
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Organizer")]
        public async Task<IActionResult> Delete(int id)
        {
            var ev = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);
            if (ev == null) return NotFound();

            return View(ev);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Organizer")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ev = await _context.Events.FindAsync(id);
            if (ev == null) return NotFound();

            _context.Events.Remove(ev);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Organizer")]
        public async Task<IActionResult> ManageAttendees(int id)
        {
            var ev = await _context.Events
                .Include(e => e.Attendees)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (ev == null) return NotFound();

            ViewData["EventName"] = ev.Title;
            return View(ev);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Organizer")]
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

        [Authorize(Roles = "Organizer")]
        public async Task<IActionResult> RemoveAttendee(int id, string attendeeId)
        {
            var attendee = await _context.Attendees
                .FirstOrDefaultAsync(a => a.Id == attendeeId && a.EventId == id);

            if (attendee == null) return NotFound();

            _context.Attendees.Remove(attendee);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ManageAttendees), new { id });
        }
    }
}