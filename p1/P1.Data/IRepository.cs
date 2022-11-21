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
        public List<User> GetAllUsers();
        public bool doesEmailExist(string username);
        public int isCredentialValid(User user);
        public void AddNewUser(User newUser);
        public User GetCurrentUser(string currentuserEmail);
        public Ticket AddNewTicket(Ticket newticket);
        public List<Ticket> getUserTickets(User currentuser);
        public List<Ticket> getPendingTickets();
        public void UpdateTicket(Ticket updatedticket);
        public List<string> getAllEmail();
        public List<Ticket> PendingUserTickets(User currentuser);
        public List<Ticket> ApprovedUserTickets(User currentuser);
        public List<Ticket> DeniedUserTickets(User currentuser);


    }
}
