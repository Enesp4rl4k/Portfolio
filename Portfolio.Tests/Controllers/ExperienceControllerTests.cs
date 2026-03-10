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
    public class ExperienceControllerTests
    {
        private MyPortfolioContext GetInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<MyPortfolioContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new MyPortfolioContext(options);
        }

        [Fact]
        public void Index_ReturnsViewResult_WithListOfExperiences()
        {
            var dbName = "ExpIndexDb";
            using (var context = GetInMemoryContext(dbName))
            {
                context.Experiences.Add(new Experience { Head = "Dev", Title = "Company", Date = "2020", Description = "Desc" });
                context.SaveChanges();
            }

            using (var context = GetInMemoryContext(dbName))
            {
                var controller = new ExperienceController(context);
                var result = controller.Index();
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<List<Experience>>(viewResult.ViewData.Model);
                Assert.Single(model);
            }
        }

        [Fact]
        public void Create_Post_AddsExperienceAndRedirects()
        {
            var dbName = "ExpCreateDb";
            using var context = GetInMemoryContext(dbName);
            var controller = new ExperienceController(context);
            var newExp = new Experience { Head = "Senior Dev", Title = "TechInc", Date = "2023", Description = "Desc" };

            var result = controller.Create(newExp);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Single(context.Experiences);
        }

        [Fact]
        public void Edit_Post_UpdatesExperienceAndRedirects()
        {
            var dbName = "ExpEditDb";
            int expectedId;
            using (var context = GetInMemoryContext(dbName))
            {
                var exp = new Experience { Head = "Old", Title = "Old", Date = "Old", Description = "Old" };
                context.Experiences.Add(exp);
                context.SaveChanges();
                expectedId = exp.ExperienceId;
            }

            using (var context = GetInMemoryContext(dbName))
            {
                var controller = new ExperienceController(context);
                var updatedExp = new Experience { ExperienceId = expectedId, Head = "New", Title = "New", Date = "New", Description = "New" };

                var result = controller.Edit(updatedExp);

                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", redirectToActionResult.ActionName);
                
                var dbExp = context.Experiences.Find(expectedId);
                Assert.Equal("New", dbExp.Head);
            }
        }

        [Fact]
        public void Delete_RemovesExperienceAndRedirects()
        {
            var dbName = "ExpDeleteDb";
            int expectedId;
            using (var context = GetInMemoryContext(dbName))
            {
                var exp = new Experience { Head = "Del", Title = "Del", Date = "Del", Description = "Del" };
                context.Experiences.Add(exp);
                context.SaveChanges();
                expectedId = exp.ExperienceId;
            }

            using (var context = GetInMemoryContext(dbName))
            {
                var controller = new ExperienceController(context);
                var result = controller.Delete(expectedId);
                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", redirectToActionResult.ActionName);
                Assert.Empty(context.Experiences);
            }
        }
    }
}
