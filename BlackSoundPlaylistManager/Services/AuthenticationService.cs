using BlackSound_playlist_manager.Entity;
using BlackSound_playlist_manager.Helpers;
using BlackSound_playlist_manager.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackSound_playlist_manager.Services
{
    public static class AuthenticationService
    {
        public static User LoggedUser { get; set; }

        public static void AuthenticateUser(string email, string password)
        {
            UsersRepository userRepo = new UsersRepository(Constants.UsersPath);
            LoggedUser = userRepo.GetUserByUserNameAndPassword(email, password);
        }


    }
}
