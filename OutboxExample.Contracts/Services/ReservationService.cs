

using MassTransit;
using Microsoft.EntityFrameworkCore;
using OutboxExample.Contracts.MessageContracts;
using OutboxExample.Contracts.Persistence;
using OutboxExample.Contracts.Persistence.Models;

namespace OutboxExample.Contracts.Services
{
    public class ReservationService : IReservationService
    {
        private readonly AppDbContext dbContext;
        private readonly IPublishEndpoint publishEndpoint;

        public ReservationService(AppDbContext dbContext,IPublishEndpoint publishEndpoint)
        {
            this.dbContext = dbContext;
            this.publishEndpoint = publishEndpoint;
        }
        public async Task Submit(Guid customerId,int tableNo, DateTime startFrom,DateTime endIn, int peopleCount)
        {
            var reservation=new Reservation(customerId,tableNo,startFrom,endIn,peopleCount);
            dbContext.Reservations.Add(reservation);

            await publishEndpoint.Publish(new ReservationSubmitted
            {
                ReservationId=reservation.Id,
                CustomerId = customerId,
                TableNo = tableNo,
                StartFrom = startFrom,
                EndIn = endIn,
                PeopleCount = peopleCount
            });
            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
            
        }
    }
}
