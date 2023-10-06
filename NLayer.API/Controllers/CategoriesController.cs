using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using NLayer.Core.Services;

namespace NLayerAPI.Controllers
{
    
    public class CategoriesController : CustomBaseController
    {
        private readonly ICategoryService _categoryService;

        //api/categories/GetSingleCategoryByIdWithProducts/2
      
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
    
        [HttpGet("[action]/{categoryId}")]
        public async Task< IActionResult> GetSingleCategoryByWithProducts(int categoryId)
        {
            return CreateActionResult(await _categoryService.GetSingleCategoryByIdWithProductsAsync(categoryId));
        }

    }
}
