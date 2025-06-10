
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
namespace project_API.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly appdbcontext _dbContext;
    private IPaymentRepository _paymentRepositoryImplementation;

    public PaymentRepository(appdbcontext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Payment> Add(Payment payment)
    {
        await _dbContext.Payments.AddAsync(payment);
        await _dbContext.SaveChangesAsync();
        return payment;
    }

    public async Task<Payment> Edite(Payment payment)
    {
        _dbContext.Payments.Update(payment);
        await _dbContext.SaveChangesAsync();
        return payment;
    }

    public async Task<Payment> FindById(int id)
    {
        return await _dbContext.Payments
            .Include(p => p.Order)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Payment> FindByPredicate(Expression<Func<Payment, bool>> predicate)
    {
        return await _dbContext.Payments
            .Include(p => p.Order)
            .FirstOrDefaultAsync(predicate);
    }

    public async Task<IEnumerable<Payment>> GetView()
    {
        return await _dbContext.Payments
            .Include(p => p.Order)
            .ToListAsync();
    }

    public async Task Remove(Payment payment)
    {
        _dbContext.Payments.Remove(payment);
        await _dbContext.SaveChangesAsync();
    }
}