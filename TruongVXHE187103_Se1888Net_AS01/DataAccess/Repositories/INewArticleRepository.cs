using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;

namespace DataAccess.Repositories
{
    public class NewsArticleResponse
    {
        public string NewsArticleId { get; set; }

        public string? NewsTitle { get; set; }

        public string? Headline { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? NewsContent { get; set; }

        public string? NewsSource { get; set; }

        public short? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool? NewsStatus { get; set; }

        public short? CreatedById { get; set; }
        //thêm trường CreatedByName và UpdatedByName
        public string? CreatedByName { get; set; }
        public string UpdatedByName { get; set; }

        public short? UpdatedById { get; set; }
    }
    public interface INewArticleRepository
    {
        Task<IEnumerable<NewsArticleResponse>> GetNewsArticlesAsync();
        Task<NewsArticle> GetNewsArticleByIdAsync(int newsArticleId);
        Task AddNewsArticleAsync(NewsArticle newsArticle);
        Task UpdateNewsArticleAsync(NewsArticle newsArticle);
        Task DeleteNewsArticleAsync(NewsArticle newsArticle);
    }

}
