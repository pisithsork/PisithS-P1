

public class Program
{

    public static void Main(String[] args)
    {

        /* We are creating a login/register program, one that is stable enough to be able to fall back to
         *  it is not a necessity to create it as efficient as possible because it is only to be meant as
         *  as a state of the program to fall back to if ever need to redo and rework testing programs
         */

        // test case
        string userName = "pisithsork@yahoo.com";
        string password = "123456";
        //string value = "";
        bool looping = true;
        Dictionary<string, string> Credentials = new Dictionary<string, string>();
        Credentials.Add(userName, password);

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
                //!Create a way to have it check with the Dictionary if the email exist already or not
                if (Int32.Parse(entryInput) == 2)
                {
                    Console.WriteLine("Please input a valid email");
                    string newuserName = Console.ReadLine();
                    Console.WriteLine("Please input a secure password");
                    string newpassword = Console.ReadLine();

                    //Once entered it is entered into the database to be used as well as ending this while loop
                    Credentials.Add(newuserName, newpassword);
                    looping = false;
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
            //Prompts user to input their username(email) and password associated with it
            Console.Write("Username: ");
            string userInput1 = Console.ReadLine();
            Console.Write("Password: ");
            string userInput2 = Console.ReadLine();

            //Based on whether the input is valid will perform one of the if statements
            //!Recreate the if statements in order to have it based off of the dictionary
            //Nested if statement of the outside loop to check if the username exist

            // if (Credentials[userInput1] == userInput2)
            
            if (Credentials.ContainsKey(userInput1) && Credentials[userInput1] == userInput2)
            {

                Console.WriteLine("Credential Accepted!");
                looping = false;

            }
            else if ((!(Credentials.ContainsKey(userInput1)) && Credentials[userInput1] == userInput2) || ((Credentials.ContainsKey(userInput1)) && Credentials[userInput1] != userInput2))
            {


                Console.WriteLine("The username or password you entered was invalid please try again");
            }
            else
            {

                Console.WriteLine("That username seems to not be registered. Would you like to register it?");
                looping = false;

            }
        }
    }
}