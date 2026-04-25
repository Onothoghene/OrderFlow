using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IPaymentRepositoryAsync : IGenericRepositoryAsync<Payment>
    {
        //Task<Payment> GetPaymentByIdAsync(int id);
        Task<IEnumerable<Payment>> GetPaymentsByOrderIdAsync(int orderId);
        Task<Payment> GetPaymentByOrderIdAsync(int orderId);
    }
}
