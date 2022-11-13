using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P1.Logic
{
    public class Ticket
    {

        //Fields
        public string Description { get; set; }
        public double Amount { get; set; }
        public bool StatusofTicket { get; set; }
        public string TicketId { get; set; }

        //Constructors
        public Ticket() { }


        public Ticket(string description, double amount, bool statusOfticket, string ticketid)
        {
            this.Description = description;
            this.Amount = amount;
            this.StatusofTicket = statusOfticket;
            this.TicketId = ticketid;
        }

        //Methods


    }
}
