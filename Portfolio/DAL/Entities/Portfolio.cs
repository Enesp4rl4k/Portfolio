using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyPortfolio.DAL.Entities
{
    public class Portfolio
    {
        public int PortfolioId { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string ImageUrl { get; set; }
        public string ProjectUrl { get; set; }
        public string Description { get; set; }
    }
}