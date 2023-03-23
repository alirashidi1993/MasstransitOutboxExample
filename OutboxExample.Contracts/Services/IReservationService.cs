

using OutboxExample.Contracts.Persistence.Models;

namespace OutboxExample.Contracts.Services
{
    public interface IReservationService
    {
        Task Submit(Guid customerId, int tableNo, DateTime startFrom, DateTime endIn, int peopleCount);
    }
}
