
using P1.Logic;
using System;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Text;

namespace P1.Client
{

    class Program
    {
        static HttpClient client = new HttpClient();

        static void Main()
        {
            Runit().GetAwaiter().GetResult();
        }

        static async Task Runit()
        {
            client.BaseAddress = new Uri("https://localhost:7284/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                //We need to first decide if the user will either login or register
                User currentuser = await LoginorRegister();

                //Menu Display for either a manager or a employee
                Console.Clear();

                if (!currentuser.isManager)
                {
                    int curruserinput = -1;
                    bool toContinue = true;
                    bool LoggedIn = true;
                    //Display Employee menu
                    do
                    {
                        while (toContinue)
                        {
                            Console.WriteLine("Please select the following options:\n[1]. Create New tickets \n[2]. View pending submitted tickets\n[3]. Log Out");
                            string userinput = Console.ReadLine();
                            if (!(String.IsNullOrEmpty(userinput)))
                            {
                                curruserinput = Int32.Parse(userinput);
                                toContinue = false;
                            }
                            else
                            {
                                Console.WriteLine("Please enter a valid input");
                            }
                        }
                        toContinue = true;
                        switch (curruserinput)
                        {
                            // Employee creates new ticket and displays the URL
                            case 1:
                                Console.Clear();
                                Ticket newticket = getTicketInput();
                                newticket.EmployeeId = currentuser.EmployeeId;
                                var Uri = await AddNewTicketAsync(newticket);
                                Console.WriteLine($"Ticket submitted at {Uri}");
                                break;
                            //User filters which ticket to view
                            case 2:
                                /*int tickettype = GetTicketType();*/
                                IEnumerable<Ticket> usertickets = await GetTicketType(currentuser);
                                Console.Clear();
                                /*IEnumerable<Ticket> usertickets = await getUserTicketsAsync(currentuser);*/
                                DisplayTickets(usertickets);
                                break;
                            case 3:
                                System.Environment.Exit(0);
                                break;
                            default:
                                Console.WriteLine("Enter a valid input");
                                break;
                        }
                    } while (LoggedIn);
                }
                else
                {
                    //display Manager menu
                    bool LoggedIn = true;
                    do
                    {
                        int curruserinput = -1;
                        bool isValid = true;
                        while (isValid)
                        {
                            Console.WriteLine("Please select the following options:\n[1]. Process pending tickets \n[2]. View pending submitted tickets\n[3]. Log Out");
                            string stringinput = Console.ReadLine();
                            if (!(string.IsNullOrEmpty(stringinput)))
                            {
                                isValid = false;
                                curruserinput = Int32.Parse(stringinput);
                            }
                            else
                            {
                                Console.WriteLine("Please enter a valid input");
                            }
                        }
                        isValid = true;
                        switch (curruserinput)
                        {
                            case 1:
                                Console.Clear();
                                IEnumerable<Ticket> pendingtickets = await getPendingTicketsAsync();
                                DisplayTickets(pendingtickets);

                                Ticket updatedticket = UpdateTicketInput(pendingtickets);
                                await UpdateTicketAsync(updatedticket);
                                Console.WriteLine($"Ticket {updatedticket.TicketId} has been updated to {updatedticket.StatusofTicket}");
                                Console.WriteLine("Press any key to continue");
                                Console.ReadKey();
                                Console.Clear();
                                break;
                            case 2:
                                IEnumerable<Ticket> usertickets = await getPendingTicketsAsync();
                                Console.Clear();
                                DisplayTickets(usertickets);
                                break;
                            case 3:
                                System.Environment.Exit(0);
                                return;
                            default:
                                Console.WriteLine("Enter a valid input");
                                break;
                        }
                    } while (LoggedIn);

                }
            }
            catch(Exception e)
            {
                Console.WriteLine("An error has occurred, throwing exception: " + e);
            }

        }




