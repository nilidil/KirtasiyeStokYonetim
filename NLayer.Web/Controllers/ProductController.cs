using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;
using NLayer.Service.Services;
using NLayer.Web.Models;

namespace NLayerWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IMemoryCache _memoryCache;
        public ProductController(IProductService productService,
            IMapper mapper,
            ICategoryService categoryService,
            IMemoryCache memoryCache,
            ILogger<ProductController> logger)
        {
            _mapper = mapper;
            _productService = productService;
            _categoryService = categoryService;
            _memoryCache = memoryCache;
            _logger = logger;
        }
        private void CleanProductCache()
        {
            if (_memoryCache.TryGetValue("product_list", out List<ProductWithCategoryAndFeatureDto> _))
            {
                _memoryCache.Remove("product_list");
            }
        }
        public async Task<IActionResult> Index()
        {
           if(_memoryCache.TryGetValue("product_list", out List<ProductWithCategoryAndFeatureDto> liste))
            {
                return View(liste);
            }


            var response = await _productService.GetProductsWithCategoryAndFeature();

            _memoryCache.Set("product_list", response.Data);

            return response.StatusCode == 200 ? View(response.Data) : View();

        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product is not null)
            {
                return View(product);
            }

            return RedirectPermanent("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product is not null)
            {
                await _productService.RemoveAsync(product);
                CleanProductCache();
            }
            _logger.LogWarning("Product deleted", id);
            return RedirectPermanent("/Product/Index");
        }



        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAllAsync();

            var categorySelectData = categories.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });

            ViewData["CategoryId"] = categorySelectData;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductWithCategoryAndFeatureDto model)
        {
            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrWhiteSpace(model.Name))
            {
                ModelState.AddModelError("Name", "Ürün adı girmelisiniz.");
                return View(model);
            }

            var productRecord = _mapper.Map<Product>(model);
            productRecord.CreatedDate = DateTime.Now;

            await _productService.AddAsync(productRecord);
            CleanProductCache();
            _logger.LogWarning("Product created", model);

            return RedirectPermanent("/Product/Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var categories = await _categoryService.GetAllAsync();

            var categorySelectData = categories.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });

            ViewData["CategoryId"] = categorySelectData;

            var product = await _productService.GetProductWithCategoryAndFeature(id);

            if (product.Data is not null)
            {
                return View(product.Data);
            }

            return RedirectPermanent("/Product/Index");
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ProductWithCategoryAndFeatureDto model)
        {
            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrWhiteSpace(model.Name))
            {
                ModelState.AddModelError("Name", "Ürün adı girmelisiniz.");
                return View(model);
            }

            var productRecord = _mapper.Map<Product>(model); 

            await _productService.UpdateAsync(productRecord);
            CleanProductCache();
            return RedirectPermanent("/Product/Index");
        }
    }
}
