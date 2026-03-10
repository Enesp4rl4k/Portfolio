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
    public class MessageControllerTests
    {
        private MyPortfolioContext GetInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<MyPortfolioContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new MyPortfolioContext(options);
        }

        [Fact]
        public void Index_ReturnsViewResult_WithListOfMessages()
        {
            var dbName = "MsgIndexDb";
            using (var context = GetInMemoryContext(dbName))
            {
                context.Messages.Add(new Message { Namesurname = "John", Email = "j@j.com", Subject = "Sub", MessageDetail = "Det" });
                context.SaveChanges();
            }

            using (var context = GetInMemoryContext(dbName))
            {
                var controller = new MessageController(context);
                var result = controller.Index();
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<List<Message>>(viewResult.ViewData.Model);
                Assert.Single(model);
            }
        }

        [Fact]
        public void Read_MarksMessageAsReadAndReturnsView()
        {
            var dbName = "MsgReadDb";
            int expectedId;
            using (var context = GetInMemoryContext(dbName))
            {
                var msg = new Message { Namesurname = "John", Email = "j@j.com", Subject = "Sub", MessageDetail = "Det", IsRead = false };
                context.Messages.Add(msg);
                context.SaveChanges();
                expectedId = msg.MessageId;
            }

            using (var context = GetInMemoryContext(dbName))
            {
                var controller = new MessageController(context);
                var result = controller.Read(expectedId);

                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<Message>(viewResult.ViewData.Model);
                
                Assert.True(model.IsRead);
                var dbMsg = context.Messages.Find(expectedId);
                Assert.True(dbMsg.IsRead);
            }
        }
    }
}
