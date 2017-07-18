using BlackSound_playlist_manager.Entity;
using BlackSound_playlist_manager.Enums;
using BlackSound_playlist_manager.Helpers;
using BlackSound_playlist_manager.Repository;
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
                    case AdminViewOption.ManageSongs:
                        SongsView songsView = new SongsView();
                        songsView.Show();
                        break;
                    case AdminViewOption.ManageArtists:
                        ArtistsView artistsView = new ArtistsView();
                        artistsView.Show();
                        break;
                    case AdminViewOption.Logout:
                        AuthenticationService.LoggedUser = null;
                        return;
                    default:
                        break;
                }
            }
        }

        public AdminViewOption RenderMenu()
        {
            RenderMenu:
            Console.Clear();
            Console.WriteLine("Manage [S]ongs");
            Console.WriteLine("Manage [A]rtists");
            Console.WriteLine("[L]ogout");
            Console.WriteLine("Press one of the available keys to select option.");
            ConsoleKeyInfo cki = Console.ReadKey(true);
            switch (cki.Key)
            {
                case ConsoleKey.S:
                    return AdminViewOption.ManageSongs;
                case ConsoleKey.A:
                    return AdminViewOption.ManageArtists;
                case ConsoleKey.L:
                    return AdminViewOption.Logout;
                default:
                    Console.WriteLine("You have pressed an invalid option. Try again with one of the available ones above.");
                    Console.ReadKey(true);
                    goto RenderMenu;
            }
        }

        public void ReadSong() //To implement after CreateSong() is ready.
        {

        }

        public void CreateSong()
        {
            Console.Clear();
            CreateSong:
            Song newSong = new Song();
            Console.Write("Enter new song's title: ");
            string inputSongTitle = Console.ReadLine();
            if (inputSongTitle.Length == 0)
            {
                Console.WriteLine("Song title cannot be empty. Try again!!");
                Console.ReadKey(true);
                goto CreateSong;
            }
            newSong.Title = inputSongTitle;
            Console.Write("Enter song release year: ");
            short inputYear;
            bool isShort = short.TryParse(Console.ReadLine(), out inputYear);
            while (isShort == false)
            {
                Console.WriteLine("Year should be a digit. Try again!!");
                Console.Write("Enter song release year: ");
                isShort = short.TryParse(Console.ReadLine(), out inputYear);
            }

            if ((short)DateTime.Now.Year < inputYear || inputYear < 1850)
            {
                Console.WriteLine("Input year cannot be before 1500 or after the current year. Try again!!");
                Console.Write("Enter song release year: ");
                isShort = short.TryParse(Console.ReadLine(), out inputYear);
            }

            newSong.Year = inputYear;
            SongsRepository songsRepo = new SongsRepository(Constants.SongsPath);
            songsRepo.Save(newSong);
            Console.WriteLine("Song saved successfully!!");
            Console.ReadKey(true);
        }
    }
}
