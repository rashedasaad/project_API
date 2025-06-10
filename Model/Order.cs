
using System.Text.Json.Serialization;

namespace project_API.Model
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual User? User { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Order Date")]
        public DateTime? OrderDate { get; set; }= DateTime.Now;
        [Display(Name = "Total Amount")]
        public double TotalAmount {  get; set; } 
        public string Status { get; set; }
        [Display(Name = "Shipping Address")]

        public string ShippingAddress { get; set; }

        [Display(Name = "payment Method")]
        public string PaymentMethod { get; set; }
        //[JsonIgnore] // تجاهل هذه الخاصية عند التسلسل

        public virtual ICollection<Order_item>? Order_item { get; set; }

    }
}
