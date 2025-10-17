using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Context;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;


namespace DataAccess.Repositories
{
    public class NewsArticleRepository : INewArticleRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public NewsArticleRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddNewsArticleAsync(NewsArticle newsArticle)
        {
            await _dbContext.NewsArticles.AddAsync(newsArticle);
            await _dbContext.SaveChangesAsync();
        }

        public Task DeleteNewsArticleAsync(NewsArticle newsArticle)
        {
            throw new NotImplementedException();
        }

        public Task<NewsArticle> GetNewsArticleByIdAsync(int newsArticleId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<NewsArticleResponse>> GetNewsArticlesAsync()
        {
            var query = _dbContext.NewsArticles.AsNoTracking()
          .Join(_dbContext.Categories.AsNoTracking(),
                n => n.CategoryId,
                c => c.CategoryId,
                (n, c) => new { n, c })
          .Join(_dbContext.SystemAccounts.AsNoTracking(),
                x => x.n.CreatedById,
                cr => cr.AccountId,
                (x, cr) => new { x.n, x.c, cr })
          .GroupJoin(_dbContext.SystemAccounts.AsNoTracking(),  
                     y => y.n.UpdatedById,
                     up => up.AccountId,
                     (y, upg) => new { y.n, y.c, y.cr, up = upg.FirstOrDefault() })
          .OrderByDescending(z => z.n.CreatedDate)
          .Select(z => new NewsArticleResponse
          {
              NewsArticleId = z.n.NewsArticleId,
              NewsTitle = z.n.NewsTitle,
              Headline = z.n.Headline,
              CreatedDate = z.n.CreatedDate,
              NewsContent = z.n.NewsContent,
              NewsSource = z.n.NewsSource,
              NewsStatus = z.n.NewsStatus,

              CategoryId = z.n.CategoryId,
              CategoryName = z.c.CategoryName,

              CreatedById = z.n.CreatedById,
              CreatedByName = z.cr.AccountName,

              UpdatedById = z.n.UpdatedById,
              UpdatedByName = z.up != null ? z.up.AccountName : null,
          });

            return await query.ToListAsync();
        }

        public Task UpdateNewsArticleAsync(NewsArticle newsArticle)
        {
            throw new NotImplementedException();
        }
    }
}
