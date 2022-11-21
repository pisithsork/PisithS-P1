using P1.Logic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Net;

namespace P1.Data
{
    public class SqlRepository : IRepository
    {
        //Fields
        private string ConnectionString = File.ReadAllText(@"/Users/pisit/Desktop/SchoolFolder/WorkRevature/ConnectionStrings/P1ConnectionString.txt");

        //Constructors
        public SqlRepository() { }

        public SqlRepository(string connectionstring)
        {
            this.ConnectionString = connectionstring;
        }

        //Methods

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
                    if (isManager == 1)
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

        public User GetCurrentUser(string currentuserEmail)
        {
            User currentuser = new User();
            using SqlConnection Connection = new SqlConnection(ConnectionString);
            string CommandText = new("SELECT * FROM TicketSystem.Users WHERE Email = @Email;");
            
            Connection.Open();
            using SqlCommand Command = new SqlCommand(CommandText, Connection);
            Command.Parameters.AddWithValue("@Email", currentuserEmail);

            using SqlDataReader Reader = Command.ExecuteReader();
            while (Reader.Read())
            {
                currentuser.EmployeeId = Reader.GetInt32(0);
                currentuser.FirstName = Reader.GetString(1);
                currentuser.LastName = Reader.GetString(2);
                currentuser.Password= Reader.GetString(3);
                currentuser.Email = Reader.GetString(4);
                int isManager = Reader.GetByte(5);
                if(isManager == 1)
                {
                    currentuser.isManager = true;
                }
                else
                {
                    currentuser.isManager = false;
                }
            }
            Reader.Close();
            return currentuser;
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

        public List<string> getAllEmail()
        {
            List<string> Emails = new List<string>();
            using SqlConnection Connection = new SqlConnection(ConnectionString);

            Connection.Open();

            using SqlCommand Command = new("SELECT Email FROM TicketSystem.Users", Connection);
            using SqlDataReader Reader = Command.ExecuteReader();

            while (Reader.Read())
            {
                Emails.Add(Reader.GetString(0).ToString());
            }
            Reader.Close();
            Connection.Close();
            return Emails;
        }


        public int isCredentialValid(User currentuser)
        {
            using SqlConnection Connection = new SqlConnection(ConnectionString);
            int returnvalue = -1;

            Connection.Open();

            using SqlCommand Command = new("SELECT EmployeeId, PW, Email FROM TicketSystem.Users", Connection);
            using SqlDataReader Reader = Command.ExecuteReader();

            while (Reader.Read())
            {
                int employeeid = Reader.GetInt32(0);
                string PW = Reader.GetString(1);
                string Email = Reader.GetString(2);

                if (currentuser.Email == Email)
                {
                    if (currentuser.Password == PW)
                    {
                        Console.WriteLine("Credentials Accepted!");
                        returnvalue = employeeid;
                    }
                    else
                    {
                        Console.WriteLine("The email or password was incorrect please try again");
                    }
                }
            }
            Connection.Close();
            Command.Dispose();
            return returnvalue;
        }

        public void AddNewUser(User newuser)
        {
            using SqlConnection Connection = new SqlConnection(ConnectionString);
            string CommandText = @"INSERT INTO TicketSystem.Users (FirstName, LastName, Email, PW, isManager) VALUES (@FirstName, @LastName, @Email, @Password, DEFAULT);";

            Connection.Open();

            using SqlCommand Command = new SqlCommand(CommandText, Connection);
            Command.Parameters.AddWithValue("@FirstName", newuser.FirstName);
            Command.Parameters.AddWithValue("@LastName", newuser.LastName);
            Command.Parameters.AddWithValue("@Email", newuser.Email);
            Command.Parameters.AddWithValue("@Password", newuser.Password);

            Command.ExecuteNonQuery();
            Connection.Close();

        }

        public Ticket AddNewTicket(Ticket newticket)
        {
            using SqlConnection Connection = new SqlConnection(ConnectionString);
            string CommandText = @"INSERT INTO TicketSystem.Tickets (employeeid, Descriptions, Amount, StatusofTicket, SubmittedAt, CompletedAt) VALUES (@EmployeeId, @Description, @Amount, DEFAULT, DEFAULT, DEFAULT); SELECT @@IDENTITY from TicketSystem.Tickets;";
            Connection.Open();

            using SqlCommand Command = new SqlCommand(CommandText, Connection);
            Command.Parameters.AddWithValue("@EmployeeID", newticket.EmployeeId);
            Command.Parameters.AddWithValue("@Description", newticket.Description);
            Command.Parameters.AddWithValue("@Amount", newticket.Amount);
            int ticketid = Convert.ToInt32(Command.ExecuteScalar());
            newticket.TicketId = ticketid;
            Command.Dispose();
            Connection.Close();
            return newticket;
        }
        

        //Gets all user tickets
        public List<Ticket> getUserTickets(User currentuser)
        {
            var ticketList = new List<Ticket>();
            using SqlConnection Connection = new SqlConnection(ConnectionString);
            SqlCommand Command = new SqlCommand();
            Connection.Open();
            Command = new(@"SELECT * FROM TicketSystem.Tickets WHERE employeeid = @EmployeeId", Connection);
            Command.Parameters.AddWithValue("@EmployeeId", currentuser.EmployeeId);
            using SqlDataReader Reader = Command.ExecuteReader();
            while (Reader.Read())
            {   Ticket usertickets = new Ticket();
                usertickets.TicketId = Reader.GetInt32(0);
                usertickets.EmployeeId = Reader.GetInt32(1);
                usertickets.Description = Reader.GetString(2);
                usertickets.Amount = Decimal.ToDouble(Reader.GetDecimal(3));
                usertickets.StatusofTicket = Reader.GetString(4);
                usertickets.SubmittedAt = Reader.GetDateTime(5);
                usertickets.CompletedAt = Reader.GetDateTime(6);
                ticketList.Add(usertickets);
            }
            Reader.Close();
            Connection.Close();

            return ticketList;
        }

        //get all user pending tickets
        public List<Ticket> PendingUserTickets(User currentuser)
        {
            var ticketList = new List<Ticket>();
            using SqlConnection Connection = new SqlConnection(ConnectionString);
            SqlCommand Command = new SqlCommand();
            Connection.Open();
            Command = new(@"SELECT * FROM [View.PendingTickets] WHERE employeeid = @EmployeeId;", Connection);
            Command.Parameters.AddWithValue("@EmployeeId", currentuser.EmployeeId);
            using SqlDataReader Reader = Command.ExecuteReader();
            while (Reader.Read())
            {
                Ticket usertickets = new Ticket();
                usertickets.TicketId = Reader.GetInt32(0);
                usertickets.EmployeeId = Reader.GetInt32(1);
                usertickets.Description = Reader.GetString(2);
                usertickets.Amount = Decimal.ToDouble(Reader.GetDecimal(3));
                usertickets.StatusofTicket = Reader.GetString(4);
                usertickets.SubmittedAt = Reader.GetDateTime(5);
                usertickets.CompletedAt = Reader.GetDateTime(6);
                ticketList.Add(usertickets);
            }
            Reader.Close();
            Connection.Close();

            return ticketList;
        }


        //gets all user approved tickets
        public List<Ticket> ApprovedUserTickets(User currentuser)
        {
            var ticketList = new List<Ticket>();
            using SqlConnection Connection = new SqlConnection(ConnectionString);
            SqlCommand Command = new SqlCommand();
            Connection.Open();
            Command = new(@"SELECT * FROM [View.ApprovedTickets] WHERE employeeid = @EmployeeId;", Connection);
            Command.Parameters.AddWithValue("@EmployeeId", currentuser.EmployeeId);
            using SqlDataReader Reader = Command.ExecuteReader();
            while (Reader.Read())
            {
                Ticket usertickets = new Ticket();
                usertickets.TicketId = Reader.GetInt32(0);
                usertickets.EmployeeId = Reader.GetInt32(1);
                usertickets.Description = Reader.GetString(2);
                usertickets.Amount = Decimal.ToDouble(Reader.GetDecimal(3));
                usertickets.StatusofTicket = Reader.GetString(4);
                usertickets.SubmittedAt = Reader.GetDateTime(5);
                usertickets.CompletedAt = Reader.GetDateTime(6);
                ticketList.Add(usertickets);
            }
            Reader.Close();
            Connection.Close();

            return ticketList;
        }


        //gets all user denied tickets
        public List<Ticket> DeniedUserTickets(User currentuser)
        {
            var ticketList = new List<Ticket>();
            using SqlConnection Connection = new SqlConnection(ConnectionString);
            SqlCommand Command = new SqlCommand();
            Connection.Open();
            Command = new(@"SELECT * FROM [View.DeniedTickets] WHERE employeeid = @EmployeeId;", Connection);
            Command.Parameters.AddWithValue("@EmployeeId", currentuser.EmployeeId);
            using SqlDataReader Reader = Command.ExecuteReader();
            while (Reader.Read())
            {
                Ticket usertickets = new Ticket();
                usertickets.TicketId = Reader.GetInt32(0);
                usertickets.EmployeeId = Reader.GetInt32(1);
                usertickets.Description = Reader.GetString(2);
                usertickets.Amount = Decimal.ToDouble(Reader.GetDecimal(3));
                usertickets.StatusofTicket = Reader.GetString(4);
                usertickets.SubmittedAt = Reader.GetDateTime(5);
                usertickets.CompletedAt = Reader.GetDateTime(6);
                ticketList.Add(usertickets);
            }
            Reader.Close();
            Connection.Close();

            return ticketList;
        }

        public List<Ticket> getPendingTickets()
        //Gets all of PENDING tickets only
        {
            var ticketList = new List<Ticket>();
            using SqlConnection Connection = new SqlConnection(ConnectionString);

            Connection.Open();
            using SqlCommand Command = new("SELECT * FROM [View.PendingTickets]", Connection);
            using SqlDataReader Reader = Command.ExecuteReader();
            while (Reader.Read())
            {
                Ticket usertickets = new Ticket();
                usertickets.TicketId = Reader.GetInt32(0);
                usertickets.EmployeeId = Reader.GetInt32(1);
                usertickets.Description = Reader.GetString(2);
                usertickets.Amount = Decimal.ToDouble(Reader.GetDecimal(3));
                usertickets.StatusofTicket = Reader.GetString(4);
                usertickets.SubmittedAt = Reader.GetDateTime(5);
                usertickets.CompletedAt = Reader.GetDateTime(6);
                ticketList.Add(usertickets);
            }
            Reader.Close();
            Connection.Close();

            return ticketList;
        }



        public void UpdateTicket(Ticket updatedticket)
        {
            using SqlConnection Connection = new SqlConnection(ConnectionString);
            Connection.Open();
            string Commandtext = (@"UPDATE TicketSystem.Tickets SET StatusofTicket = @StatusofTicket, CompletedAt = @CompletedAt WHERE TicketId = @TicketId");
            using SqlCommand Command = new SqlCommand(Commandtext, Connection);
            Command.Parameters.AddWithValue("@StatusofTicket", updatedticket.StatusofTicket);
            Command.Parameters.AddWithValue("@TicketId", updatedticket.TicketId);
            Command.Parameters.AddWithValue("@CompletedAt", DateTime.Now);
            Command.ExecuteNonQuery();
            Connection.Close();
            
        }

    }

}