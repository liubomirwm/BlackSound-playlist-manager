using BlackSound_playlist_manager.Enums;
using BlackSound_playlist_manager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackSound_playlist_manager.Views
{
    public class AdminView
    {
        public void Show()
        {
            while (true)
            {
                AdminViewOption selectedOption = RenderMenu();
                switch (selectedOption)
                {
                    case AdminViewOption.ReadSong:
                        
                    case AdminViewOption.CreateSong:
                        break;
                    case AdminViewOption.UpdateSong:
                        break;
                    case AdminViewOption.DeleteSong:
                        break;
                    case AdminViewOption.Logout:
                        AuthenticationService.LoggedUser = null;
                        return;
                    default:
                        throw new NotImplementedException("Reached default: this shouldn't happen in this case!!");
                }
            }
        }

        public AdminViewOption RenderMenu()
        {
            RenderMenu:
            Console.WriteLine("[R]ead song");
            Console.WriteLine("[C]reate song");
            Console.WriteLine("[U]pdate song");
            Console.WriteLine("[D]elete song");
            Console.WriteLine("[L]ogout");
            Console.WriteLine("Press one of the above keys to select option.");
            ConsoleKeyInfo cki = Console.ReadKey(true);
            switch (cki.Key)
            {
                case ConsoleKey.C:
                    return AdminViewOption.CreateSong;
                case ConsoleKey.R:
                    return AdminViewOption.ReadSong;
                case ConsoleKey.U:
                    return AdminViewOption.UpdateSong;
                case ConsoleKey.D:
                    return AdminViewOption.DeleteSong;
                case ConsoleKey.L:
                    return AdminViewOption.Logout;
                default:
                    Console.WriteLine("You have pressed an invalid option. Try again with one of the available ones above.");
                    Console.ReadKey(true);
                    goto RenderMenu;
            }
        }
    }
}
