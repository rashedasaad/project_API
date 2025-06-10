namespace project_API.Repositories.Interface;

    public interface IdataHelper<T> where T : class
    {
       
       Task< IEnumerable<T>> GetView();
        Task <T> FindByid(int id);
        Task< IQueryable<T>> Search(string sersh);
        Task<IEnumerable<OrderSummaryDto>> total_Amount(int id);
        Task< IEnumerable<T>> pagination(int nopage=1,int pageRecord=3);
         Task UpdateOrderTotalAmount(int orderId);


        void Add(T item );
        void Edite(T item);
        void Remove(T item);
       





    }
