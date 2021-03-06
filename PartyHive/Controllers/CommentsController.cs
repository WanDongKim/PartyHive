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
    public class CommentsController : Controller
    {
        private readonly PartyHiveContext _context;
        public CommentsController(PartyHiveContext context)
        {
            _context = context;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CommentId, Title, Body, UserId, PartyId")]Comment newComment)
        {
            if (ModelState.IsValid)
            {
                if(HttpContext.Session.GetInt32("token") != null)
                {
                    int id = (int)HttpContext.Session.GetInt32("token");
                    newComment.UserId = id;
                    newComment.User = _context.User.Include(c => c.Comment).Where(x => x.Id.Equals(id)).FirstOrDefault();
                    newComment.Party = _context.Party.Where(x => x.Id.Equals(newComment.PartyId)).FirstOrDefault();
                }
                _context.Add(newComment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Parties", new { id = newComment.PartyId });
            }
            return View(newComment);
        }


        public async Task<IActionResult> Delete(bool confirm, int id)
        {
            var comment = await _context.Comment.Where(x => x.CommentId.Equals(id)).FirstOrDefaultAsync();

            _context.Comment.Remove(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Parties", new { id = comment.PartyId });
        }
    }
}