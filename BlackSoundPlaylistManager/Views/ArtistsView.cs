using BlackSound_playlist_manager.Entity;
using BlackSound_playlist_manager.Enums;
using BlackSound_playlist_manager.Helpers;
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
                        EditArtist();
                        break;
                    case ArtistsManagementOption.ViewArtists:
                        ViewArtists();
                        break;
                    case ArtistsManagementOption.DeleteArtist:
                        DeleteArtist();
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

            ArtistsRepository artistsRepo = new ArtistsRepository(Constants.ArtistsPath);
            if (artistsRepo.EntityExists(a => a.Name == inputArtistName))
            {
                Console.WriteLine("The artist already exists in the database!");
                Console.ReadKey(true);
                return;
            }
            Artist artist = new Artist();
            artist.Name = inputArtistName;
            artistsRepo.Save(artist);
            Console.WriteLine("Artist saved successfully!");
            Console.ReadKey(true);
        }

        internal void ViewArtists()
        {
            ArtistsRepository artistsRepo = new ArtistsRepository(Constants.ArtistsPath);
            List<Artist> artists = artistsRepo.GetAll();
            Console.Clear();
            foreach (Artist artist in artists)
            {
                Console.WriteLine("***************************************");
                Console.WriteLine("Id: {0}", artist.Id);
                Console.WriteLine("Artist name: {0}", artist.Name);
                Console.WriteLine("***************************************");
            }
            Console.ReadKey(true);
        }

        public void EditArtist()
        {
            ArtistsRepository artistRepo = new ArtistsRepository(Constants.ArtistsPath);
            EditArtist:
            List<Artist> artists = artistRepo.GetAll();
            Console.Clear();
            foreach (Artist artistEntity in artists)
            {
                Console.WriteLine("************************************");
                Console.WriteLine("Id: {0}", artistEntity.Id);
                Console.WriteLine("Artist name: {0}", artistEntity.Name);
            }
            Console.WriteLine();
            Console.Write("Enter id to edit: ");
            int inputArtistId;
            bool isInt = int.TryParse(Console.ReadLine(), out inputArtistId);
            while (isInt == false)
            {
                Console.WriteLine("Id should be an integer number. Try again!!");
                Console.ReadKey(true);
                goto EditArtist;
            }

            Artist artist = new Artist();
            Console.WriteLine("Editing artist with Id: {0}", inputArtistId);
            EnterArtistName:
            Console.Write("Enter new artist name: ");
            string inputArtistName = Console.ReadLine();
            if (inputArtistName.Length < 1)
            {
                Console.WriteLine("Artist name cannot be empty. Try Again!!");
                Console.ReadKey(true);
                goto EnterArtistName;
            }
            artist.Id = inputArtistId;
            artist.Name = inputArtistName;
            artistRepo.Save(artist);
            Console.WriteLine("Artist edited successfully!");
            Console.ReadKey(true);
        }

        public void DeleteArtist()
        {
            ArtistsRepository artistsRepo = new ArtistsRepository(Constants.ArtistsPath);
            List<Artist> artists = artistsRepo.GetAll();
            Console.Clear();
            foreach (Artist artistEntity in artists)
            {
                Console.WriteLine("***************************************");
                Console.WriteLine("Id: {0}", artistEntity.Id);
                Console.WriteLine("Artist name: {0}", artistEntity.Name);
                Console.WriteLine("***************************************");
            }
            Console.WriteLine();
            InputId:
            Console.Write("Enter user id to delete: ");
            int deleteId;
            bool isInt = int.TryParse(Console.ReadLine(), out deleteId);
            while (isInt == false)
            {
                Console.WriteLine("IDs can only be integer numbers. Try again!!");
                Console.ReadKey(true);
                goto InputId;
            }
            Artist artist = artistsRepo.GetAll(a => a.Id == deleteId).FirstOrDefault();
            artistsRepo.Delete(artist);
            Console.WriteLine("Artist deleted successfully!");
            Console.ReadKey(true);
        }
    }
}
