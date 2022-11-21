using System.Text;

namespace P1.Logic
{
    public class User
    {

        //Fields
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int EmployeeId { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool isManager { get; set; }
        //Constructors
        public User() { }

        public User(int employeeid, string firstname, string lastname, string password, string email, bool ismanager)
        {
            this.FirstName = firstname;
            this.LastName = lastname;
            this.EmployeeId = employeeid;
            this.Password = password;
            this.Email = email;
            this.isManager = ismanager;
        }

        public User(string firstname, string lastname, string password, string email)
        //Used to register new users, employeeid and ismanager should automatically add when doing SQL Injection
        {
            this.FirstName = firstname;
            this.LastName = lastname;
            this.Password = password;
            this.Email = email;
        }
        public User(string email, string password)
        {
            this.Email = email;
            this.Password = password;
        }



        //Methods
        //I wanna method to display certain information of the user(s)
        public string GetUserInformation()
        {
            StringBuilder StringBuilder = new StringBuilder();
            if (this.isManager == true)
            {
                StringBuilder.Append($"{this.FirstName} {this.LastName} Employee Number {this.EmployeeId} and I am a Manager");
            }
            else
            {
                StringBuilder.Append($"{this.FirstName} {this.LastName} Employee Number {this.EmployeeId} and I am an Employee");
            }
            return StringBuilder.ToString();
        }

    }
}