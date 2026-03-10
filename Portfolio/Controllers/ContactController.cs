using Microsoft.AspNetCore.Mvc;
using MyPortfolio.DAL.Context;
using MyPortfolio.DAL.Entities;
using System;
using System.Linq;

namespace MyPortfolio.Controllers
{
    public class ContactController : Controller
    {
        private readonly MyPortfolioContext context;

        public ContactController(MyPortfolioContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public IActionResult SendMessage(Message message)
        {
            message.SenDate = DateTime.Now;
            message.IsRead = false;
            
            context.Messages.Add(message);
            context.SaveChanges();

            TempData["MessageSent"] = "Your message has been sent successfully. I will get back to you soon!";
            
            return RedirectToAction("Index", "Default");
        }
        
        // Admin CRUD methods for Contact Information
        public IActionResult Index()
        {
            var values = context.Contacts.ToList();
            return View(values);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Contact contact)
        {
            context.Contacts.Add(contact);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var value = context.Contacts.Find(id);
            return View(value);
        }

        [HttpPost]
        public IActionResult Edit(Contact contact)
        {
            context.Contacts.Update(contact);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var value = context.Contacts.Find(id);
            if(value != null)
            {
                context.Contacts.Remove(value);
                context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
