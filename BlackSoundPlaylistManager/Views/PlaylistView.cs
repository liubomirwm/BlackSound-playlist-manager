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
                        EditPlaylist();
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

            bool isIncorrectInput;
            string isPublic;
            do
            {
                isIncorrectInput = false;
                Console.Write("New playlist will be public(yes/no): ");
                isPublic = Console.ReadLine();
                if (!(isPublic.ToLower() == "yes" ^ isPublic.ToLower() == "no"))
                {
                    isIncorrectInput = true;
                }

                if (isIncorrectInput == true)
                {
                    Console.WriteLine("You can only enter \"yes\" and \"no\". Try again!");
                    Console.ReadKey(true);
                }
            } while (isIncorrectInput == true);

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
            int currentUserId = AuthenticationService.LoggedUser.Id;
            UsersPlaylists usersPlaylistsEntity = new UsersPlaylists();
            usersPlaylistsEntity.PlaylistId = playlistId;
            usersPlaylistsEntity.UserId = currentUserId;
            UsersPlaylistsRepository usersPlaylistsRepo = new UsersPlaylistsRepository(Constants.UsersPlaylistsPath);
            usersPlaylistsRepo.Save(usersPlaylistsEntity);
            Console.WriteLine("Playlist added successfuly!");
            Console.ReadKey(true);

        }

        public void EditPlaylist()
        {
            PlaylistsRepository playlistsRepo = new PlaylistsRepository(Constants.PlaylistsPath);
            UsersPlaylistsRepository usersPlaylistsRepo = new UsersPlaylistsRepository(Constants.UsersPlaylistsPath);
            int currentUserId = AuthenticationService.LoggedUser.Id;
            List<UsersPlaylists> usersPlaylistsEntities = usersPlaylistsRepo.GetAll(upe => upe.UserId == currentUserId);
            List<Playlist> playlists = new List<Playlist>();
            foreach (UsersPlaylists usersPlaylistsEntity in usersPlaylistsEntities)
            {
                Playlist playlist = playlistsRepo.GetAll(p => p.Id == usersPlaylistsEntity.PlaylistId).FirstOrDefault();
                playlists.Add(playlist);
            }

            Console.Clear();
            foreach (Playlist playlist in playlists)
            {
                Console.WriteLine("*******************************");
                Console.WriteLine("Id: {0}", playlist.Id);
                Console.WriteLine("Playlist Name: {0}", playlist.Name);
                if (!String.IsNullOrWhiteSpace(playlist.Description))
                {
                    Console.WriteLine("Description: {0}", playlist.Description);
                }

                if (playlist.IsPublic == true)
                {
                    Console.WriteLine("Is public: yes");
                }
                else
                {
                    Console.WriteLine("Is public: no");
                }
                Console.WriteLine("*******************************");
            }

            Console.WriteLine();
            int editId = 0;
            bool isIntId = false;
            do
            {
                Console.Write("Enter id to edit: ");
                isIntId = int.TryParse(Console.ReadLine(), out editId);
                if (isIntId == false)
                {
                    Console.WriteLine("Only integer numbers can be entered for IDs. Try again!!");
                    Console.ReadKey(true);
                }
            } while (isIntId == false);

            bool hasRightsToEdit = false;
            foreach (UsersPlaylists usersPlaylistsEntity in usersPlaylistsEntities)
            {
                if (editId == usersPlaylistsEntity.PlaylistId)
                {
                    hasRightsToEdit = true;
                    break;
                }
            }

            if (hasRightsToEdit == false)
            {
                Console.WriteLine("Playlist with id {0} does not exist or you have no rights to edit!", editId);
                Console.ReadKey(true);
                return;
            }
            Playlist playlistToEdit = null;
            foreach (Playlist playlist in playlists)
            {
                if (playlist.Id == editId)
                {
                    playlistToEdit = playlist;
                    break;
                }
            }

            Console.Clear();
            Console.WriteLine("Old name: {0}", playlistToEdit.Name);
            string newName;
            do
            {
                Console.Write("New name: ");
                newName = Console.ReadLine();
                if (String.IsNullOrWhiteSpace(newName))
                {
                    Console.WriteLine("Playlist name cannot be empty. Try Again!!");
                    Console.ReadKey(true);
                }

            } while (String.IsNullOrWhiteSpace(newName));
            playlistToEdit.Name = newName;

            Console.WriteLine("Old description: {0}", playlistToEdit.Description);
            Console.Write("New description: ");
            playlistToEdit.Description = Console.ReadLine();
            if (playlistToEdit.IsPublic)
            {
                Console.WriteLine("Playlist is public: yes");
            }
            else
            {
                Console.WriteLine("Playlist is public: no");
            }

            bool isIncorrectInput;
            string isPublic;
            do
            {
                isIncorrectInput = false;
                Console.Write("New playlist will be public(yes/no): ");
                isPublic = Console.ReadLine();
                if (!(isPublic.ToLower() == "yes" ^ isPublic.ToLower() == "no"))
                {
                    isIncorrectInput = true;
                }

                if (isIncorrectInput == true)
                {
                    Console.WriteLine("You can only enter \"yes\" and \"no\". Try again!");
                    Console.ReadKey(true);
                }
            } while (isIncorrectInput == true);

            if (isPublic == "yes")
            {
                playlistToEdit.IsPublic = true;
            }
            else
            {
                playlistToEdit.IsPublic = false;
            }

            playlistsRepo.Save(playlistToEdit);
            Console.WriteLine("Playlist edited successfully!");
            Console.ReadKey(true);
        }
    }
}
