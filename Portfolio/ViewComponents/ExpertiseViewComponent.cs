using Microsoft.AspNetCore.Mvc;

namespace MyPortfolio.ViewComponents
{
    public class ExpertiseViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