        static void DisplayUser(User user)
        {
            string Title;
            if(user.isManager = true)
            {
                Title = "Manager";
            }
            else
            {
                Title = "Employee";
            }
            Console.WriteLine($"{user.FirstName} {user.LastName} \n Employee ID: {user.EmployeeId} \n Title: {Title}");
            Console.WriteLine();

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

        static async Task<List<User>> GetAllUsersAsync()
        {
            List<User> users = new List<User>();
            var findpath = "getuser";
            HttpResponseMessage response = await client.GetAsync(findpath);
            if (response.IsSuccessStatusCode)
            {
                users = await response.Content.ReadAsAsync<List<User>>();
                           }
            return users;
        }

        static async Task<List<string>> GetAllEmailAsync()
        {
            List<string> listofEmail = new List<string>();
            var findpath = "getemails";
            HttpResponseMessage response = await client.GetAsync(findpath);
            if (response.IsSuccessStatusCode)
            {
                listofEmail = await response.Content.ReadAsAsync<List<string>>();
            }
            return listofEmail;
        }

        static async Task<List<Ticket>> GetAllTicketsASync()
        {
            List<Ticket> tickets = new List<Ticket>();
            var findpath = "gettickets";
            HttpResponseMessage response = await client.GetAsync(findpath);
            if (response.IsSuccessStatusCode)
            {
                tickets = await response.Content.ReadAsAsync<List<Ticket>>();
            }
            return tickets;
        }

        static async Task<int> GetValidCredentialsAsync(User currentuser)
        {
            int isValid = -1;
            var findpath = ($"getvalid/{currentuser.EmployeeId}");
            HttpResponseMessage response = await client.GetAsync(findpath);
            if (response.IsSuccessStatusCode)
            {
                isValid = await response.Content.ReadAsAsync<int>();
            }
            return isValid;
        }

        static async Task<User> GetUserAsync(string username)
        {
            username.Replace("@", "%40");
            User currentuser = new User();
            var response = await client.PostAsJsonAsync($"getlogin/?username={username}", currentuser);
            if (response.IsSuccessStatusCode)
            {
                currentuser = await response.Content.ReadAsAsync<User>();
            }
            return currentuser;
        }

        static async Task<bool> doesEmailExistAsync(string email)
        {
            if (email.Contains("@"))
            {
                email.Replace("@", "%40");
            }
            bool returnbool = false;
            var findpath = ($"emailexist/?username={email}");
            HttpResponseMessage response = await client.GetAsync(findpath);
            if (response.IsSuccessStatusCode)
            {
                returnbool = await response.Content.ReadAsAsync<bool>();
            }
            return returnbool;
        }

        static async Task<User> CreateNewUserAsync(User newuser)
        {
            
            var response = await client.PostAsJsonAsync("getregister", newuser);
            User user = await response.Content.ReadAsAsync<User>();
            //response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                user = await response.Content.ReadAsAsync<User>();
            }
            return user;
        }

        static async Task<Uri> AddNewTicketAsync(Ticket newticket)
        {
            var response = await client.PostAsJsonAsync("ticket", newticket);
            response.EnsureSuccessStatusCode();
            return response.Headers.Location;
        }

        static async Task<List<Ticket>> getPendingTicketsAsync(User currentuser)
        {
            List<Ticket> pendingtickets = new List<Ticket>();
            var response = await client.PostAsJsonAsync($"pendingtickets/{currentuser.EmployeeId}", currentuser);
            if (response.IsSuccessStatusCode)
            {
                pendingtickets = await response.Content.ReadAsAsync<List<Ticket>>();
            }
            return pendingtickets;
        }

        static async Task<List<Ticket>> getApprovedTicketsAsync(User currentuser)
        {
            List<Ticket> approvedtickets = new List<Ticket>();
            var response = await client.PostAsJsonAsync($"approvedtickets/{currentuser.EmployeeId}", currentuser);
            if (response.IsSuccessStatusCode)
            {
                approvedtickets = await response.Content.ReadAsAsync<List<Ticket>>();
            }
            return approvedtickets;
        }

        static async Task<List<Ticket>> getDeniedTicketsAsync(User currentuser)
        {
            List<Ticket> deniedtickets = new List<Ticket>();
            var response = await client.PostAsJsonAsync($"deniedtickets/{currentuser.EmployeeId}", currentuser);
            if (response.IsSuccessStatusCode)
            {
                deniedtickets = await response.Content.ReadAsAsync<List<Ticket>>();
            }
            return deniedtickets;
        }

        static async Task<List<Ticket>> getUserTicketsAsync(User currentuser)
        {
            List<Ticket> tickets = new List<Ticket>();
            var response = await client.PostAsJsonAsync($"alltickets/{currentuser.EmployeeId}", currentuser);
            if (response.IsSuccessStatusCode)
            {
                tickets = await response.Content.ReadAsAsync<List<Ticket>>();
            }
            return tickets;
        }


        //Gets all pending tickets
        static async Task<List<Ticket>> getPendingTicketsAsync()
        {
            List<Ticket> tickets = new List<Ticket>();
            var findpath = "allpendingtickets";
            HttpResponseMessage response = await client.GetAsync(findpath);
            if (response.IsSuccessStatusCode)
            {
                tickets = await response.Content.ReadAsAsync<List<Ticket>>();
            }
            return tickets;
        }

        static async Task<Uri> UpdateTicketAsync(Ticket updatedticket)
        {
            var response = await client.PutAsJsonAsync("updateticket", updatedticket);
            response.EnsureSuccessStatusCode();
            return response.Headers.Location;
        }



        public static async Task<User> LoginorRegister()
        {
            User currentuser = new User();
            bool toContinue = true;
            while (toContinue)
            {
                Console.WriteLine("Welcome to this program, Please select the following option:\n[1]. Login\n[2]. Register\n[3]. Exit");
                string input = Console.ReadLine();

                if (String.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Please enter a valid input");
                    Console.WriteLine("Please select the following option:\n[1]. Login\n[2]. Register\n[3]. Exit");
                }
                else
                {
                    if (input != "1" && input != "2" && input != "3")
                    {
                        Console.WriteLine("Please enter a valid input");
                        Console.WriteLine("Please select the following option:\n[1]. Login\n[2]. Register\n[3]. Exit");
                    }
                    else
                    {
                        //Registration occurs here
                        if (input == "1")
                        {
                            //user will input values to login
                            return currentuser = await getLogin();
                            toContinue = false;
                            Console.Clear();
                        }
                        else if (input == "2")
                        {
                            //user will register then is logged in
                            currentuser = await getRegistration();
                            //Console.Clear();
                        }
                        else if(input == "3")
                        {
                            System.Environment.Exit(0);
                        }
                        else
                        {
                            Console.WriteLine("Please enter a valid input");
                        }//3rd if-else end  

                    }//2nd if-else end

                }//1st if-else end

            }//while loop end
            Console.WriteLine("Error Returning empty user object");
            return currentuser;
        }

