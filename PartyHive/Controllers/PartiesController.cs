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
            IEnumerable<Party> allParties = _context.Party.Include(c => c.Host).ToArray();
            return View(allParties);
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