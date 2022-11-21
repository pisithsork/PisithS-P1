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

        //User currentuser = System1.LoginOrRegister();
    }
}



