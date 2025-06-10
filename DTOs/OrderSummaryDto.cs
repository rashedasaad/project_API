namespace project_API.view_models
{
    public class OrderSummaryDto
    {
        public int orderid { get; set; }

        public int OrderItemId { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Order Date")]
        public DateTime? OrderDate { get; set; } = DateTime.Now;
        public  double TotalAmount { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }

        public string Status { get; set; }


    }
}
