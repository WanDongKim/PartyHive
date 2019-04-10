using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartyHive.Models;

namespace PartyHive.Controllers
{
    public class BookingsController : Controller
    {
        private readonly PartyHiveContext _context;
        public BookingsController(PartyHiveContext context)
        {
            _context = context;
        }
        public IActionResult Confirmation(int BookingId, int UserId, int PartyId)
        {
            Booking booking = _context.Booking.Include(c=>c.Party).Include(c=>c.User).Where(x => x.BookingId.Equals(BookingId)).FirstOrDefault();
            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirmation([Bind("BookingId, UserId, PartyId, HowManyGuest")]Booking booking)
        {
            if (booking.UserId == 0)
            {
                return NotFound();
            }
            if (booking.PartyId == 0)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                Party party = _context.Party.Where(x => x.Id.Equals(booking.PartyId)).FirstOrDefault();
                if(!(Convert.ToInt32(party.CurrentEnrollment) >= Convert.ToInt32(party.MaxEnrollment)))
                {
                    party.CurrentEnrollment = (Convert.ToInt32(party.CurrentEnrollment) + booking.HowManyGuest).ToString();
                    if (HttpContext.Session.GetInt32("token") != null)
                    {
                        booking.Party = party;
                        booking.User = _context.User.Where(x => x.Id.Equals(booking.UserId)).FirstOrDefault();
                    }
                    _context.Add(booking);
                    _context.Update(party);

                    await _context.SaveChangesAsync();
                    return RedirectToAction("Confirmation", "Bookings", new { booking.BookingId, booking.UserId, booking.PartyId });
                }
            }

            return View(booking);
        }

        public IActionResult MyBookings(int id)
        {
            var bookings = _context.Booking.Include(c => c.Party).Include(c => c.User).Where(x => x.UserId.Equals(id)).ToList();

            return View(bookings);
        }
    }
}