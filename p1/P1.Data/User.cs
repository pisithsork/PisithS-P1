namespace P1.Data
{
    public class User
    {
        //Field
        string userName;
        string Password;

        //Constructors
        public User() { }

        public User(string userName, string password)
        {
            this.userName = userName;
            this.Password = password;
        }



        //Methods
        protected string getUserName()
        {
            return this.userName;
        }

        protected string getPassword()
        {
            return this.Password;
        }
    }
}