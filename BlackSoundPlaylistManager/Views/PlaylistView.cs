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
    public class PlaylistView
    {
        public void Show()
        {
            while (true)
            {
                PlaylistOption selectedOption = RenderMenu();
                switch (selectedOption)
                {
                    case PlaylistOption.AddPlaylist:
                        AddPlaylist();
                        break;
                    case PlaylistOption.EditPlaylist:
                        //EditPlaylist();
                        break;
                    case PlaylistOption.ViewAllPlaylists:
                        //ViewAllPlaylists();
                        break;
                    case PlaylistOption.DeletePlaylist:
                        //DeletePlaylist();
                        break;
                    case PlaylistOption.SharePlaylist:
                        //SharePlaylist();
                        break;
                    default:
                        throw new NotImplementedException("Reached default - this shouldn't happen in this case!");
                }
            }
        }

        public PlaylistOption RenderMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("[A]dd Playlist");
                Console.WriteLine("[E]dit Playlist");
                Console.WriteLine("[V]iew all Playlists");
                Console.WriteLine("[D]elete Playlist");
                Console.WriteLine("[S]hare a playlist");
                Console.WriteLine("Press one of the keys above to select option.");
                ConsoleKeyInfo cki = Console.ReadKey(true);
                switch (cki.Key)
                {
                    case ConsoleKey.A:
                        return PlaylistOption.AddPlaylist;
                    case ConsoleKey.E:
                        return PlaylistOption.EditPlaylist;
                    case ConsoleKey.V:
                        return PlaylistOption.ViewAllPlaylists;
                    case ConsoleKey.D:
                        return PlaylistOption.DeletePlaylist;
                    case ConsoleKey.S:
                        return PlaylistOption.SharePlaylist;
                    default:
                        Console.WriteLine("You have entered an invalid option. Try again with one of the available ones!!");
                        Console.ReadKey(true);
                        break;
                }
            }
        }

        public void AddPlaylist()
        {
            string inputPlaylistName;
            bool isEmptyName = false;
            do
            {
                Console.Clear();
                Console.Write("Enter playlist name: ");
                inputPlaylistName = Console.ReadLine();
                isEmptyName = string.IsNullOrWhiteSpace(inputPlaylistName);
                if (isEmptyName)
                {
                    Console.WriteLine("Playlist name cannot be empty. Try again!");
                    Console.ReadKey(true);
                }
            } while (isEmptyName == true);

            Playlist newPlaylist = new Playlist();
            newPlaylist.Name = inputPlaylistName;

            Console.Write("Enter playlist description: ");
            newPlaylist.Description = Console.ReadLine();

            bool incorrectInput;
            string isPublic;
            do
            {
                incorrectInput = false;
                Console.Write("New playlist will be public(yes/no): ");
                isPublic = Console.ReadLine();
                if (!(isPublic.ToLower() == "yes" ^ isPublic.ToLower() == "no"))
                {
                    incorrectInput = true;
                }

                if (incorrectInput == true)
                {
                    Console.WriteLine("You can only enter \"yes\" and \"no\". Try again!");
                    Console.ReadKey(true);
                }
            } while (incorrectInput == true);

            if (isPublic == "yes")
            {
                newPlaylist.IsPublic = true;
            }
            else
            {
                newPlaylist.IsPublic = false;
            }

            PlaylistsRepository playlistsRepo = new PlaylistsRepository(Constants.PlaylistsPath);
            int playlistId = playlistsRepo.Save(newPlaylist);
            int userId = AuthenticationService.LoggedUser.Id;
            UsersPlaylists usersPlaylistsEntity = new UsersPlaylists();
            usersPlaylistsEntity.PlaylistId = playlistId;
            usersPlaylistsEntity.UserId = userId;
            UsersPlaylistsRepository usersPlaylistsRepo = new UsersPlaylistsRepository(Constants.UsersPlaylistsPath);
            usersPlaylistsRepo.Save(usersPlaylistsEntity);
            Console.WriteLine("Playlist added successfuly!");
            Console.ReadKey(true);
            
        }
    }
}
