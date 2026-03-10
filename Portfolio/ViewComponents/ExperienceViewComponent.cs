using Microsoft.AspNetCore.Mvc;
using MyPortfolio.DAL.Context;
using System.Linq;

namespace MyPortfolio.ViewComponents
{
    public class ExperienceViewComponent : ViewComponent
    {
        private readonly MyPortfolioContext context;

        public ExperienceViewComponent(MyPortfolioContext context)
        {
            this.context = context;
        }

        public IViewComponentResult Invoke()
        {
            var values = context.Experiences.OrderByDescending(x => x.ExperienceId).ToList();
            return View(values);
        }
    }
}
