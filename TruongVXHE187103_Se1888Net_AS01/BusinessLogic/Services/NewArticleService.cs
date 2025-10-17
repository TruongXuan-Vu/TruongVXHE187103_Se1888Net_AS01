using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.DTOs;
using DataAccess.Entities;
using DataAccess.Repositories;

namespace BusinessLogic.Services
{
    public class NewArticleService
    {
        private readonly INewArticleRepository _newsRepository;

        private readonly ISystemAccountRepository _systemAccountRepository;

        public NewArticleService(INewArticleRepository newsRepository, ISystemAccountRepository systemAccountRepository)
        {
            _newsRepository = newsRepository;
            _systemAccountRepository = systemAccountRepository;
        }

        public async Task AddNewArticle(NewsArticle newsArticle)    
        {
            await _newsRepository.AddNewsArticleAsync(newsArticle);
        }
        public async Task<IEnumerable<NewsArticleDTO>> GetNewsArticlesAsync()
        {
            var news = await _newsRepository.GetNewsArticlesAsync(); // List<NewsArticle>

            return news.Select(n => new NewsArticleDTO
                {
                    NewsArticleId = n.NewsArticleId,
                    NewsTitle = n.NewsTitle,
                    Headline = n.Headline,
                    CreatedDate = n.CreatedDate,
                    NewsContent = n.NewsContent,
                    NewsSource = n.NewsSource,
                    CategoryId = n.CategoryId,
                    CategoryName = n.CategoryName,
                    NewsStatus = n.NewsStatus,
                    CreatedById = n.CreatedById,
                    CreatedByName = n.CreatedByName,
                    UpdatedById = n.UpdatedById,
                    UpdatedByName = n.UpdatedByName
                }
            ).ToList();
        }

    }
}
