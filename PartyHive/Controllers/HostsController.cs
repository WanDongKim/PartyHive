using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartyHive.Models;

namespace PartyHive.Controllers
{
    public class HostsController : Controller
    {
        private readonly PartyHiveContext _context;
        public HostsController(PartyHiveContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var host = await _context.Host.Include(c => c.Party).FirstOrDefaultAsync(m => m.Id.Equals(id));
            if(host == null)
            {
                return NotFound();
            }
            return View(host);
        }
    }
}