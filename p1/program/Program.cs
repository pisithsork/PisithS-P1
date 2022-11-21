using P1.App;
using P1.Data;
using P1.Logic;
using System;
using System.Net.Mail;

public class Program
{
    public static void Main(String[] args)
    {

        //!Let this be a guide to building our client, follow the function and anywhere there is some need for inputs from the user, depending on if the input leads to specific functions that needs to be called or are just simply inputs that leads to those function that needs to be called


        string ConnectionString = File.ReadAllText(@"/Users/pisit/Desktop/SchoolFolder/WorkRevature/ConnectionStrings/P1ConnectionString.txt");

        IRepository Repo = new SqlRepository(ConnectionString);

        TicketSystem System1 = new TicketSystem(Repo);

        //User currentuser = System1.LoginOrRegister();
        Console.WriteLine();
    }
}
//?We shall be testing implementing an API God please be by my side



