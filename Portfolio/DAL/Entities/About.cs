using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyPortfolio.DAL.Entities
{
    public class About
    {
        public int AboutId { get; set; }
        public string Title { get; set; }
        public string SubDescription { get; set; }
        public string Details  { get; set; }

    }
}