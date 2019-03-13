using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PartyHive.Helper;
using PartyHive.Models;
using PartyHive.Models.Account;

namespace PartyHive.Controllers
{
    public class AccountsController : Controller
    {
        private readonly PartyHiveContext _context;
        public AccountsController(PartyHiveContext context)
        {
            _context = context;
        }
        
        //GET -  /Accounts/login
        public IActionResult Login()
        {
            if(HttpContext.Session.GetString("user") != null)
            {
                //TODO: return if user have already logged in
            }
            return View(new LoginViewModel());
        }
        //POST - /Accounts/login
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Login([Bind])
        //GET logout
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }

        //GET - /Accounts/register
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }
    }
}