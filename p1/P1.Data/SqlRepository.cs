using P1.Logic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Net;

namespace P1.Data
{
    public class SqlRepository : IRepository
    {
        //Fields
        private string ConnectionString;

        //Constructors
        public SqlRepository() { }

        public SqlRepository(string connectionstring)
        {
            this.ConnectionString = connectionstring;
        }

        //Methods
        // I wanna make a method to call all users

        public List<User> GetAllUsers()
        {
            var users = new List<User>();
            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                    
                //                                   Index      0             1           2       3     4          5
                using SqlCommand Command = new("SELECT EmployeeId, FirstName, LastName, PW, Email, isManager FROM TicketSystem.Users;", Connection);
                Connection.Open();
                using SqlDataReader Reader = Command.ExecuteReader();

                while (Reader.Read())
                {
                    int EmployeeId = Reader.GetInt32(0);
                    string FirstName = Reader.GetString(1);
                    string LastName = Reader.GetString(2);
                    string PW = Reader.GetString(3);
                    string Email = Reader.GetString(4);
                    int isManager = Reader.GetByte(5);

                    // The 1 indicate it is a manager and the 0 indicate it is an employee
                    if(isManager == 1)
                    {
                        users.Add(new(EmployeeId, FirstName, LastName, PW, Email, true));
                    }
                    else
                    {
                        users.Add(new(EmployeeId, FirstName, LastName, PW, Email, false));
                    }

                }
                Reader.Close();
                Command.Dispose();
            }
            return users;
            

        }

        public bool doesEmailExist(string userName)
        {
            List<string> Emails = new List<string>();
            using SqlConnection Connection = new SqlConnection(ConnectionString);

            Connection.Open();

            using SqlCommand Command = new("SELECT Email FROM TicketSystem.Users", Connection);
            using SqlDataReader Reader = Command.ExecuteReader();

            while (Reader.Read())
            {
                string Email = Reader.GetString(0);
                if(userName == Email)
                {
                    return true;
                }
            }
            Reader.Close();
            return false;
        }

        //Refactor Dictionary to Database(Repo)
        public bool isCredentialValid(string userName, string Password)
        {
            using SqlConnection Connection = new SqlConnection(ConnectionString);

            Connection.Open();

            using SqlCommand Command = new("SELECT PW, Email FROM TicketSystem.Users", Connection);
            using SqlDataReader Reader = Command.ExecuteReader();

            while (Reader.Read())
            {
                string PW = Reader.GetString(0);
                string Email = Reader.GetString(1);

                if (userName == Email)
                {
                    if (Password == PW)
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
            }
            Connection.Close();
            Console.WriteLine("The email or password was incorrect please try again");
            return false;
        }

        public void AddNewUser(User newUser)
        {
            using SqlConnection Connection = new SqlConnection(ConnectionString);
            string CommandText = @"INSERT INTO TicketSystem.Users (FirstName, LastName, Email, PW, isManager) VALUES (@FirstName, @LastName, @Email, @Password, DEFAULT);";

            Connection.Open();

            using SqlCommand Command = new SqlCommand(CommandText, Connection);
            Command.Parameters.AddWithValue("@FirstName", newUser.FirstName);
            Command.Parameters.AddWithValue("@LastName", newUser.LastName);
            Command.Parameters.AddWithValue("@Email", newUser.Email);
            Command.Parameters.AddWithValue("@Password", newUser.Password);

            Command.ExecuteNonQuery();
            Connection.Close();

        }
    }
}