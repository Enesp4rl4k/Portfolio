using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPortfolio.DAL.Context;
using System.Linq;

namespace MyPortfolio.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
        private readonly MyPortfolioContext context;

        public MessageController(MyPortfolioContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var values = context.Messages.OrderByDescending(x => x.SenDate).ToList();
            return View(values);
        }

        public IActionResult Read(int id)
        {
            var value = context.Messages.Find(id);
            if (value != null && !value.IsRead)
            {
                value.IsRead = true;
                context.Messages.Update(value);
                context.SaveChanges();
            }
            return View(value);
        }

        public IActionResult Delete(int id)
        {
            var value = context.Messages.Find(id);
            if(value != null)
            {
                context.Messages.Remove(value);
                context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
