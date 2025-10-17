using BusinessLogic.DTOs;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
namespace Presentation.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            // kiểm tra tồn tại
            if (!await _categoryService.ExistsAsync(id))
            {
                TempData["Error"] = "Danh mục không tồn tại.";
                return RedirectToAction(nameof(Index));
            }

            // kiểm tra còn bài viết
            if (await _categoryService.HasArticlesAsync(id))
            {
                TempData["Error"] = "Không thể xóa vì danh mục vẫn còn bài viết.";
                return RedirectToAction(nameof(Index));
            }

            // xóa
            await _categoryService.DeleteCategoryAsync(id);
            TempData["Success"] = "Xóa danh mục thành công.";
            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetCategoriesAsync();

            var model = categories.Select(c => new Category
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                CategoryDesciption = c.CategoryDesciption,
                ParentCategoryId = c.ParentCategoryId,
                IsActive = c.IsActive,
                ArticleCount = c.ArticleCount
            }).ToList();

            return View(model);
        }

        public async Task<IActionResult> Search(string? keyword, int? parentCategoryId)
        {
            var categories = await _categoryService.SearchCategoryAsync(keyword, parentCategoryId);


            ViewBag.Keyword = keyword;
            ViewBag.ParentCategoryId = parentCategoryId;

            var model = categories.Select(c => new Category
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                CategoryDesciption = c.CategoryDesciption,
                ParentCategoryId = c.ParentCategoryId,
                IsActive = c.IsActive,
                ArticleCount = c.ArticleCount
            }).ToList();

            return View("Index", model);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Presentation.Models.Category form)
        {
            if (string.IsNullOrWhiteSpace(form.CategoryName))
            {
                TempData["Error"] = "Tên danh mục không được để trống.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var entity = new CategoryDTO
                {
                    CategoryName = form.CategoryName,
                    CategoryDesciption = form.CategoryDesciption,
                    ParentCategoryId = form.ParentCategoryId,
                    IsActive = form.IsActive
                };

                Console.WriteLine("ịdsfsd",entity.CategoryName,entity.CategoryDesciption,entity.ParentCategoryId,entity.IsActive);


                await _categoryService.AddCategoryAsync(entity);
                TempData["Success"] = "Tạo danh mục thành công.";
            }
            catch (InvalidOperationException ex) { TempData["Error"] = ex.Message; }
            catch { TempData["Error"] = "Đã xảy ra lỗi trong quá trình tạo danh mục."; }

            return RedirectToAction(nameof(Index));
        }
    }
    }