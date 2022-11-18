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
        public string StatusofTicket { get; set; }
        public int TicketId { get; set; }
        public int EmployeeId { get; set; }

        //Constructors
        public Ticket() { }


        public Ticket(string description, double amount, string statusOfticket, int ticketid, int employeeid)
        {
            this.Description = description;
            this.Amount = amount;
            this.StatusofTicket = statusOfticket;
            this.TicketId = ticketid;
            this.EmployeeId = employeeid;
        }
        //used to submit tickets
        public Ticket(string description, double amount, int Employeeid)
        {
            this.Description = description;
            this.Amount = amount;
            StatusofTicket = "PENDING";
            this.EmployeeId = Employeeid;
        }

        //Methods


    }
}
