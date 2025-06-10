


namespace project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly IdataHelper<Order_item> _rebo;
        private readonly IdataHelper<Products> _prod;
        private readonly appdbcontext _db;

        public OrderItemsController(IdataHelper<Order_item> rebo, appdbcontext db, IdataHelper<Products> prod)
        {
            _rebo = rebo;
            _db = db;
            _prod = prod;
        }

        [HttpGet("ViewData")]
        public async Task<IActionResult> GetViewOrderItem()
        {
            var getForData = await _rebo.GetView();
            if (getForData == null || !getForData.Any())
                return NotFound();

            return Ok(getForData);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddNewOrderItem([FromBody] CrudOrderItemDto orderItemDto)
        {
            var product = await _prod.FindByid(orderItemDto.Prodectid);
            if (product == null)
                return NotFound("المنتج غير موجود");

            var existingItem = await _db.Order_Items
                .FirstOrDefaultAsync(x => x.Orderid == orderItemDto.Orderid && x.ProductId == orderItemDto.Prodectid);

            if (existingItem != null)
                return BadRequest("هذا المنتج موجود بالفعل في الطلب.");

            var item = new Order_item
            {
                Orderid = orderItemDto.Orderid,
                ProductId = orderItemDto.Prodectid,
                Price = product.Price,
                Quantity = orderItemDto.Quantity,
            };

             _rebo.Add(item);
            await _rebo.UpdateOrderTotalAmount(item.Orderid);

            return CreatedAtAction(nameof(GetViewOrderItem), new { id = item.Id }, item);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateOrderItem(int id, [FromBody] CrudOrderItemDto orderItemDto)
        {
            var findOrder = await _rebo.FindByid(id);
            if (findOrder == null)
                return NotFound();

            var product = await _prod.FindByid(orderItemDto.Prodectid);
            if (product == null)
                return NotFound("المنتج غير موجود");

            findOrder.Orderid = orderItemDto.Orderid;
            findOrder.ProductId = orderItemDto.Prodectid;
            findOrder.Price = product.Price;
            findOrder.Quantity = orderItemDto.Quantity;

             _rebo.Edite(findOrder);
            await _rebo.UpdateOrderTotalAmount(findOrder.Orderid);

            return Ok(findOrder);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            var findOrder = await _rebo.FindByid(id);
            if (findOrder == null)
                return NotFound();

            _rebo.Remove(findOrder);
            return NoContent();
        }



        //عندك نوعين من summary  api  وفي واحد ثاني في order  اشوف انه الي في order سريع لانه كل ما يطلب كثير يجمعها مره وحده 
        [HttpGet("summary/{id}")]
        public async Task<ActionResult<OrderSummaryDto>> GetOrderSummary(int id)
        {
            var summary = await _rebo.total_Amount(id);
            if (summary == null)
                return NotFound();

            return Ok(summary);
        }
    }







}
