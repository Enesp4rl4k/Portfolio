using Microsoft.AspNetCore.Mvc;
using MyPortfolio.DAL.Context;
using System.Linq;

namespace MyPortfolio.ViewComponents
{
    public class SkillViewComponent : ViewComponent
    {
        private readonly MyPortfolioContext context;

        public SkillViewComponent(MyPortfolioContext context)
        {
            this.context = context;
        }
        public IViewComponentResult Invoke()
        {
            var values = context.Skills.ToList();
            return View(values);
        }
    }
}
