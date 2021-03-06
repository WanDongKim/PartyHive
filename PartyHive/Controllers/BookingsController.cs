﻿using System;
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

        public async Task<IActionResult> Delete(bool confirm, int id, int userId, int partyId)
        {
            var booking = await _context.Booking
                                                .Where(x => x.BookingId.Equals(id))
                                                .Where(x=>x.PartyId.Equals(partyId))
                                                .Where(x=>x.UserId.Equals(userId))
                                                .FirstOrDefaultAsync();
            Party party = _context.Party.Where(x => x.Id.Equals(booking.PartyId)).FirstOrDefault();
            party.CurrentEnrollment = (Convert.ToInt32(party.CurrentEnrollment) - booking.HowManyGuest).ToString();
            _context.Update(party);
            _context.Booking.Remove(booking);
            await _context.SaveChangesAsync();

            return RedirectToAction("MyBookings", "Bookings", new { id = userId });
        }
        public IActionResult Edit(int id)
        {
            var booking = _context.Booking.Include(c => c.Party).Include(c => c.User).Where(x => x.UserId.Equals(id)).FirstOrDefault();
            return View(booking);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("BookingId, UserId, PartyId, HowManyGuest")]Booking booking)
        {
            if (booking.UserId == 0)
            {
                return NotFound();
            }
            if (booking.PartyId == 0)
            {
                return NotFound();
            }
            Party party = _context.Party.Include(c=>c.Host).Include(c => c.Comment).Include(c => c.Booking).Where(x => x.Id.Equals(booking.PartyId)).FirstOrDefault();
            Booking PrevBooking = _context.Booking.Include(c=>c.User).Include(c=>c.Party).Where(x => x.BookingId.Equals(booking.BookingId)).FirstOrDefault();
            int i = (int)PrevBooking.HowManyGuest;
            PrevBooking.HowManyGuest = booking.HowManyGuest;
            party.CurrentEnrollment = (Convert.ToInt32(party.CurrentEnrollment) - i + booking.HowManyGuest).ToString();
            _context.Update(PrevBooking);

            _context.Update(party);
            await _context.SaveChangesAsync();

            return RedirectToAction("MyBookings", "Bookings", new { id = booking.UserId });
        }


    }
}