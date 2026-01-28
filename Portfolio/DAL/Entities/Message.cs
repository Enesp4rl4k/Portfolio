using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyPortfolio.DAL.Entities
{
    public class Message
    {
        public int MessageId { get; set; }
        public string Namesurname { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string MessageDetail { get; set; }
        public DateTime SenDate { get; set; }
        public bool IsRead { get; set; }


    }
}