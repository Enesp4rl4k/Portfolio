using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyPortfolio.DAL.Entities
{
    public class Contact
    {
        public int ContactId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

    }
}