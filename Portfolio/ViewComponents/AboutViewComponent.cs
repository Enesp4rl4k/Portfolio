using Microsoft.AspNetCore.Mvc;
using MyPortfolio.DAL.Context;
using System.Linq;

namespace MyPortfolio.ViewComponents
{
    public class AboutViewComponent : ViewComponent
    {
        private readonly MyPortfolioContext context;

        public AboutViewComponent(MyPortfolioContext context)
        {
            this.context = context;
        }

        public IViewComponentResult Invoke()
        {
            var value = context.Abouts.FirstOrDefault();
            return View(value);
        }
    }
}
