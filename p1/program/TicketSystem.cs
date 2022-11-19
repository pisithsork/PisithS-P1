using P1.Data;
using P1.Logic;
using P1.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace P1.App
{
    public class TicketSystem
    {
        //Fields
        IRepository Repo;
        //Validation validate = new Validation();

        //Constructors
        public TicketSystem() { }

        public TicketSystem(IRepository repo)
        {
            this.Repo = repo;
        }
        
        //Methods

        public User LoginOrRegister()
        {
            User currentuser = new User();
            bool toContinue = true;
            while (toContinue)
            {
                Console.WriteLine("Press the following Options\n[1] Login\n[2] Register");
                string entryInput = Console.ReadLine();
                if (!(string.IsNullOrEmpty(entryInput)) && (entryInput == "1" || entryInput == "2"))
                {
                    if (entryInput == "2")
                    {
                        getRegistration();
                    }

                    else if (entryInput == "1")
                    {
                        currentuser = getLogin();
                        toContinue = false;
                        
                    }
                    else
                    {
                        Console.WriteLine("Please enter a VALID input!");
                    }
                }
            }
            return currentuser;
        }

        public void getRegistration()
        {
            bool isValidEmail = true;
            while (isValidEmail)
            {
                User newUser = ConsoleMenu.getRegistrationInput();
                if (! Repo.doesEmailExist(newUser.Email))
                {
                    isValidEmail = false;
                    Repo.AddNewUser(newUser);
                    System.Console.WriteLine("Registration was successful!\n" +
                            "You will be prompt back to the previous menu.");
                    System.Console.WriteLine();
                }
                else
                {
                    System.Console.WriteLine("That email you entered already exist. You will be prompt to the previous menu");
                    System.Console.WriteLine();
                    isValidEmail = false;
                }
            }
        }
        public User getLogin()
        {
            bool loops = true;
            User currentuser = new User();
            while (loops)
            {
                var UserInput = ConsoleMenu.getUserInput();
                bool answer = Repo.isCredentialValid(UserInput.Item1, UserInput.Item2);
                if (answer)
                {
                    Console.WriteLine("Login Accepted!");
                    currentuser = Repo.GetCurrentUser(UserInput.Item1);
                    getMenuInput(currentuser);
                    loops = false;
                }
            }
            return currentuser;

        }

        public void getMenuInput(User currentuser)
        {
            bool LoggedIn = true;
            //If the current user is NOT a manager then display the following options
            if (!currentuser.isManager) { 
                do
                {
                    int curruserinput = ConsoleMenu.DisplayMenu(currentuser);
                    switch (curruserinput)
                    {
                        case 1:
                            Ticket newticket = ConsoleMenu.getTicketInput();
                            Repo.AddNewTicket(newticket, currentuser);
                            break;
                        case 2:
                            int tickettype = ConsoleMenu.GetTicketType();
                            IEnumerable<Ticket> usertickets = Repo.getUserTickets(currentuser, tickettype);
                            ConsoleMenu.DisplayTickets(usertickets);
                            break;
                        case 3:
                            LoggedIn = false;
                            return;
                        default:
                            Console.WriteLine("Enter a valid input");
                            break;
                    }
                } while (LoggedIn);
            }

            //Else being the only other choice of being a manager display the following options
            else
            {
                do
                {
                    int curruserinput = ConsoleMenu.DisplayMenu(currentuser);
                    switch (curruserinput)
                    {
                        case 1:
                            IEnumerable<Ticket> pendingtickets = Repo.getAllTickets(currentuser);
                            ConsoleMenu.DisplayTickets(pendingtickets);
                            Ticket updatedticket = ConsoleMenu.UpdateTicketInput(pendingtickets);
                            Repo.UpdateTicket(updatedticket);
                            break;
                        case 2:
                            IEnumerable<Ticket> usertickets = Repo.getAllTickets(currentuser);
                            ConsoleMenu.DisplayTickets(usertickets);
                            break;
                        case 3:
                            LoggedIn = false;
                            return;
                        default:
                            Console.WriteLine("Enter a valid input");
                            break;
                    }
                } while (LoggedIn);
            }
        }
    }
}
