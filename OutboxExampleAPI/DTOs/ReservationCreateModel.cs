namespace OutboxExampleAPI.DTOs
{
    public class ReservationCreateModel
    {
        public Guid CustomerId { get; set; }
        public int TableNo { get; set; }
        public DateTime StartFrom { get; set; }
        public DateTime EndIn { get; set; }
        public int PeopleCount { get; set; }
    }
}
