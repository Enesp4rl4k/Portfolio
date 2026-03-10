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
    public class FeatureControllerTests
    {
        private MyPortfolioContext GetInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<MyPortfolioContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new MyPortfolioContext(options);
        }

        [Fact]
        public void Index_ReturnsViewResult_WithListOfFeatures()
        {
            // Arrange
            var dbName = "FeatureIndexDb";
            using (var context = GetInMemoryContext(dbName))
            {
                context.Features.Add(new Feature { Description = "Test Feature 1" });
                context.Features.Add(new Feature { Description = "Test Feature 2" });
                context.SaveChanges();
            }

            using (var context = GetInMemoryContext(dbName))
            {
                var controller = new FeatureController(context);

                // Act
                var result = controller.Index();

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<List<Feature>>(viewResult.ViewData.Model);
                Assert.Equal(2, model.Count);
            }
        }

        [Fact]
        public void Create_Post_AddsFeatureAndRedirects()
        {
            // Arrange
            var dbName = "FeatureCreateDb";
            using var context = GetInMemoryContext(dbName);
            var controller = new FeatureController(context);
            var newFeature = new Feature { Description = "New Feature" };

            // Act
            var result = controller.Create(newFeature);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Single(context.Features);
            Assert.Equal("New Feature", context.Features.First().Description);
        }

        [Fact]
        public void Edit_Post_UpdatesFeatureAndRedirects()
        {
            // Arrange
            var dbName = "FeatureEditDb";
            int expectedId;
            using (var context = GetInMemoryContext(dbName))
            {
                var feature = new Feature { Description = "Old Description" };
                context.Features.Add(feature);
                context.SaveChanges();
                expectedId = feature.FeatureId;
            }

            using (var context = GetInMemoryContext(dbName))
            {
                var controller = new FeatureController(context);
                var updatedFeature = new Feature { FeatureId = expectedId, Description = "New Description" };

                // Act
                var result = controller.Edit(updatedFeature);

                // Assert
                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", redirectToActionResult.ActionName);
                
                var dbFeature = context.Features.Find(expectedId);
                Assert.Equal("New Description", dbFeature.Description);
            }
        }

        [Fact]
        public void Delete_RemovesFeatureAndRedirects()
        {
            // Arrange
            var dbName = "FeatureDeleteDb";
            int expectedId;
            using (var context = GetInMemoryContext(dbName))
            {
                var feature = new Feature { Description = "To Be Deleted" };
                context.Features.Add(feature);
                context.SaveChanges();
                expectedId = feature.FeatureId;
            }

            using (var context = GetInMemoryContext(dbName))
            {
                var controller = new FeatureController(context);

                // Act
                var result = controller.Delete(expectedId);

                // Assert
                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", redirectToActionResult.ActionName);

                Assert.Empty(context.Features);
            }
        }
    }
}
