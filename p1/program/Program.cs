using P1.App;
using P1.Data;
using P1.Logic;
using System;
using System.Net.Mail;

public class Program
{
    public static void Main(String[] args)
    {

        string ConnectionString = File.ReadAllText(@"/Users/pisit/Desktop/SchoolFolder/WorkRevature/ConnectionStrings/P1ConnectionString.txt");

        IRepository Repo = new SqlRepository(ConnectionString);

        TicketSystem System1 = new TicketSystem(Repo);

        User currentuser = System1.LoginOrRegister();
        Console.WriteLine();
    }
}
//?We shall be testing implementing an API God please be by my side



