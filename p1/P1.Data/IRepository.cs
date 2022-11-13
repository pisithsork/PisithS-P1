using P1.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P1.Data
{
    public interface IRepository
    {
        public IEnumerable<User> GetAllUsers();
        public bool doesEmailExist(string username);
        public bool isCredentialValid(string userName, string Password);
        public void AddNewUser(User newUser);

    }
}
