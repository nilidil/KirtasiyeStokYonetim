using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NLayer.Core.Services;
using NLayerWeb.Models;
using System.Diagnostics;

namespace NLayerWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        public HomeController(ILogger<HomeController> logger,
            IProductService productService,
            ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }
        public IActionResult Index()
        {
            ViewBag.UrunSayisi = _productService.GetAllAsync().Result.Count();
            ViewBag.KategoriSayisi = _categoryService.GetAllAsync().Result.Count();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}