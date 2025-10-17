using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers
{
    public class NewsArticleController : Controller
    {
        private readonly NewArticleService _newsArticleService;

        public NewsArticleController(NewArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }
        public async Task<IActionResult> Index()
        {
            var newsArticles = await _newsArticleService.GetNewsArticlesAsync();

            var list = newsArticles.Select(n => new NewsArticle
            {
                Id = n.NewsArticleId,
                Title = n.NewsTitle,
                Headline = n.Headline,
                CreatedDate = n.CreatedDate,
                Content = n.NewsContent,
                Source = n.NewsSource,
                Status = n.NewsStatus,
                CategoryId = n.CategoryId,
                CategoryName = n.CategoryName,
                CreatedById = n.CreatedById,
                CreatedByName = n.CreatedByName,
                UpdatedById = n.UpdatedById,
                UpdatedByName = n.UpdatedByName,
                
            });

            return View(list);
        }

    }
}
