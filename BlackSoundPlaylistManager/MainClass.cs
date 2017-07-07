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
                LoginVIew loginView = new LoginVIew();
                loginView.Show();
                if (AuthenticationService.LoggedUser != null && AuthenticationService.LoggedUser.IsAdministrator)
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
