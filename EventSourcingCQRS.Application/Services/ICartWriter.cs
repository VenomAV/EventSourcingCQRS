using System.Threading.Tasks;

namespace EventSourcingCQRS.Application.Services
{
    public interface ICartWriter
    {
        Task CreateAsync(string customerId);

        Task AddProductAsync(string cartId, string productId, int quantity);

        Task ChangeProductQuantityAsync(string cartId, string productId, int quantity);
    }
}
