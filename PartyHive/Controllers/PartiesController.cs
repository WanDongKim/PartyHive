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
    public class PartiesController : Controller
    {
        static PartyHiveContext _context;
        public PartiesController(PartyHiveContext context)
        {
            _context = context;
        }
        // parties/index
        public IActionResult Index()
        {
            // print only active parties
            IEnumerable<Party> allParties = _context.Party.Include(c => c.Host).Include(c => c.Comment).Where(x => x.IsActivated.Equals(true)).ToArray();
            return View(allParties);
        }

        public IActionResult Add()
        {
            Party party = new Party();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add([Bind("Id, Name, Description, DateTime, Price, TargetAudience, Address, MaxEnrollment")]Party party)
        {
            party.HostId = (int)HttpContext.Session.GetInt32("token");
            Host host = _context.Host.Include(c => c.Party).Where(m => m.Id.Equals(party.HostId)).FirstOrDefault();
            party.Host = host;
            party.CurrentEnrollment = 0.ToString();

            if (ModelState.IsValid)
            {
                _context.Party.Add(party);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Hosts", new { id = party.HostId});
        }
        // parties/edit/id
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                NotFound();
            }
            var party = await _context.Party.Include(c => c.Host).Where(m => m.Id.Equals(id)).FirstOrDefaultAsync();

            if (party == null)
            {
                NotFound();
            }

            return View(party);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Price, Address, DateTime, Description, MaxEnrollment, IsActivated, Name")]Party editedParty)
        {
            var party = await _context.Party.Include(c => c.Host).Include(c => c.Comment).FirstOrDefaultAsync(m => m.Id.Equals(id));

            party.Price = editedParty.Price;
            party.Address = editedParty.Address;
            party.Description = editedParty.Description;
            party.DateTime = editedParty.DateTime;
            party.MaxEnrollment = editedParty.MaxEnrollment;
            party.IsActivated = editedParty.IsActivated;
            party.Name = editedParty.Name;

            if (id != editedParty.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Party.Update(party);
                    await _context.SaveChangesAsync();
                }

                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
            return RedirectToAction("Index", "Hosts", new { id = party.HostId });
        }
        // parties/detail/
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            IEnumerable<Comment> comment =  _context.Comment.Include(c => c.User).Include(c => c.Party).AsEnumerable();
            var party = await _context.Party
                                    .Include(c => c.Host)
                                    .Include(c => c.Comment)
                                    .Where(x => x.Id.Equals(id))
                                    .FirstOrDefaultAsync();
            if (party == null)
            {
                return NotFound();
            }

            return View(party);
        }
        public async Task<IActionResult> Inactive(int? id)
        {
            if(id == null)
            {
                NotFound();
            }
            var party = await _context.Party.Include(c => c.Host).Include(c => c.Comment).FirstOrDefaultAsync(m => m.Id.Equals(id));

            if(party == null)
            {
                NotFound();
            }
            return View(party);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Inactive(int id, bool isActivte = false)
        {
            if (id == null)
            {
                NotFound();
            }

            var party = await _context.Party.Include(c => c.Host).Include(c => c.Comment).FirstOrDefaultAsync(m => m.Id.Equals(id));


            if (party == null)
            {
                NotFound();
            }

            party.IsActivated = !party.IsActivated;

            try
            {
                _context.Party.Update(party);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return RedirectToAction("Index","Hosts",new { id=party.HostId });
        }
        public static User getUser(Comment c)
        {
            return c.User = _context.User.Where(x => x.Id.Equals(c.UserId)).FirstOrDefault();
        }
        public static bool isJoined(int id, int partyId)
        {
            if(_context.Booking.Where(x=>x.UserId.Equals(id)).Where(x=>x.PartyId.Equals(partyId)).FirstOrDefault() != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}