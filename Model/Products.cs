
using System.Text.Json.Serialization;

namespace project_API.Model
{
    public class Products
    {
        [Key]
        public int Id {  get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Product name must be between 3 and 100 characters")]
        [Display(Name = "Product Name")]
        public string Name { get; set; }



        [StringLength(2500, ErrorMessage = "Description cannot exceed 2500 characters")]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }


        [Required]
        [DataType(DataType.Currency)]
        [Range(0.01, 100000, ErrorMessage = "Price must be between 0.01 and 100,000")]
        public double Price { get; set; }



        [DataType(DataType.Text)]
        public string? Country { get; set; }
        public double Amount {  get; set; }
        [Required]
        
        public int CategoryId { get; set; }
   
        public virtual Categories? Category { get; set; }



        //[Url(ErrorMessage = "Please enter a valid URL")]
        [Display(Name = "Product Image URL")]
        public byte[]? ImageUrl { get; set; }
        


    }
}
