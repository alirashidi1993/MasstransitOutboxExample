namespace OutboxExample.Contracts.MessageContracts
{
    public record ReservationSubmitted
    {
        public Guid ReservationId { get; init; }
        public Guid CustomerId { get; init; }
        public int TableNo { get; init; }
        public DateTime StartFrom { get; init; }
        public DateTime EndIn { get; init; }
        public int PeopleCount { get; init; }
    }
}
