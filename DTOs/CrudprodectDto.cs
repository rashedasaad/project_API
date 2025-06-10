
namespace project_API.view_models
{
    public class CrudprodectDto
    {
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Product name must be between 3 and 100 characters")]
        [Display(Name = "Product Name")]
        public string Name { get; set; }
        [StringLength(2500, ErrorMessage = "Description cannot exceed 2500 characters")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public double Price { get; set; }
        [DataType(DataType.Text)]
        public string? Country { get; set; }
        public double Amount { get; set; }
        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [Display(Name = "Product Image URL")]
        public IFormFile ImageUrl { get; set; }



    }
    public class FilterProductsRequest
    {
        public string? CategoryId { get; set; }
        public string? Country { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? SearchTerm { get; set; }
    }


}
