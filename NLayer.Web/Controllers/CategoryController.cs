using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;
using NLayer.Service.Services;
using NLayer.Web.Models;

namespace NLayer.Web.Controllers

{
    public class CategoryController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _mapper = mapper;
            _categoryService = categoryService;
        }
        public IActionResult Index()
        {
            var categories = _categoryService
                .Where(x => true)
                .Include(x => x.Products)
                .ToList();

            var categoryDTOs = _mapper.Map<List<CategoryWithProductsDto>>(categories);
            return View(categoryDTOs);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CategoryDto model)
        {
            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrWhiteSpace(model.Name))
            {
                ModelState.AddModelError("Name", "Kategori adı girmelisiniz.");
                return View(model);
            }

            var categoryRecord = _mapper.Map<Category>(model);
            categoryRecord.CreatedDate = DateTime.Now;

            await _categoryService.AddAsync(categoryRecord);

            return RedirectPermanent("/Category/Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
         
            if (category is not null)
            { 
                var categoryDto = _mapper.Map<CategoryDto>(category);
                return View(categoryDto);
            }

            return RedirectPermanent("/Category/Index");
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CategoryDto model)
        {
            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrWhiteSpace(model.Name))
            {
                ModelState.AddModelError("Name", "Kategori adı girmelisiniz.");
                return View(model);
            }

            var categoryRecord = _mapper.Map<Category>(model);
           
            await _categoryService.UpdateAsync(categoryRecord);

            return RedirectPermanent("/Category/Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryService.GetSingleCategoryByIdWithProductsAsync(id);
            if (category.Data is not null)
            {
                return View(category.Data);
            }

            return RedirectPermanent("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category is not null)
            {
                await _categoryService.RemoveAsync(category);
            }

            return RedirectPermanent("/Category/Index");
        }

    }
}
