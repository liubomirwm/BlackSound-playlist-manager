using BlackSound_playlist_manager.Services;
using BlackSound_playlist_manager.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackSound_playlist_manager
{
    class MainClass
    {
        static void Main(string[] args)
        {
            while (true)
            {
                if (AuthenticationService.LoggedUser == null) // the check for null is to prevent stepping into if logged in. You really don't want to touch it (or the ones below). ;)
                {
                    LoginView loginView = new LoginView();
                    loginView.Show();
                }

                if (AuthenticationService.LoggedUser != null && AuthenticationService.LoggedUser.IsAdministrator) // the check for null is to prevent stepping into if logged out.
                {
                    AdminView adminView = new AdminView();
                    adminView.Show();
                }
                else if (AuthenticationService.LoggedUser != null && AuthenticationService.LoggedUser.IsAdministrator == false)
                {
                    //RegularUserView regularUserView = new RegularUserView();
                }
            }
        }
    }
}
