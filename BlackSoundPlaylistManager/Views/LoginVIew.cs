using BlackSound_playlist_manager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackSound_playlist_manager.Views
{
    public class LoginView
    {
        public void Show()
        {
            while (true)
            {
                Console.Clear();
                Console.Write("Email: ");
                string inputEmail = Console.ReadLine();
                Console.Write("Password: ");
                string inputPassword = Console.ReadLine();
                AuthenticationService.AuthenticateUser(inputEmail, inputPassword);
                if (AuthenticationService.LoggedUser != null)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid username and/or password!!");
                    Console.ReadKey(true);
                }
            }
        }
    }
}
