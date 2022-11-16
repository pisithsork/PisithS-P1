using P1_Client;
using System;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace P1.Client
{

    class Program
    {

        static HttpClient client = new HttpClient();

        static void Main()
        {
            Runit().GetAwaiter().GetResult();
        }

        static async Task Runit()
        {
            client.BaseAddress = new Uri("https://localhost:7284/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                //I wanna get all my users from my database
                var users = await GetAllUsersAsync();
                foreach(var user in users)
                {
                    DisplayUser(user);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Oopsie woopsie you have a exception poopsy" + e);
            }

        }

        static void DisplayUser(User user)
        {
            string Title;
            if(user.isManager = true)
            {
                Title = "Manager";
            }
            else
            {
                Title = "Employee";
            }
            Console.WriteLine($"{user.FirstName} {user.LastName} \n Employee ID: {user.EmployeeId} \n Title: {Title}");
            Console.WriteLine();

        }

        static async Task<List<User>> GetAllUsersAsync()
        {
            List<User> users = new List<User>();
            var findpath = "getusers";
            HttpResponseMessage response = await client.GetAsync(findpath);
            if (response.IsSuccessStatusCode)
            {
                users = await response.Content.ReadAsAsync<List<User>>();
                           }
            return users;
        }

    }

}