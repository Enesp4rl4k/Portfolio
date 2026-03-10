using Microsoft.AspNetCore.Mvc;
using MyPortfolio.DAL.Context;
using System.Linq;

namespace MyPortfolio.ViewComponents
{
    public class ContactViewComponent : ViewComponent
    {
        private readonly MyPortfolioContext context;

        public ContactViewComponent(MyPortfolioContext context)
        {
            this.context = context;
        }

        public IViewComponentResult Invoke()
        {
            var value = context.Contacts.FirstOrDefault();
            return View(value);
        }
    }
}
