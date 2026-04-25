using System.Threading.Tasks;

namespace Application.JobServices
{
    public interface IOrderCompletionJobService
    {
        Task ProcessPendingOrders();
    }
}
