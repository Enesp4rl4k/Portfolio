using Microsoft.AspNetCore.Mvc;

namespace MyPortfolio.Views.Shared.Components
{
    public class _NavbarComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
