using AutoMapper;
using NLayer.Core.DTOs;
using NLayer.Core.IUnitOfWorks;
using NLayer.Core.Models;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Service.Services
{
    public class ProductService : Service<Product>, IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IGenericRepository<Product> repository, IUnitOfWork unitOfWork, IMapper mapper, IProductRepository productRepository) : base(repository, unitOfWork)
        {
            _mapper = mapper;
            _productRepository = productRepository;

        }
        public async Task<CustomResponseDto<List<ProductWithCategoryAndFeatureDto>>> GetProductsWithCategoryAndFeature()
        {
            var products = await _productRepository.GetProductsWithCategoryAndFeature();
            var productsDto = _mapper.Map<List<ProductWithCategoryAndFeatureDto>>(products);
            return CustomResponseDto<List<ProductWithCategoryAndFeatureDto>>.Success(200,productsDto);
        }

        public async Task<CustomResponseDto<ProductWithCategoryAndFeatureDto>> GetProductWithCategoryAndFeature(int id)
        {
            var product = await _productRepository.GetProductWithCategoryAndFeature(id);
            var productDto = _mapper.Map<ProductWithCategoryAndFeatureDto>(product);
            return CustomResponseDto<ProductWithCategoryAndFeatureDto>.Success(200, productDto);
        }
    }
}
