using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([Bind("Email, Password, UserType")]LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                if(loginViewModel != null)
                {

                }
            }
            else
            {
                ViewData["Message"] = "Email address or Password doesn't exist";
            }
            return View();
        }
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

        //POST - Accounts/register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("FirstName, LastName, EmailAddress, Password, ConfirmPassword, PhoneNumber, Address, UserType")]RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                if (!UserExists(registerViewModel.EmailAddress))
                {
                    switch (registerViewModel.UserType)
                    {
                        case "Customer":
                            User user = new User
                            {
                                Email = registerViewModel.EmailAddress,
                                Password = EncryptionPassword(registerViewModel.Password),
                                FirstName = registerViewModel.FirstName,
                                LastName = registerViewModel.LastName,
                                Phone = registerViewModel.PhoneNumber,
                                Address = registerViewModel.Address
                            };

                            await _context.User.AddAsync(user);
                            break;
                        case "Host":
                            Host host = new Host
                            {
                                Email = registerViewModel.EmailAddress,
                                Password = EncryptionPassword(registerViewModel.Password),
                                FirstName = registerViewModel.FirstName,
                                LastName = registerViewModel.LastName,
                                Phone = registerViewModel.PhoneNumber,
                                Address = registerViewModel.Address
                            };

                            await _context.Host.AddAsync(host);
                            break;
                        default:
                            throw new Exception();
                    }
                    await _context.SaveChangesAsync();
                    ViewData["Message"] = "Registration succeed";
                    return View("Login");
                }
            }
            //TODO: Need to create UI for exception w/ message choose different email address
            throw new Exception();
        }

        private bool UserExists(string email)
        {
            bool hostEmailVal = _context.Host.Any(e => e.Email.Equals(email));
            bool customerEmailVal = _context.User.Any(e => e.Email.Equals(email));

            if(hostEmailVal || customerEmailVal)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private string EncryptionPassword(string password)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            UTF8Encoding encode = new UTF8Encoding();

            byte[] en = md5.ComputeHash(encode.GetBytes(password));
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < en.Length; i++)
            {
                sb.Append(en[i].ToString());
            }

            return sb.ToString();
        }
    }
}