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
        Validation validate = new Validation();

        //Constructors
        public TicketSystem() { }

        public TicketSystem(IRepository repo)
        {
            this.Repo = repo;
        }
        
        //Methods
        public string DisplayUser ()
        {
            StringBuilder StringBuilder = new StringBuilder();
            IEnumerable<User> users = Repo.GetAllUsers();
            foreach(User user in users)
            {
                StringBuilder.AppendLine(user.GetUserInformation());
            }
            return StringBuilder.ToString();

        }

        public void LoginOrRegister()
        {
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
                        getLogin();
                        toContinue = false;
                    }
                    else
                    {
                        Console.WriteLine("Please enter a VALID input!");
                    }
                }
            }
        }

        public void getRegistration()
        {
            bool isValidEmail = true;
            while (isValidEmail)
            {
                User newUser = validate.getRegistrationInput();
                if (! Repo.doesEmailExist(newUser.Email))
                {
                    isValidEmail = false;
                    Repo.AddNewUser(newUser);
                    Console.WriteLine("Registration was successful!\n" +
                            "You will be prompt back to the previous menu.");
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("That email you entered already exist. You will be prompt to the previous menu");
                    Console.WriteLine();
                    isValidEmail = false;
                }
            }
        }
        public void getLogin()
        {

            bool loops = true;
            while (loops)
            {
                var UserInput = validate.getUserInput();
                bool answer = Repo.isCredentialValid(UserInput.Item1, UserInput.Item2);
                if (answer)
                {
                    Console.WriteLine("Login Accepted!");
                    loops = false;
                }
            }

        }


    }
}
