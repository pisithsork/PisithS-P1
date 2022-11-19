using P1.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P1.Data
{
    public interface IRepository
    {
        public bool doesEmailExist(string username);
        public bool isCredentialValid(string userName, string Password);
        public void AddNewUser(User newUser);
        public User GetCurrentUser(string currentuserEmail);
        public void AddNewTicket(Ticket newticket, User currentuser);
        public List<Ticket> getUserTickets(User currentuser, int tickettype);
        public List<Ticket> getAllTickets(User currentuser);
        public void UpdateTicket(Ticket updatedticket);

    }
}
