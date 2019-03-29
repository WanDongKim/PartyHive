using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartyHive.Models;

namespace PartyHive.Controllers
{
    public class PartiesController : Controller
    {
        private readonly PartyHiveContext _context;
        public PartiesController(PartyHiveContext context)
        {
            _context = context;
        }
        // parties/index
        public IActionResult Index()
        {
            // print only active parties
            IEnumerable<Party> allParties = _context.Party.Include(c => c.Host).Where(x =>x.IsActivated.Equals(true)).ToArray();
            return View(allParties);
        }
        // parties/edit/id
        public async Task<IActionResult> Edit(int id)
        {
            if(id == null)
            {
                NotFound();
            }
            var party = await _context.Party.Include(c => c.Host).FirstOrDefaultAsync();

            if(party == null)
            {
                NotFound();
            }

            return View(party);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Price, Address, Description, MaxEnrollment, IsActivated, Name")]Party editedParty)
        {
            var party = await _context.Party.Include(c => c.Host).FirstOrDefaultAsync(m => m.Id.Equals(id));

            party.Price = editedParty.Price;
            party.Address = editedParty.Address;
            party.Description = editedParty.Description;
            party.MaxEnrollment = editedParty.MaxEnrollment;
            party.IsActivated = editedParty.IsActivated;
            party.Name = editedParty.Name;
            
            if(id != editedParty.Id)
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
            return RedirectToAction("Index");
        }
        // parties/detail/
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var party = await _context.Party
                                    .Include(c => c.Host)
                                    .Where(x => x.Id.Equals(id))
                                    .FirstOrDefaultAsync();
            if(party == null)
            {
                return NotFound();
            }

            return View(party);
        }

    }
}