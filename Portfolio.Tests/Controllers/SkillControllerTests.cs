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
    public class SkillControllerTests
    {
        private MyPortfolioContext GetInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<MyPortfolioContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new MyPortfolioContext(options);
        }

        [Fact]
        public void Index_ReturnsViewResult_WithListOfSkills()
        {
            var dbName = "SkillIndexDb";
            using (var context = GetInMemoryContext(dbName))
            {
                context.Skills.Add(new Skill { Title = "C#", Value = 90 });
                context.SaveChanges();
            }

            using (var context = GetInMemoryContext(dbName))
            {
                var controller = new SkillController(context);
                var result = controller.Index();
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<List<Skill>>(viewResult.ViewData.Model);
                Assert.Single(model);
            }
        }

        [Fact]
        public void Create_Post_AddsSkillAndRedirects()
        {
            var dbName = "SkillCreateDb";
            using var context = GetInMemoryContext(dbName);
            var controller = new SkillController(context);
            var newSkill = new Skill { Title = "SQL", Value = 80 };

            var result = controller.Create(newSkill);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Single(context.Skills);
            Assert.Equal("SQL", context.Skills.First().Title);
        }

        [Fact]
        public void Edit_Post_UpdatesSkillAndRedirects()
        {
            var dbName = "SkillEditDb";
            int expectedId;
            using (var context = GetInMemoryContext(dbName))
            {
                var skill = new Skill { Title = "HTML", Value = 70 };
                context.Skills.Add(skill);
                context.SaveChanges();
                expectedId = skill.SkillId;
            }

            using (var context = GetInMemoryContext(dbName))
            {
                var controller = new SkillController(context);
                var updatedSkill = new Skill { SkillId = expectedId, Title = "HTML5", Value = 95 };

                var result = controller.Edit(updatedSkill);

                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", redirectToActionResult.ActionName);
                
                var dbSkill = context.Skills.Find(expectedId);
                Assert.Equal("HTML5", dbSkill.Title);
                Assert.Equal(95, dbSkill.Value);
            }
        }

        [Fact]
        public void Delete_RemovesSkillAndRedirects()
        {
            var dbName = "SkillDeleteDb";
            int expectedId;
            using (var context = GetInMemoryContext(dbName))
            {
                var skill = new Skill { Title = "CSS", Value = 60 };
                context.Skills.Add(skill);
                context.SaveChanges();
                expectedId = skill.SkillId;
            }

            using (var context = GetInMemoryContext(dbName))
            {
                var controller = new SkillController(context);
                var result = controller.Delete(expectedId);
                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", redirectToActionResult.ActionName);
                Assert.Empty(context.Skills);
            }
        }
    }
}
