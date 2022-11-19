using P1.Data;
using P1.Logic;
using System.Net;
using System.Text;

namespace P1.IO
{
    /*!
     * P1.IO: "What is my Purpose?"
     * Me: "You read and write lines"
     * P1.IO: "Oh my God"
     * Me: "Yeah join the club"
    */
    public class ConsoleMenu
    {
        //Fields

        //Constructors

        //Methods
        public static (string,string) getUserInput()
        {
            string inputUsername = "";
            string inputPassword = "";
            bool isValid = true;
            bool isEmailUnique = true;

            while (isValid)
            {
                Console.WriteLine("Please enter your email");
                Console.Write("Email: ");
                inputUsername = Console.ReadLine();
                Console.Write("Password: ");
                inputPassword = Console.ReadLine();

                if(!(String.IsNullOrEmpty(inputUsername)) && !(String.IsNullOrEmpty(inputPassword)))
                {
                    isValid = false;
                }
                else
                {
                    Console.WriteLine("Please re-enter valid username AND password!");
                }
            }
            return (inputUsername, inputPassword);
        }

        public static User getRegistrationInput()
        {
            User user = new User();
            List<string> fullInput = new List<string> ();
            //!                                     INDEX          0               1           2              3
            List<string> outliner = new List<string> { "First Name", "Last Name", "Email", "Secure Password" };

            foreach(string input in outliner)
            {   
                bool loops = true;
                while (loops)
                {
                    Console.WriteLine($"Please enter your {input}");
                    Console.Write($"{input}: ");
                    string userInput = Console.ReadLine();

                    if (!String.IsNullOrEmpty(userInput))
                    {
                        fullInput.Add(userInput);
                        loops = false;
                    }
                    else
                    {
                        Console.WriteLine($"Please enter a valid {input}");
                    }
                }
            }
            user.FirstName = fullInput[0];
            user.LastName = fullInput[1];
            user.Email = fullInput[2];
            user.Password = fullInput[3];
            return user;
        }


        public static int DisplayMenu(User currentuser)
        {
            Console.Write($"Welcome back {currentuser.FirstName} {currentuser.LastName} \n");
            //If the user is NOT a manager printout the following options
            if (!currentuser.isManager) 
            {
                Console.WriteLine("Please select the following options:\n[1]. Create a Reimbursement Ticket \n[2]. View previous submitted tickets\n[3]. Log Out");
                bool isValid = true;
                while (isValid)
                {
                    string curruserinput = Console.ReadLine();
                    if (!(string.IsNullOrEmpty(curruserinput)) && (curruserinput == "1" || curruserinput == "2" || curruserinput == "3"))
                    {
                        isValid = false;
                        return Int32.Parse(curruserinput);
                    }
                    else
                    {
                        Console.WriteLine("Please enter a valid input");
                        Console.WriteLine("Please select the following options:\n[1]. Create a Reimbursement Ticket \n[2]. View previous submitted tickets\n[3]. Log Out");
                    }
                }
            }
            //Else which is the only other choice of being a manager then printout the following option
            else
            {
                Console.WriteLine("Please select the following options:\n[1]. Process pending tickets \n[2]. View pending submitted tickets\n[3]. Log Out");
                bool isValid = true;
                while (isValid)
                {
                    string curruserinput = Console.ReadLine();
                    if (!(string.IsNullOrEmpty(curruserinput)) && (curruserinput == "1" || curruserinput == "2" || curruserinput == "3"))
                    {
                        isValid = false;
                        return Int32.Parse(curruserinput);
                    }
                    else
                    {
                        Console.WriteLine("Please enter a valid input");
                        Console.WriteLine("Please select the following options:\n[1]. Process pending tickets \n[2]. View pending submitted tickets\n[3]. Log Out");
                    }
                }
            }


            Console.WriteLine("Ayyy bro you gone too far my boy Oh and you returning a -1 for DisplayMenu()");
            return -1;
        }

