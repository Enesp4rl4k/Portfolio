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
    public class AboutControllerTests
    {
        private MyPortfolioContext GetInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<MyPortfolioContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new MyPortfolioContext(options);
        }

        [Fact]
        public void Index_ReturnsViewResult_WithListOfAboutEntries()
        {
            // Arrange
            var dbName = "AboutIndexDb";
            using (var context = GetInMemoryContext(dbName))
            {
                context.Abouts.Add(new About { Title = "About Me", SubDescription = "Sub Desc", Details = "Details" });
                context.SaveChanges();
            }

            using (var context = GetInMemoryContext(dbName))
            {
                var controller = new AboutController(context);

                // Act
                var result = controller.Index();

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<List<About>>(viewResult.ViewData.Model);
                Assert.Single(model);
            }
        }

        [Fact]
        public void Create_Post_AddsAboutAndRedirects()
        {
            // Arrange
            var dbName = "AboutCreateDb";
            using var context = GetInMemoryContext(dbName);
            var controller = new AboutController(context);
            var newAbout = new About { Title = "New About", SubDescription = "New Sub", Details = "New Details" };

            // Act
            var result = controller.Create(newAbout);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Single(context.Abouts);
            Assert.Equal("New About", context.Abouts.First().Title);
        }

        [Fact]
        public void Edit_Post_UpdatesAboutAndRedirects()
        {
            // Arrange
            var dbName = "AboutEditDb";
            int expectedId;
            using (var context = GetInMemoryContext(dbName))
            {
                var about = new About { Title = "Old Title", SubDescription = "Old Sub", Details = "Old Details" };
                context.Abouts.Add(about);
                context.SaveChanges();
                expectedId = about.AboutId;
            }

            using (var context = GetInMemoryContext(dbName))
            {
                var controller = new AboutController(context);
                var updatedAbout = new About { AboutId = expectedId, Title = "Updated Title", SubDescription = "Updated Sub", Details = "Updated Details" };

                // Act
                var result = controller.Edit(updatedAbout);

                // Assert
                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", redirectToActionResult.ActionName);
                
                var dbAbout = context.Abouts.Find(expectedId);
                Assert.Equal("Updated Title", dbAbout.Title);
            }
        }

        [Fact]
        public void Delete_RemovesAboutAndRedirects()
        {
            // Arrange
            var dbName = "AboutDeleteDb";
            int expectedId;
            using (var context = GetInMemoryContext(dbName))
            {
                var about = new About { Title = "To Delete", SubDescription = "Sub", Details = "Details" };
                context.Abouts.Add(about);
                context.SaveChanges();
                expectedId = about.AboutId;
            }

            using (var context = GetInMemoryContext(dbName))
            {
                var controller = new AboutController(context);

                // Act
                var result = controller.Delete(expectedId);

                // Assert
                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", redirectToActionResult.ActionName);
                Assert.Empty(context.Abouts);
            }
        }
    }
}
