using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.DTOs;
using BusinessLogic.Validation;
using DataAccess.Entities;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{
    public class CategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        private readonly CategoryValidator _categoryValidator;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;

        }


        public async Task DeleteCategoryAsync(short id) // đồng bộ kiểu dữ liệu
        {
            await _categoryRepository.DeleteCategoryAsync(id);
        }
        public async Task<IEnumerable<CategoryDTO>> GetCategoriesAsync()
        {
            var news = await _categoryRepository.GetCategoriesAsync();

            return news.Select(c => new CategoryDTO
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                CategoryDesciption = c.CategoryDesciption,
                ParentCategoryId = c.ParentCategoryId,
                IsActive = c.IsActive,
                ArticleCount = c.ArticleCount
            }
            ).ToList();
        }

       
        public async Task<bool> ExistsAsync(short id)
        {
            await _categoryRepository.ExistsAsync(id);
            return true;
        }

        public async Task<bool> HasArticlesAsync(short id)
        {
            return await _categoryRepository.HasArticlesAsync(id);
        }


        public async Task<IEnumerable<NewCategoryResponse>> SearchCategoryAsync(string? categoryName, int? parentCategoryId)
        {
            return await _categoryRepository.SearchCategoryAsync(categoryName, parentCategoryId);
        }


        public async Task AddCategoryAsync(CategoryDTO category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

           
            var existing = await _categoryRepository.GetCategoriesAsync();
            if (existing.Any(c => c.CategoryName.Equals(category.CategoryName, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("Tên danh mục đã tồn tại.");

            // ✅ Kiểm tra ParentCategoryId hợp lệ (nếu có)
            if (category.ParentCategoryId.HasValue)
            {
                bool parentExists = await _categoryRepository.ExistsAsync(category.ParentCategoryId.Value);
                if (!parentExists)
                    throw new InvalidOperationException("Danh mục cha không tồn tại.");
            }

            Category ct = new Category
            {
                CategoryId = 0,
                CategoryName = category.CategoryName,
                CategoryDesciption = category.CategoryDesciption,
                ParentCategoryId = category.ParentCategoryId,
                IsActive = category.IsActive
            };

            // ✅ Gọi repository thêm mới
            await _categoryRepository.AddCategoryAsync(ct);
        }

      
    }
}