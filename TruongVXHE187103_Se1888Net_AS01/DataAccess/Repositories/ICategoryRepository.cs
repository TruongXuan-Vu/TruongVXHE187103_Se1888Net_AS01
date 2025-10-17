using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;

namespace DataAccess.Repositories
{
    public class NewCategoryResponse
    {
        public short CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string CategoryDesciption { get; set; }

        public short? ParentCategoryId { get; set; }

        public bool? IsActive { get; set; }
        //them trường đếm aricle có trong 1 categoryID
        public int ArticleCount { get; set; }
    }
    public interface ICategoryRepository
    {
        Task<IEnumerable<NewCategoryResponse>> GetCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(int categoryId);
        Task<IEnumerable<NewCategoryResponse>> SearchCategoryAsync(string? categoryName, int? parentCategoryId);
        Task AddCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);

        Task DeleteCategoryAsync(int categoryId);
        Task<bool> ExistsAsync(int categoryId);
        Task<bool> HasArticlesAsync(int categoryId);

    }
}
