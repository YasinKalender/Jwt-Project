using JwtTokenProject.Common.DTOs;
using JwtTokenProject.Entities;
using JwtTokenProject.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JwtTokenProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IService<Product, ProductDto> _productService;

        public ProductsController(IService<Product, ProductDto> productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Products()
        {
            var result = await _productService.GetAllAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Products(ProductDto productDto)
        {
            var result = await _productService.AddAsync(productDto);
            return Ok(result);
        }
    }
}
