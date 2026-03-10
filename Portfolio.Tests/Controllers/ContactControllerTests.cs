using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPortfolio.Controllers;
using MyPortfolio.DAL.Context;
using MyPortfolio.DAL.Entities;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Portfolio.Tests.Controllers
{
    public class ContactControllerTests
    {
        private MyPortfolioContext GetInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<MyPortfolioContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new MyPortfolioContext(options);
        }

        [Fact]
        public void Index_ReturnsViewResult_WithListOfContacts()
        {
            var dbName = "ContactIndexDb";
            using (var context = GetInMemoryContext(dbName))
            {
                context.Contacts.Add(new Contact { Title = "Phone", Description = "Call me", Phone1 = "123", Phone2 = "456", Email = "j@j.com", Address = "123 St" });
                context.SaveChanges();
            }

            using (var context = GetInMemoryContext(dbName))
            {
                var controller = new ContactController(context);
                var result = controller.Index();
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<List<Contact>>(viewResult.ViewData.Model);
                Assert.Single(model);
            }
        }

        [Fact]
        public void SendMessage_Post_AddsMessageAndRedirectsToDefaultIndex()
        {
            var dbName = "ContactSendMessageDb";
            using var context = GetInMemoryContext(dbName);
            
            var tempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(
                new Microsoft.AspNetCore.Http.DefaultHttpContext(), 
                Moq.Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>());

            var controller = new ContactController(context)
            {
                TempData = tempData
            };
            
            var newMsg = new Message { Namesurname = "John Doe", Email = "a@a.com", Subject = "Subj", MessageDetail = "Body" };

            var result = controller.SendMessage(newMsg);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal("Default", redirectToActionResult.ControllerName);
            Assert.Single(context.Messages);
            Assert.False(context.Messages.First().IsRead);
        }
    }
}
