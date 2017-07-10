using BlackSound_playlist_manager.Entity;
using BlackSound_playlist_manager.Enums;
using BlackSound_playlist_manager.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackSound_playlist_manager.Views
{
    public class ArtistsView
    {
        public void Show()
        {
            while (true)
            {
                ArtistsManagementOption selectedOption = RenderMenu();
                switch (selectedOption)
                {
                    case ArtistsManagementOption.AddArtist:
                        AddArtist();
                        break;
                    case ArtistsManagementOption.EditArtist:
                        break;
                    case ArtistsManagementOption.ViewArtists:
                        break;
                    case ArtistsManagementOption.DeleteArtist:
                        break;
                    case ArtistsManagementOption.Back:
                        return;
                    default:
                        break;
                }
            }
        }

        private ArtistsManagementOption RenderMenu()
        {
            while (true)
            {
                RenderMenu:
                Console.Clear();
                Console.WriteLine("[A]dd Artist");
                Console.WriteLine("[V]iew Artists");
                Console.WriteLine("[E]dit Artist");
                Console.WriteLine("[D]elete Artist");
                Console.WriteLine("[B]ack");
                Console.WriteLine("Press one of the keys above to select option.");
                ConsoleKeyInfo cki = Console.ReadKey(true);
                switch (cki.Key)
                {
                    case ConsoleKey.A:
                        return ArtistsManagementOption.AddArtist;
                    case ConsoleKey.V:
                        return ArtistsManagementOption.ViewArtists;
                    case ConsoleKey.E:
                        return ArtistsManagementOption.EditArtist;
                    case ConsoleKey.D:
                        return ArtistsManagementOption.DeleteArtist;
                    case ConsoleKey.B:
                        return ArtistsManagementOption.Back;
                    default:
                        Console.WriteLine("You have pressed an invalid option. Try again with one of the available ones above.");
                        Console.ReadKey(true);
                        goto RenderMenu;
                }
            }
        }

        internal void AddArtist()
        {
            AddArtist:
            Console.Clear();
            Console.Write("Enter new artist name: ");
            string inputArtistName = Console.ReadLine();
            if (inputArtistName.Length < 1)
            {
                Console.WriteLine("Artist name cannot be empty. Try again!!");
                Console.ReadKey(true);
                goto AddArtist;
            }
            Artist artist = new Artist();
            artist.Name = inputArtistName;
            ArtistsRepository artistsRepo = new ArtistsRepository("artists.txt");
            artistsRepo.Save(artist);
            Console.WriteLine("Artist saved successfully!");
            Console.ReadKey(true);
        }
    }
}
