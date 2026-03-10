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
    public class PortfolioControllerTests
    {
        private MyPortfolioContext GetInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<MyPortfolioContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new MyPortfolioContext(options);
        }

        [Fact]
        public void Index_ReturnsViewResult_WithListOfPortfolios()
        {
            var dbName = "PortfolioIndexDb";
            using (var context = GetInMemoryContext(dbName))
            {
                context.Portfolios.Add(new MyPortfolio.DAL.Entities.Portfolio { Title = "Proj1", Subtitle = "Sub1", ImageUrl = "Img1", Description = "Desc1", ProjectUrl = "Url1" });
                context.SaveChanges();
            }

            using (var context = GetInMemoryContext(dbName))
            {
                var controller = new PortfolioController(context);
                var result = controller.Index();
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<List<MyPortfolio.DAL.Entities.Portfolio>>(viewResult.ViewData.Model);
                Assert.Single(model);
            }
        }

        [Fact]
        public void Create_Post_AddsPortfolioAndRedirects()
        {
            var dbName = "PortfolioCreateDb";
            using var context = GetInMemoryContext(dbName);
            var controller = new PortfolioController(context);
            var newPortfolio = new MyPortfolio.DAL.Entities.Portfolio { Title = "Proj2", Subtitle = "Sub2", ImageUrl = "Img2", Description = "Desc2", ProjectUrl = "Url2" };

            var result = controller.Create(newPortfolio);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Single(context.Portfolios);
            Assert.Equal("Proj2", context.Portfolios.First().Title);
        }

        [Fact]
        public void Edit_Post_UpdatesPortfolioAndRedirects()
        {
            var dbName = "PortfolioEditDb";
            int expectedId;
            using (var context = GetInMemoryContext(dbName))
            {
                var portfolio = new MyPortfolio.DAL.Entities.Portfolio { Title = "ProjOld", Subtitle = "SubOld", ImageUrl = "ImgOld", Description = "DescOld", ProjectUrl = "UrlOld" };
                context.Portfolios.Add(portfolio);
                context.SaveChanges();
                expectedId = portfolio.PortfolioId;
            }

            using (var context = GetInMemoryContext(dbName))
            {
                var controller = new PortfolioController(context);
                var updatedPortfolio = new MyPortfolio.DAL.Entities.Portfolio { PortfolioId = expectedId, Title = "ProjNew", Subtitle = "SubNew", ImageUrl = "ImgNew", Description = "DescNew", ProjectUrl = "UrlNew" };

                var result = controller.Edit(updatedPortfolio);

                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", redirectToActionResult.ActionName);
                
                var dbPortfolio = context.Portfolios.Find(expectedId);
                Assert.Equal("ProjNew", dbPortfolio.Title);
            }
        }

        [Fact]
        public void Delete_RemovesPortfolioAndRedirects()
        {
            var dbName = "PortfolioDeleteDb";
            int expectedId;
            using (var context = GetInMemoryContext(dbName))
            {
                var portfolio = new MyPortfolio.DAL.Entities.Portfolio { Title = "ProjDel", Subtitle = "SubDel", ImageUrl = "ImgDel", Description = "DescDel", ProjectUrl = "UrlDel" };
                context.Portfolios.Add(portfolio);
                context.SaveChanges();
                expectedId = portfolio.PortfolioId;
            }

            using (var context = GetInMemoryContext(dbName))
            {
                var controller = new PortfolioController(context);
                var result = controller.Delete(expectedId);
                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", redirectToActionResult.ActionName);
                Assert.Empty(context.Portfolios);
            }
        }
    }
}
