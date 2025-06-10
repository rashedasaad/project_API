
namespace project_API.view_models
{
    public class CrudOrderItemDto
    {
        [Required(ErrorMessage = "Order is required")]
        [Display(Name = "Order")]
        public int Orderid { get; set; }


        [Required(ErrorMessage = "Product is required")]
        [Display(Name = "Product")]
        public int Prodectid { get; set; }



        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        [Display(Name = "Quantity")]
        public double Quantity { get; set; } = 1;

    }
}
