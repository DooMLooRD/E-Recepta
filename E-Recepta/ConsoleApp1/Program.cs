using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserDatabaseAPI.Service;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            UserService service=new UserService();
            foreach (var allPatient in service.GetAllPatients().Result)
            {
                Console.WriteLine(allPatient.Username);
            }
            LoginService loginService = new LoginService();     
            Console.WriteLine(loginService.GetPasswordHash("Patient1", "Patient").Result);
            Console.ReadKey();
        }
    }
}
