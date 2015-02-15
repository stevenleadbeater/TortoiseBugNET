namespace TurtleBugNET
{
    public class TicketItem
    {
        private readonly int _ticketNumber;
        private readonly string _ticketSummary;
        private readonly string _ticketStatus;

        public TicketItem(int ticketNumber, string ticketSummary, string ticketStatus)
        {
            _ticketNumber = ticketNumber;
            _ticketSummary = ticketSummary;
            _ticketStatus = ticketStatus;
        }

        public int Number
        {
            get { return _ticketNumber; }
        }

        public string Summary
        {
            get { return _ticketSummary; }
        }

        public string Status
        {
            get { return _ticketStatus; }
        }
    }
}