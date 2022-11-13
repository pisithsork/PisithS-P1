using P1.App;
using P1.Data;
//using P1.IO;
using System;
using System.Net.Mail;

public class Program
{
    //! NOTES:
    //! Complete in Validation.cs file the getRegistrationInput()
    //! Create the AddNewUser in SqlRepository.cs
    //! Test ALL NEW MOVED AND CREATED FUNCTION to ensure proper refactoring.
    public static void Main(String[] args)
    {

        string ConnectionString = File.ReadAllText(@"/Users/pisit/Desktop/SchoolFolder/WorkRevature/ConnectionStrings/P1ConnectionString.txt");

        IRepository Repo = new SqlRepository(ConnectionString);

        TicketSystem System1 = new TicketSystem(Repo);

        Console.WriteLine(System1.DisplayUser());
        Console.WriteLine();

        System1.LoginOrRegister();
        Console.WriteLine();

        Console.WriteLine(System1.DisplayUser());
        Console.WriteLine();





        //!Once testing is done uncomment this
        //Credentials.LoginOrRegister();


    }
}