namespace project_API.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
public interface IPaymentRepository
{
    Task<Payment> Add(Payment payment);
    Task<Payment> Edite(Payment payment);
    Task<Payment> FindById(int id);
    Task<Payment> FindByPredicate(Expression<Func<Payment, bool>> predicate);
    Task<IEnumerable<Payment>> GetView();
    Task Remove(Payment payment);
}