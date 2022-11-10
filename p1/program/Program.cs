using P1.Data;
using P1.IO;
using System;

public class Program
{
    //! NOTES:
    //! 1. The login and register method works fine IF I input the value correct the first time BUT If say I user input incorrectly the first time, it loops to asking the email and password from user over and over
    //! 2. Implement a way to user database and sql IF it is possible based on the current state of the program. There would be no reason to implement something that the foundation of the structure has not been made yet
    //! 3. Use the Dictionary to create a database and refractor all use of dictionary
    //! 4. Create a LoginOrRegister() Method in Validation.cs class that outputs either 1 or 2 to decide if user will login or register
    public static void Main(String[] args)
    {
        /* We are creating a login/register program, one that is stable enough to be able to fall back to
         *  it is not a necessity to create it as efficient as possible because it is only to be meant as
         *  as a state of the program to fall back to if ever need to redo and rework testing programs
         */
        string userName = "pisithsork@yahoo.com";
        string password = "123456";
        bool looping = true;
        Dictionary<string, string> tmp = new Dictionary<string, string>();
        tmp.Add(userName, password);
        Validation Credentials = new Validation(tmp);

        //Create a prompt for user to either login or register
        //Checks to see if user input is valid of either 1 or 2
        while (looping)
        {
            //Prompts user to input of either logging in or registering a new account
            Console.WriteLine("Press the following Options\n[1] Login\n[2] Register");
            string entryInput = Console.ReadLine();
            //As long as the input is not null or empty and if it is either "1" or "2" otherwise prompt to input again
            if (!(string.IsNullOrEmpty(entryInput)) && (entryInput == "1" || entryInput == "2"))
            {
                //Prompts user to register with a valid email and a secure password
                if (Int32.Parse(entryInput) == 2)
                {
                    bool isregistered = Credentials.getRegistration();

                    if (isregistered)
                    {
                        Console.WriteLine("Registration was successful!\n" +
                            "You will be prompt back to the previous menu.");
                    }
                    else
                    {
                        Console.WriteLine("Registration was unsuccessful you will be prompted to the previous menu");
                    }
                }
                //Prompts the user to continue forward with the login
                else
                {
                    looping = false;
                    continue;
                }
            }
            //If the user input is invalid have the user to input correct values
            else
            {
                Console.WriteLine("Please enter a valid input");
            }
        }
        looping = true;
        Console.WriteLine("Welcome please enter your username and password");

            //This is the user login prompt to have user input their valid email and password
            while (looping)
            {
                bool islogin = Credentials.getLogin();
            if (islogin)
            {
                Console.WriteLine("Login attempt was Successful");
                Console.WriteLine("GoodBye for now. I'm still building this app so hold your horses. But good job Pisith!");
                looping = false;
            }
            else
            {
                Console.WriteLine("Login was unsuccessful. You will be prompted to the previous menu");
            }
        }
    }
}