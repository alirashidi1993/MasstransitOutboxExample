namespace OutboxExample.Contracts.Persistence.Models
{
    public class Reservation
    {
        protected Reservation() { }
        public Reservation(Guid customerId,int tableNo,DateTime startFrom,DateTime endIn,int peopleCount)
        {
            Id = Guid.NewGuid();
            CustomerId = customerId;
            TableNo = tableNo;
            StartFrom = startFrom;
            EndIn = endIn;
            PeopleCount = peopleCount;
        }
        public  Guid Id { get; private set; }
        public Guid CustomerId { get;private  set; }
        public int TableNo { get; private set; }
        public DateTime StartFrom { get; private set; }
        public DateTime EndIn { get; private set; }
        public int PeopleCount { get; private set; }
    }
}
