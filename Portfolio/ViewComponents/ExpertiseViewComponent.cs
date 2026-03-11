using Microsoft.AspNetCore.Mvc;

namespace MyPortfolio.ViewComponents
{
    public class ExpertiseViewComponent : ViewComponent
    {
        private readonly DAL.Context.MyPortfolioContext _context;

        public ExpertiseViewComponent(DAL.Context.MyPortfolioContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var values = _context.Expertises.ToList();
            return View(values);
        }
    }
}
