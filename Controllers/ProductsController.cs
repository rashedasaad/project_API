


using Microsoft.AspNetCore.Authorization;
using project_API.Repositories;
using project_API.view_models;
using Microsoft.EntityFrameworkCore;
namespace project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IdataHelper<Products> _rebo;
        private readonly IProductRepository _productRepository;
        public ProductsController(IdataHelper<Products> rebo, IProductRepository productRepository)
        {
            _rebo = rebo;
            _productRepository = productRepository;
        }

        // GET /Products/ViewData
        [HttpGet("ViewData")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productRepository.GetAllAsync();
            if (products == null || !products.Any())
                return NotFound("لا توجد منتجات.");
            return Ok(products);
        }
     [HttpGet("Filter")]
        public async Task<IActionResult> FilterProducts([FromQuery] FilterProductsRequest request)
        {
            try
            {
                // Start with a queryable to build the filter dynamically
            
                var query = await _productRepository.GetAllAsync();
                // Filter by category IDs
                if (!string.IsNullOrEmpty(request.CategoryId))
                {
                    var categoryIds = new List<int>();
                    try
                    {
                        categoryIds = request.CategoryId.Split(',')
                            .Select(s => int.Parse(s.Trim()))
                            .ToList();
                    }
                    catch (FormatException)
                    {
                        return BadRequest("Invalid category ID format.");
                    }

                    query = query.Where(p => categoryIds.Contains(p.CategoryId));
                }

                // Filter by country
                if (!string.IsNullOrEmpty(request.Country))
                {
                    var countries = request.Country.Split(',')
                        .Select(c => c.Trim())
                        .ToList();
                    query = query.Where(p => countries.Contains(p.Country));
                }

                // Filter by price range
                if (request.MinPrice.HasValue)
                {
                    if (request.MinPrice < 0)
                        return BadRequest("Minimum price cannot be negative.");
                    query = query.Where(p => p.Price >= (double)request.MinPrice.Value);
                }

                if (request.MaxPrice.HasValue)
                {
                    if (request.MaxPrice < 0)
                        return BadRequest("Maximum price cannot be negative.");
                    if (request.MinPrice.HasValue && request.MaxPrice < request.MinPrice)
                        return BadRequest("Maximum price cannot be less than minimum price.");
                    query = query.Where(p => p.Price <= (double)request.MaxPrice.Value);
                }

                // Filter by search term (e.g., product name or description)
                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    string searchTerm = request.SearchTerm.Trim().ToLower();
                    query = query.Where(p =>
                        p.Name.ToLower().Contains(searchTerm) ||
                        p.Description.ToLower().Contains(searchTerm));
                }

                // Execute query to get filtered products
                var products =  query
                    .OrderBy(p => p.Id);
                   
                // If no products are found
                if (!products.Any())
                    return NotFound("لا توجد منتجات مطابقة للفلاتر.");

                // Return the product list directly
                return Ok(products);
            }
            catch (Exception ex)
            {
                // Log the exception (use your logging framework)
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("TopSelling")]
        public async Task<IActionResult> GetTopSellingProducts()
        {
            var topProducts = await _productRepository.GetTopSellingProductsAsync(4); // You can change 4 to any number
            if (topProducts == null || !topProducts.Any())
                return NotFound("لا توجد بيانات مبيعات.");

            return Ok(topProducts);
        }

        [HttpGet("DistinctCountries")]
        public async Task<IActionResult> GetDistinctCountries()
        {
            var products = await _productRepository.GetAllAsync();


            var countries = products
                .Where(p => !string.IsNullOrWhiteSpace(p.Country))
                .Select(p => p.Country!.Trim())
                .Distinct()
                .ToList();

            return Ok(countries);
        }

        // GET /Products/Pagination?pageNumber=1&pageSize=10
        [HttpGet("Pagination")]
        public async Task<IActionResult> GetProductsPaged(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var pagedResult = await _rebo.pagination(pageNumber, pageSize);
            return Ok(pagedResult);
        }

        [HttpPost("Add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddNewProduct([FromForm] CrudprodectDto ProductDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (ProductDto.ImageUrl == null || ProductDto.ImageUrl.Length == 0)
                return BadRequest("الصورة مطلوبة.");

            using var ms = new MemoryStream();
            await ProductDto.ImageUrl.CopyToAsync(ms);

            var item = new Products
            {
                Name = ProductDto.Name,
                Price = ProductDto.Price,
                Description = ProductDto.Description,
                Amount = ProductDto.Amount,
                CategoryId = ProductDto.CategoryId,
                Country = ProductDto.Country,
                ImageUrl = ms.ToArray()
            };

             _rebo.Add(item);
            return CreatedAtAction(nameof(GetProducts), new { id = item.Id }, item);
        }

        [HttpPut("Update/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] CrudprodectDto ProductDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await _rebo.FindByid(id);
            if (existing == null)
                return NotFound($"المنتج بالمعرّف {id} غير موجود.");

            // إذا أرسل المستخدم صورة جديدة
            if (ProductDto.ImageUrl != null && ProductDto.ImageUrl.Length > 0)
            {
                using var ms = new MemoryStream();
                await ProductDto.ImageUrl.CopyToAsync(ms);
                existing.ImageUrl = ms.ToArray();
            }

            existing.Name = ProductDto.Name;
            existing.Price = ProductDto.Price;
            existing.Description = ProductDto.Description;
            existing.Amount = ProductDto.Amount;
            existing.CategoryId = ProductDto.CategoryId;
            existing.Country = ProductDto.Country;

             _rebo.Edite(existing);
            return Ok(existing);
        }

        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var existing = await _rebo.FindByid(id);
            if (existing == null)
                return NotFound($"المنتج بالمعرّف {id} غير موجود.");

             _rebo.Remove(existing);
            return NoContent();
        }
    }






}