        public static User getRegistrationInput()
        {
            User user = new User();
            List<string> fullInput = new List<string>();
            //!                                     INDEX          0               1           2              3
            List<string> outliner = new List<string> { "First Name", "Last Name", "Email", "Secure Password" };

            foreach (string input in outliner)
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
            user.FirstName = fullInput[0].ToString();
            user.LastName = fullInput[1].ToString();
            user.Email = fullInput[2].ToString();
            user.Password = fullInput[3].ToString();
            return user;
        }

        static async Task<User> getLogin()
        {
            User currentuser = new User();
            string inputUsername = "";
            string inputPassword = "";
            int employeeid = -1;
            bool isValid = true;
            bool toContinue = true;
            while (isValid)
            {
                while (toContinue)
                {
                    Console.WriteLine("Please enter your email");
                    Console.Write("Email: ");
                    inputUsername = Console.ReadLine();
                    if (!(String.IsNullOrEmpty(inputUsername)))
                    {
                        currentuser = await GetUserAsync(inputUsername);
                    }
                    else
                    {
                        Console.WriteLine("The email you entered is incorrect, please try again");
                    }
                    if (currentuser.Email == inputUsername)
                    {
                        toContinue = false;
                    }
                    else
                    {
                        Console.WriteLine("The email you entered is incorrect, please try again");
                    }
                }
                Console.Write("Password: ");
                inputPassword = Console.ReadLine();

                if (!(String.IsNullOrEmpty(inputPassword)) && inputPassword == currentuser.Password)
                {
                    return currentuser;
                    isValid = false;
                }
                else
                {
                    Console.WriteLine("The password you entered is incorrect, please try again");
                }
            }
            Console.WriteLine("Error returning empty user");
            return currentuser;
        }

        public static async Task<User> getRegistration()
        {
            

            bool isValidEmail = true;
            while (isValidEmail)
            {
                User newUser = getRegistrationInput();
                List<string> listofEmail = await GetAllEmailAsync();
                if (!listofEmail.Contains(newUser.Email))
                {
                    isValidEmail = false;
                    User currentuser = await CreateNewUserAsync(newUser);
                    System.Console.WriteLine("Registration was successful!\n" +
                            "You will be prompt back to the previous menu.");
                    System.Console.WriteLine("Press any key to Continue");
                    Console.ReadKey();
                }
                else
                {
                    System.Console.WriteLine("That email you entered already exist. Please re-enter your registration with a new email");
                    System.Console.WriteLine();
                }
            }
            User emptyuser = new User();
            return emptyuser;
        }

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

        public static async Task<IEnumerable<Ticket>> GetTicketType(User currentuser)
        {
            bool isValid = true;
            int num = -1;
            isValid = true;
            List<Ticket> listofTickets = new List<Ticket>();
            while (isValid)
            {
                Console.WriteLine("Select from the following of which ticket you want to view:\n[1]. Pending\n[2]. Approved\n[3]. Denied\n[4]. All");
                string returnvalue = Console.ReadLine();
                if (!(String.IsNullOrEmpty(returnvalue)))
                {
                    num = Int32.Parse(returnvalue);
                    switch (num)
                    {
                        case 1:
                            //get pending tickets
                            return listofTickets = await getPendingTicketsAsync(currentuser);
                            break;
                        case 2:
                            //get approved tickets
                            return listofTickets = await getApprovedTicketsAsync(currentuser);
                            break;
                        case 3:
                            //get denied tickets
                            return listofTickets = await getDeniedTicketsAsync(currentuser);
                            break;
                        case 4:
                            //get all tickets
                            return listofTickets = await getUserTicketsAsync(currentuser);
                            break;
                        default:
                            Console.WriteLine("Please enter a valid input");
                            break;
                    }
                }
            }
            Console.WriteLine("Error, reached outside of while loop in GetTicketType()");
            return listofTickets;
        }

        public static Ticket UpdateTicketInput(IEnumerable<Ticket> pendingtickets)
        {
            //User must input info of which ticket they want to update: TicketID and Status of Ticket
            List<int> listofticketsid = new List<int>();
            Ticket updateticket = new Ticket();
            foreach (Ticket ticket in pendingtickets)
            {
                listofticketsid.Add(ticket.TicketId);
            }
            int ticketid = -1;
            bool isValid = true;
            while (isValid)
            {
                Console.WriteLine("Enter the TicketID that you wish to update");
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
            switch (selectinput)
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


    }

}