        // I wanna have a method that will get users to input new tickets
        public static Ticket getTicketInput()
        {
            Ticket newticket = new Ticket();
            List<string> fullInput = new List<string>();
            List<string> outliner = new List<string> { "Description", "Amount" };
            foreach (string input in outliner)
            {
                bool loops = true;
                while (loops)
                {
                    Console.WriteLine($"Please enter the {input}");
                    Console.Write($"{input}: ");
                    string userInput = Console.ReadLine();

                    if (!String.IsNullOrEmpty(userInput))
                    {
                        fullInput.Add(userInput);
                        loops = false;
                    }
                    else
                    {
                        Console.WriteLine($"Please enter a valid {input}");
                    }
                }
            }
            double amount = Convert.ToDouble(fullInput[1]);
            amount = Math.Round(amount, 2);
            newticket.Description = fullInput[0];
            newticket.Amount = amount;
            return newticket;

        }

        public static void DisplayTickets(IEnumerable<Ticket> usertickets)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("TicketID \t Description \t\t\t Amount \t Current Status \t Submitted At \t\t Completed At");
            sb.AppendLine("======== \t =========== \t\t\t ====== \t ============== \t ============ \t\t ============");
            foreach (Ticket ticket in usertickets)
            {
                string thisstring = String.Format("{0,-15}", ticket.TicketId);
                thisstring += String.Format("{0,-34}", ticket.Description);
                thisstring += String.Format("{0,-16:C}", ticket.Amount);
                thisstring += String.Format("{0,-20}", ticket.StatusofTicket);
                thisstring += String.Format("{0, -27}", ticket.SubmittedAt);
                thisstring += String.Format("{0, -12}", ticket.CompletedAt);
                sb.AppendLine(thisstring);
            }
            Console.WriteLine(sb);
        }
        //!If this does not break my shit I am amazing, and I can do this
        public static Ticket UpdateTicketInput(IEnumerable<Ticket> pendingtickets)
        {
            //User must input info of which ticket they want to update: TicketID and Status of Ticket
            List<int> listofticketsid = new List<int>();
            Ticket updateticket = new Ticket();
            foreach(Ticket ticket in pendingtickets)
            {
                listofticketsid.Add(ticket.TicketId);
            }
            int ticketid = -1;
            bool isValid = true;
            while (isValid)
            {   Console.WriteLine("Enter the TicketID that you wish to update");
                Console.Write("Ticket ID: ");
                string ticketidstring = Console.ReadLine();
                if (!(String.IsNullOrEmpty(ticketidstring)))
                {
                    ticketid = Int32.Parse(ticketidstring);
                    if (listofticketsid.Contains(ticketid))
                    {
                        isValid = false;
                        int index = listofticketsid.IndexOf(ticketid);
                        updateticket = pendingtickets.ElementAt(index);
                    }
                    else
                    {
                        Console.WriteLine("Please enter a valid Ticket ID to update");
                    }
                }
                else
                {
                    Console.WriteLine("Please enter a valid Ticket ID to update");
                }
            }
            Console.WriteLine("Select [1] to Approve the ticket or [2] to Deny the ticket");
            int selectinput = Int32.Parse(Console.ReadLine());
            switch(selectinput)
            {
                case 1:
                    updateticket.StatusofTicket = "APPROVED";
                    return updateticket;
                case 2:
                    updateticket.StatusofTicket = "DENIED";
                    return updateticket;
                default:
                    Console.WriteLine("Please enter 1 or 2");
                    break;
            }
            Console.WriteLine("Oops wrong way, turn it back around");
            return updateticket;
        }


        //I wanna have a method to decide if user wants to get specific types of tickets based on the status of the tickets
        public static int GetTicketType()
        {
            bool isValid = true;
            int num = -1;
            isValid = true;
            while (isValid) {
                Console.WriteLine("Select from the following of which ticket you want to view:\n[1]. Pending\n[2]. Approved\n[3]. Denied\n[4]. All");
                string returnvalue = Console.ReadLine();
                if (!(String.IsNullOrEmpty(returnvalue)))
                {
                    if(returnvalue == "1" || returnvalue == "2" || returnvalue == "3" || returnvalue == "4") {
                        num = Int32.Parse(returnvalue);
                        isValid = false;
                        return num;
                    }
                    else
                    {
                        Console.WriteLine("Please enter a valid input");
                    }
                }
            }
            Console.WriteLine("Error, reached outside of while loop in GetTicketType()");
            return num;
        }
               
    }
}