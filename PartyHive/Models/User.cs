using System;
using System.Collections.Generic;

namespace PartyHive.Models
{
    public partial class User
    {
        public User()
        {
            Booking = new HashSet<Booking>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Phone { get; set; }
        public string Address { get; set; }

        public ICollection<Booking> Booking { get; set; }
    }
}
