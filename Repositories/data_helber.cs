

using project_API.Repositories.Interface;

namespace project_API.Repositories;

    public class DataHelper<T> : IdataHelper<T> where T : class
    {
        private readonly appdbcontext _db;
        public DataHelper(appdbcontext db)
        {
            _db = db;
        }

        public void Add(T item)
        {


            _db.Add(item);
            _db.SaveChanges();
        }

        public void Edite(T item)
        {
            _db.Update(item);
            _db.SaveChanges();
        }

        public void Remove(T item)
        {
            _db.Remove(item);
            _db.SaveChanges();
        }

        public async Task<T> FindByid(int id)
        {
            return await _db.Set<T>().FindAsync(id);
        }

      public  async Task<IEnumerable<T>> GetView()
        {
            return await _db.Set<T>().ToListAsync();
        }

        public async Task<IQueryable<T>> Search(string Search)
        {

            var tabels=_db.Products.AsQueryable();
            return await (Task<IQueryable<T>>)tabels.Where(x=>x.Name.Contains( Search));

        }
 

        public async Task UpdateOrderTotalAmount(int orderId)
        {
            var orderItem = await _db.Order_Items.Where(x=>x.Orderid==orderId).ToListAsync();

            var total = orderItem.Sum(x=>x.Price*x.Quantity);

            var orders=await _db.Orders.FindAsync(orderId);

            if (orders != null)
            {
                orders.TotalAmount = total;
                await _db.SaveChangesAsync();
            }

        }



        public async Task<IEnumerable<OrderSummaryDto>> total_Amount(int id)
        {
            var order = await _db.Orders
                .Include(o => o.Order_item)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return Enumerable.Empty<OrderSummaryDto>(); // أو ممكن ترجع null وتتعامل معه فوق

            var itemList = order.Order_item.Select(item => new OrderSummaryDto
            {
                orderid = order.Id,
                OrderItemId = item.Id,
                OrderDate = order.OrderDate, // انت جبتها من item.OrderF.OrderDate، لكنها غالباً موجودة بنفس Order
                TotalAmount = item.Quantity * item.Price,
                Quantity = item.Quantity,
                Price = item.Price
            }).ToList();

            return itemList;
        }




        public async Task<IEnumerable<T>> pagination(int PageNumber, int PageSize)
        {
            var FindToTabel =await _db.Set<T>().ToListAsync();
            int TotalCount =  Convert.ToInt32(Math.Ceiling(Convert.ToDouble( FindToTabel.Count)/Convert.ToDouble(PageSize)));
            
            var noSkipepageRecord =_db.Set<T>().Skip((PageNumber - 1)* PageSize).Take(PageSize).ToListAsync();

            return await noSkipepageRecord;

        }
        
        

    
    }

