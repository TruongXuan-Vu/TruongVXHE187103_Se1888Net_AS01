using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Context;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;


namespace DataAccess.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

   

 

        public async Task<IEnumerable<NewCategoryResponse>> GetCategoriesAsync()
        {
            var result = await (
                from c in _dbContext.Categories.AsNoTracking()
                join n in _dbContext.NewsArticles.AsNoTracking()
                    on c.CategoryId equals n.CategoryId into g // group join (LEFT JOIN)
                orderby c.CategoryName
                select new NewCategoryResponse
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    CategoryDesciption = c.CategoryDesciption,
                    ParentCategoryId = c.ParentCategoryId,
                    IsActive = c.IsActive,

                    // Đếm số lượng bài viết thuộc danh mục
                    ArticleCount = g.Count()
                }
            ).ToListAsync();

            return result;
        }

        public Task<Category> GetCategoryByIdAsync(int categoryId)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            // lấy ra category cơ bản
            var existing = await _dbContext.Categories.FirstOrDefaultAsync(c => c.CategoryId == category.CategoryId);

            if (existing == null)
            {
                throw new InvalidOperationException("Category ko tồn tại.");
            }
        
            bool existingArticle = await _dbContext.NewsArticles.AnyAsync(c => c.CategoryId == existing.CategoryId);

            if (category.ParentCategoryId.HasValue)
            {
                if(category.ParentCategoryId == category.CategoryId)
                {
                    throw new InvalidOperationException("không thể vừa là con vừa là cha");
                }
                //tìm trong categories có category nào
               bool parentExists = await _dbContext.Categories.AnyAsync(c => c.CategoryId == category.ParentCategoryId);
                if (!parentExists)
                {
                    throw new InvalidOperationException("Parent category không tồn tại.");
                }
               

            }
            if (existingArticle)
                throw new InvalidOperationException(
                    "When editing, ParentCategoryID cannot be changed if the category is already used by articles."
                );
            existing.CategoryName = category.CategoryName;
            existing.CategoryDesciption = category.CategoryDesciption;
            existing.IsActive = category.IsActive;

            if (!existingArticle)
                existing.ParentCategoryId = category.ParentCategoryId;

            await _dbContext.SaveChangesAsync();
        }



        public async Task<bool> ExistsAsync(int categoryId)
        {
            return await _dbContext.Categories
                .AsNoTracking()
                .AnyAsync(c => c.CategoryId == categoryId);
        }

        // Kiểm tra xem danh mục có bài viết nào không
        public async Task<bool> HasArticlesAsync(int categoryId)
        {
            return await _dbContext.NewsArticles
                .AsNoTracking()
                .AnyAsync(a => a.CategoryId == categoryId);
        }

        // Hàm xóa danh mục (bạn đã có)
        public async Task DeleteCategoryAsync(int categoryId)
        {
            await _dbContext.Categories
                .Where(c => c.CategoryId == categoryId)
                .ExecuteDeleteAsync();
        }
        public async Task<IEnumerable<NewCategoryResponse>> SearchCategoryAsync(string? categoryName, int? parentCategoryId)
        {
            var query = _dbContext.Categories.AsQueryable();

            // 🔍 Tìm theo tên hoặc mô tả
            if (!string.IsNullOrWhiteSpace(categoryName))
            {
                query = query.Where(c =>
                    c.CategoryName.Contains(categoryName) ||
                    c.CategoryDesciption.Contains(categoryName));
            }

            // 🔗 Lọc theo ParentCategoryId (nếu có)
            if (parentCategoryId.HasValue)
            {
                query = query.Where(c => c.ParentCategoryId == parentCategoryId.Value);
            }
            var categories = await query
                .AsNoTracking()
                .OrderBy(c => c.CategoryName)
                .Select(c => new NewCategoryResponse
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    CategoryDesciption = c.CategoryDesciption,
                    ParentCategoryId = c.ParentCategoryId,
                    IsActive = c.IsActive,
                   
                })
                .ToListAsync();

            return categories;
        }


        public async Task AddCategoryAsync(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            // Kiểm tra trùng tên (tuỳ theo nghiệp vụ bạn muốn)
            bool exists = await _dbContext.Categories
                .AnyAsync(c => c.CategoryName == category.CategoryName);

            if (exists)
                throw new InvalidOperationException("Category name already exists.");

            // Xử lý giá trị mặc định
            if (!category.IsActive.HasValue)
                category.IsActive = true;

            // Gán ParentCategoryId = null nếu người dùng không chọn
            if (category.ParentCategoryId == 0)
                category.ParentCategoryId = null;

            // Thêm và lưu
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
        }



    }
}
