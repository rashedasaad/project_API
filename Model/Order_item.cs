

using System.Text.Json.Serialization;

namespace project_API.Model
{
    public class Order_item
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Order is required")]
        [Display(Name = "Order")]
        public int Orderid { get; set; }
        [JsonIgnore] // تجاهل هذه الخاصية عند التسلسل
        public virtual Order? Order { get; set; }
  
        [Required(ErrorMessage = "Product is required")]
        [Display(Name = "Product")]
        public int ProductId { get; set; }
     
        public virtual Products? Product { get; set; }


        [Required(ErrorMessage = "Unit price is required")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        [Display(Name = "Unit Price")]
        public double Price { get; set; }


        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        [Display(Name = "Quantity")]
        public double Quantity { get; set; } = 1;

      // public virtual ICollection<Products>? Order_item { get; set; }

    }
}
