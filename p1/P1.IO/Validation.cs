using P1.Data;
using System.Net;

namespace P1.IO
{
    public class Validation
    {
        //Fields
        Dictionary <string, string> Credentials = new Dictionary <string, string>();

        //Constructors
        public Validation () { }

        public Validation (Dictionary<string, string> credentials)
        {
            /*this.userName = newuser.getUserName();
            this.Password = newuser.getPassword();*/
            this.Credentials = credentials;
        }

        //Methods

        //I wanna method to check to see if email exist within the database(dictionary)
        public bool doesEmailExist(string userName)
        {
            if (Credentials.ContainsKey(userName))
            {
                return true;
            }
            return false;
        }

        //I wanna method to validate if the credentials entered exist within the database(dictionary)
        public bool isCredentialValid(string userName, string Password, Dictionary<string, string> credentials)
        {
            if (credentials.ContainsKey(userName))
            {
                if (credentials[userName] == Password)
                {
                    Console.WriteLine("Credentials Accepted!");
                    return true;
                }
                else
                {
                    Console.WriteLine("The email or password was incorrect please try again");
                    return false;
                }
            }
            else 
            {
                Console.WriteLine("The email or password was incorrect please try again");
                return false;
            }
        }
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
        //I wanna have a method to register new user
        public bool getRegistration()
        {
            bool isValidEmail = true;
            while (isValidEmail)
            {
                var UserInput = getUserInput();
                if (!doesEmailExist(UserInput.Item1))
                {
                    isValidEmail = false;
                    Credentials.Add(UserInput.Item1, UserInput.Item2);
                    return true;
                }
                else
                {
                    Console.WriteLine("Registration did not work end program and fix bug");
                    isValidEmail = false;
                }
            }
            return false;
        }
        //I wanna get a method to login an existing user
        public bool getLogin()
        {
            
            bool loops = true;
            while (loops)
            {
                var UserInput = getUserInput();
                bool answer = isCredentialValid(UserInput.Item1, UserInput.Item2, Credentials);
                if (answer)
                {
                    Console.WriteLine("Login Accepted!");
                    return true;
                }
            }
            return false;

        } 
    }
}