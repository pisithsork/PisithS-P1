using P1.Data;
using P1.Logic;
using System.Net;

namespace P1.IO
{
    /*!
     * P1.IO: "What is my Purpose?"
     * Me: "You read and write lines"
     * P1.IO: "Oh my God"
     * Me: "Yeah join the club"
    */
    public class Validation
    {
        //Fields

        //Constructors
        public Validation () { }

        //Methods
        //I wanna method to take in Valid Input
        public (string,string) getUserInput()
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

        public User getRegistrationInput()
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
    }
}