using System;
using System.Collections.Generic;

namespace PartyHive.Models
{
    public partial class Comment
    {
        public int CommentId { get; set; }
        public string Body { get; set; }
        public int UserId { get; set; }
        public int PartyId { get; set; }
        public string Title { get; set; }

        public Party Party { get; set; }
        public User User { get; set; }
    }
}
