using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPortfolio.DAL.Context;
using MyPortfolio.DAL.Entities;
using System.Linq;

namespace MyPortfolio.Controllers
{
    [Authorize]
    public class ExpertiseController : Controller
    {
        private readonly MyPortfolioContext context;

        public ExpertiseController(MyPortfolioContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var values = context.Expertises.ToList();
            return View(values);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Expertise expertise)
        {
            context.Expertises.Add(expertise);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var value = context.Expertises.Find(id);
            return View(value);
        }

        [HttpPost]
        public IActionResult Edit(Expertise expertise)
        {
            context.Expertises.Update(expertise);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var value = context.Expertises.Find(id);
            context.Expertises.Remove(value);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
