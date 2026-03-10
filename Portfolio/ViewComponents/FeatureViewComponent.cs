using Microsoft.AspNetCore.Mvc;
using MyPortfolio.DAL.Context;
using System.Linq;

namespace MyPortfolio.ViewComponents
{
    public class FeatureViewComponent : ViewComponent
    {
        private readonly MyPortfolioContext context;

        public FeatureViewComponent(MyPortfolioContext context)
        {
            this.context = context;
        }
        public IViewComponentResult Invoke()
        {
            var value = context.Features.FirstOrDefault();
            return View(value);
        }
    }
}
