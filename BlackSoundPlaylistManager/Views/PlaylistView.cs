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
                    case PlaylistOption.ViewUserPlaylists:
                        ViewUserPlaylists(true);
                        break;
                    case PlaylistOption.DeletePlaylist:
                        DeletePlaylist();
                        break;
                    case PlaylistOption.SharePlaylist:
                        SharePlaylist();
                        break;
                    case PlaylistOption.AddSongToPlaylist:
                        AddSongToPlaylist();
                        break;
                    case PlaylistOption.RemoveSongFromPlaylist:
                        RemoveSongFromPlaylist();
                        break;
                    case PlaylistOption.ViewSpecificPlaylist:
                        ViewSpecificPlaylist();
                        break;
                    case PlaylistOption.ShowAllSongsPlaylist:
                        SongsView.ViewSongs();
                        Console.ReadKey();
                        break;
                    case PlaylistOption.ViewPublicPlaylists:
                        ViewPublicPlaylists();
                        break;
                    case PlaylistOption.Logout:
                        AuthenticationService.LoggedUser = null;
                        return;
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
                Console.WriteLine("Add a song [T]o Playlist");
                Console.WriteLine("[R]emove a song from playlist");
                Console.WriteLine("View specific [P]laylist");
                Console.WriteLine("S[H]ow all songs playlist");
                Console.WriteLine("View [O]ther users public playlists");
                Console.WriteLine("[L]ogout");
                Console.WriteLine("Press one of the keys above to select option.");
                ConsoleKeyInfo cki = Console.ReadKey(true);
                switch (cki.Key)
                {
                    case ConsoleKey.A:
                        return PlaylistOption.AddPlaylist;
                    case ConsoleKey.E:
                        return PlaylistOption.EditPlaylist;
                    case ConsoleKey.V:
                        return PlaylistOption.ViewUserPlaylists;
                    case ConsoleKey.D:
                        return PlaylistOption.DeletePlaylist;
                    case ConsoleKey.S:
                        return PlaylistOption.SharePlaylist;
                    case ConsoleKey.T:
                        return PlaylistOption.AddSongToPlaylist;
                    case ConsoleKey.R:
                        return PlaylistOption.RemoveSongFromPlaylist;
                    case ConsoleKey.P:
                        return PlaylistOption.ViewSpecificPlaylist;
                    case ConsoleKey.H:
                        return PlaylistOption.ShowAllSongsPlaylist;
                    case ConsoleKey.O:
                        return PlaylistOption.ViewPublicPlaylists;
                    case ConsoleKey.L:
                        return PlaylistOption.Logout;
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
            do //TODO: Refactore all do while loops with a simple while loop where possible (see bookmarked while loop for example)
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

        public void ViewUserPlaylists(bool calledFromPlaylistsView = false)
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
            if (playlists.Count == 0)
            {
                Console.WriteLine("You have no playlists yet.");
                Console.ReadKey(true);
                return;
            }
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

            if (calledFromPlaylistsView)
            {
                Console.ReadKey(true);
            }
        }

        public void DeletePlaylist()
        {
            ViewUserPlaylists();
            Console.WriteLine();
            Console.Write("Enter id to delete: ");
            int deleteId = 0;
            int currentUserId = AuthenticationService.LoggedUser.Id;
            bool isIntId = int.TryParse(Console.ReadLine(), out deleteId);
            while (isIntId == false)
            {
                Console.WriteLine("Id can only be an integer number. Try again!!");
                Console.ReadKey();
                Console.Write("Enter id to delete: ");
                isIntId = int.TryParse(Console.ReadLine(), out deleteId);
            }

            UsersPlaylistsRepository usersPlaylistsRepo = new UsersPlaylistsRepository(Constants.UsersPlaylistsPath);
            PlaylistsRepository playlistsRepo = new PlaylistsRepository(Constants.PlaylistsPath);
            UsersPlaylists usersPlaylistsEntity = usersPlaylistsRepo.GetAll(upe => upe.PlaylistId == deleteId && upe.UserId == currentUserId)
                .FirstOrDefault();

            if (usersPlaylistsEntity == null)
            {
                Console.WriteLine("Playlist with id {0} does not exist or you have no rights to delete!", deleteId);
                Console.ReadKey();
                return;
            }

            PlaylistsSongsRepository playlistsSongsRepo = new PlaylistsSongsRepository(Constants.PlaylistsSongsPath);
            List<PlaylistsSongs> playlistsSongsEntities = playlistsSongsRepo.GetAll(pse => pse.PlaylistId == deleteId);
            foreach (PlaylistsSongs playlistsSongsEntity in playlistsSongsEntities)
            {
                playlistsSongsRepo.Delete(playlistsSongsEntity);
            }

            Playlist playlistToDelete = playlistsRepo.GetAll(p => p.Id == deleteId).FirstOrDefault();
            usersPlaylistsRepo.Delete(usersPlaylistsEntity);
            playlistsRepo.Delete(playlistToDelete);
            Console.WriteLine("Playlist successfully deleted!");
            Console.ReadKey(true);
        }

        public void SharePlaylist() //TODO: Ensure there are checks if playlist is already shared.
        {
            UsersRepository usersRepo = new UsersRepository(Constants.UsersPath);
            int currentUserId = AuthenticationService.LoggedUser.Id;
            List<User> users = usersRepo.GetAll(u => u.IsAdministrator != true && u.Id != currentUserId);
            Console.Clear();
            if (users.Count == 0)
            {
                Console.WriteLine("There are currently no users with whom you can share a playlist.");
                Console.ReadKey(true);
                return;
            }
            foreach (User user in users)
            {
                Console.WriteLine("**************************");
                Console.WriteLine("Id: {0}", user.Id);
                Console.WriteLine("Name: {0}", user.DisplayName);
                Console.WriteLine("**************************");
            }

            Console.WriteLine();
            Console.Write("Enter Id of user with whom you will share playlist: ");
            int userId = 0;
            bool isIntId = int.TryParse(Console.ReadLine(), out userId);
            while (isIntId == false)
            {
                Console.WriteLine("User Id can only be an integer number. Try again!!");
                Console.ReadKey(true);
                Console.Write("Enter Id of user with whom you will share playlist: ");
                isIntId = int.TryParse(Console.ReadLine(), out userId);
            }

            bool isValidUser = false;
            foreach (User user in users)
            {
                if (user.Id == userId)
                {
                    isValidUser = true;
                    break;
                }
            }

            if (isValidUser == false)
            {
                Console.WriteLine("You cannot share a playlist with that user!");
                Console.ReadKey(true);
                return;
            }

            PlaylistsRepository playlistsRepo = new PlaylistsRepository(Constants.PlaylistsPath);
            UsersPlaylistsRepository usersPlaylistsRepo = new UsersPlaylistsRepository(Constants.UsersPlaylistsPath);
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
                Console.WriteLine("****************************");
                Console.WriteLine("Id: {0}", playlist.Id);
                Console.WriteLine("Name: {0}", playlist.Name);
                Console.WriteLine("Description: {0}", playlist.Description);
                Console.WriteLine("****************************");
            }

            Console.WriteLine();
            Console.Write("Enter id of playlist to share: ");
            isIntId = false;
            int playlistShareId = 0;
            isIntId = int.TryParse(Console.ReadLine(), out playlistShareId);
            while (isIntId == false)
            {
                Console.WriteLine("Id can only be an integer number. Try again!!");
                Console.ReadKey(true);
                Console.Write("Enter id of playlist to share: ");
                isIntId = int.TryParse(Console.ReadLine(), out playlistShareId);
            }

            bool isExistingPlaylist = usersPlaylistsRepo.EntityExists(upe => upe.PlaylistId == playlistShareId && upe.UserId == currentUserId);
            if (isExistingPlaylist == false)
            {
                Console.WriteLine("No playlist with id {0} exists or you have no rights to share it!");
                Console.ReadKey(true);
                return;
            }

            UsersPlaylists newUsersPlaylistsEntity = new UsersPlaylists(); //TODO: Enforce sharing restrictions with checks.
            newUsersPlaylistsEntity.PlaylistId = playlistShareId;
            newUsersPlaylistsEntity.UserId = userId;
            usersPlaylistsRepo.Save(newUsersPlaylistsEntity);
            Console.WriteLine("Playlist shared successfully!");
            Console.ReadKey(true);
        }

        public void AddSongToPlaylist()
        {
            SongsView.ViewSongs();
            Console.WriteLine();
            Console.Write("Enter id of song to add: ");
            int songId = 0;
            bool isIntSongId = int.TryParse(Console.ReadLine(), out songId);
            while (isIntSongId == false)
            {
                Console.WriteLine("Song id can only be an integer number. Try again!");
                Console.ReadKey(true);
                Console.Write("Enter id of song to add: ");
                isIntSongId = int.TryParse(Console.ReadLine(), out songId);
            }

            SongsRepository songsRepo = new SongsRepository(Constants.SongsPath);
            if (songsRepo.EntityExists(s => s.Id == songId) == false)
            {
                Console.WriteLine("Song with id {0} doesn't exist!", songId);
                Console.ReadKey(true);
                return;
            }

            ViewUserPlaylists();
            Console.Write("Select id of playlist you would add to: ");
            int playlistId = 0;
            bool isIntPlaylistId = int.TryParse(Console.ReadLine(), out playlistId);
            while (isIntPlaylistId == false)
            {
                Console.WriteLine("Song id can only be an integer number. Try again!");
                Console.ReadKey(true);
                Console.Write("Enter id of song to add: ");
                isIntPlaylistId = int.TryParse(Console.ReadLine(), out playlistId);
            }

            UsersPlaylistsRepository usersPlaylistsRepo = new UsersPlaylistsRepository(Constants.UsersPlaylistsPath);
            int currentUserId = AuthenticationService.LoggedUser.Id;
            if (usersPlaylistsRepo.EntityExists(upe => upe.PlaylistId == playlistId && upe.UserId == currentUserId) == false)
            {
                Console.WriteLine("Playlist does not exist exist or you have no rights to add songs to it!");
                Console.ReadKey(true);
                return;
            }
            PlaylistsSongs playlistSongsEntity = new PlaylistsSongs();
            playlistSongsEntity.SongId = songId;
            playlistSongsEntity.PlaylistId = playlistId;
            PlaylistsSongsRepository playlistsSongsRepo = new PlaylistsSongsRepository(Constants.PlaylistsSongsPath);
            if (playlistsSongsRepo.EntityExists(p => p.PlaylistId == playlistSongsEntity.PlaylistId && p.SongId == playlistSongsEntity.SongId))
            {
                Console.WriteLine("The selected song is already in that playlist!");
                Console.ReadKey(true);
                return;
            }

            playlistsSongsRepo.Save(playlistSongsEntity);
            Console.WriteLine("Song successfully added to playlist");
            Console.ReadKey(true);
        }

        public void RemoveSongFromPlaylist()
        {
            ViewUserPlaylists();
            Console.WriteLine();
            Console.Write("Enter id of playlist: ");
            int playlistId;
            bool isIntPlaylistId = int.TryParse(Console.ReadLine(), out playlistId);
            while (isIntPlaylistId == false)
            {
                Console.WriteLine("Id can only be an integer number. Try again!!");
                Console.ReadKey();
                Console.Write("Enter id of playlist: ");
                isIntPlaylistId = int.TryParse(Console.ReadLine(), out playlistId);
            }

            UsersPlaylistsRepository usersPlaylistsRepo = new UsersPlaylistsRepository(Constants.UsersPlaylistsPath);
            int currentUserId = AuthenticationService.LoggedUser.Id;
            if (usersPlaylistsRepo.EntityExists(upe => upe.PlaylistId == playlistId && upe.UserId == currentUserId) == false)
            {
                Console.WriteLine("Playlist does not exist exist or you have no rights to add songs to it!");
                Console.ReadKey(true);
                return;
            }

            PlaylistsSongsRepository playlistsSongsRepo = new PlaylistsSongsRepository(Constants.PlaylistsSongsPath);
            List<PlaylistsSongs> playlistsSongsEntities = playlistsSongsRepo.GetAll(pse => pse.PlaylistId == playlistId);
            SongsRepository songsRepo = new SongsRepository(Constants.SongsPath);
            SongsArtistsRepository songsArtistsRepo = new SongsArtistsRepository(Constants.SongsArtistsPath);
            ArtistsRepository artistsRepo = new ArtistsRepository(Constants.ArtistsPath);
            List<Song> songs = new List<Song>();
            foreach (PlaylistsSongs playlistSongsEntity in playlistsSongsEntities)
            {
                Song song = songsRepo.GetAll(s => s.Id == playlistSongsEntity.SongId).FirstOrDefault();
                songs.Add(song);
            }

            Console.Clear();
            foreach (Song song in songs)
            {
                Console.WriteLine("********************************");
                Console.WriteLine("Id: {0}", song.Id);
                Console.WriteLine("Song title: {0}", song.Title);
                Console.WriteLine("Song release year: {0}", song.Year);
                List<SongsArtists> songsArtists = songsArtistsRepo.GetAll(sa => sa.SongId == song.Id);
                List<Artist> artists = new List<Artist>();
                foreach (SongsArtists songArtistItem in songsArtists)
                {
                    int artistId = songArtistItem.ArtistId;
                    Artist artist = artistsRepo.GetAll(a => a.Id == songArtistItem.ArtistId).FirstOrDefault();
                    artists.Add(artist);
                }

                if (artists.Count == 1)
                {
                    Console.WriteLine("Artist: {0}", artists[0].Name);
                }
                else
                {
                    Console.Write("Artists: ");
                    int end = artists.Count - 1;
                    for (int i = 0; i < end; i++)
                    {
                        Console.WriteLine("{0}, ", artists[i].Name);
                    }
                    Console.WriteLine(artists[end]);
                }
                Console.WriteLine("********************************");
            }
            Console.WriteLine();
            Console.Write("Select song id to remove from the playlist: ");
            int songId;
            bool isIntSongId = int.TryParse(Console.ReadLine(), out songId);
            while (isIntSongId == false)
            {
                Console.WriteLine("Id can only be an integer number. Try again!!");
                Console.ReadKey();
                Console.Write("Select song id to remove from the playlist: ");
                isIntSongId = int.TryParse(Console.ReadLine(), out songId);
            }

            PlaylistsSongs playlistsSongsEntity = playlistsSongsRepo.GetAll(pse => pse.PlaylistId == playlistId && pse.SongId == songId).FirstOrDefault();
            if (playlistsSongsEntity == null)
            {
                Console.WriteLine("Song does not exist in that playlist or you have no rights to remove!");
                Console.ReadKey(true);
                return;
            }
            playlistsSongsRepo.Delete(playlistsSongsEntity);
            Console.WriteLine("Song successfully removed from playlist!");
            Console.ReadKey(true);

        }

        public void ViewSpecificPlaylist()
        {
            ViewUserPlaylists();
            Console.WriteLine();
            Console.Write("Enter playlist id to view: ");
            int currentUserId = AuthenticationService.LoggedUser.Id;
            int playlistId;
            bool isIntPlaylistId = int.TryParse(Console.ReadLine(), out playlistId);
            while (isIntPlaylistId == false)
            {
                Console.WriteLine("Id can only be an integer number");
                Console.Write("Enter playlist id to view: ");
                isIntPlaylistId = int.TryParse(Console.ReadLine(), out playlistId);
            }

            UsersPlaylistsRepository usersPlaylistsRepo = new UsersPlaylistsRepository(Constants.UsersPlaylistsPath);
            bool hasRightsToShare = usersPlaylistsRepo.EntityExists(upe => upe.PlaylistId == playlistId && upe.UserId == currentUserId);
            if (hasRightsToShare == false)
            {
                Console.WriteLine("Playlist does not exist or you have no rights to view!");
                Console.ReadKey(true);
                return;
            }
            PlaylistsSongsRepository playlistsSongsRepo = new PlaylistsSongsRepository(Constants.PlaylistsSongsPath);
            List<PlaylistsSongs> playlistsSongsEntities = playlistsSongsRepo.GetAll(pse => pse.PlaylistId == playlistId);
            SongsRepository songsRepo = new SongsRepository(Constants.SongsPath);
            SongsArtistsRepository songsArtistsRepo = new SongsArtistsRepository(Constants.SongsArtistsPath);
            ArtistsRepository artistsRepo = new ArtistsRepository(Constants.ArtistsPath);
            List<Song> songs = new List<Song>();
            foreach (PlaylistsSongs playlistsSongsEntity in playlistsSongsEntities)
            {
                Song song = songsRepo.GetAll(s => s.Id == playlistsSongsEntity.SongId).FirstOrDefault();
                songs.Add(song);
            }

            if (songs.Count == 0)
            {
                Console.WriteLine("Playlist empty!");
                Console.ReadKey(true);
                return;
            }

            Console.Clear();
            foreach (Song song in songs)
            {
                Console.WriteLine("********************************");
                Console.WriteLine("Id: {0}", song.Id);
                Console.WriteLine("Song title: {0}", song.Title);
                Console.WriteLine("Song release year: {0}", song.Year);
                List<SongsArtists> songsArtists = songsArtistsRepo.GetAll(sa => sa.SongId == song.Id);
                List<Artist> artists = new List<Artist>();
                foreach (SongsArtists songArtistItem in songsArtists)
                {
                    int artistId = songArtistItem.ArtistId;
                    Artist artist = artistsRepo.GetAll(a => a.Id == songArtistItem.ArtistId).FirstOrDefault();
                    artists.Add(artist);
                }

                if (artists.Count == 1)
                {
                    Console.WriteLine("Artist: {0}", artists[0].Name);
                }
                else
                {
                    Console.Write("Artists: ");
                    int end = artists.Count - 1;
                    for (int i = 0; i < end; i++)
                    {
                        Console.WriteLine("{0}, ", artists[i].Name);
                    }
                    Console.WriteLine(artists[end]);
                }
                Console.WriteLine("********************************");
            }

            Console.ReadKey(true);
        }
        public void ViewPublicPlaylists()
        {
            PlaylistsRepository playlistsRepo = new PlaylistsRepository(Constants.PlaylistsPath);
            List<Playlist> playlists = playlistsRepo.GetAll();
            IEnumerable<Playlist> filteredPlaylists = playlists.Where(p => p.IsPublic == true);

            Console.Clear();
            foreach (Playlist playlist in filteredPlaylists)
            {
                Console.WriteLine("*************************************");
                Console.WriteLine("Id: {0}", playlist.Id);
                Console.WriteLine("Playlist name: {0}", playlist.Name);
                if (string.IsNullOrWhiteSpace(playlist.Description) == false)
                {
                    Console.WriteLine("Description: {0}", playlist.Description);
                }
                Console.WriteLine("*************************************");
            }

            Console.WriteLine();
            Console.Write("Enter playlist id to see: ");
            int playlistId;
            bool isIntPlaylistId = int.TryParse(Console.ReadLine(), out playlistId);
            while (isIntPlaylistId == false)
            {
                Console.WriteLine("Playlist id can only be an integer number!");
                Console.ReadKey();
                Console.Write("Enter playlist id to see: ");
                isIntPlaylistId = int.TryParse(Console.ReadLine(), out playlistId);
            }

            PlaylistsSongsRepository playlistsSongsRepo = new PlaylistsSongsRepository(Constants.PlaylistsSongsPath);
            SongsArtistsRepository songsArtistsRepo = new SongsArtistsRepository(Constants.SongsArtistsPath);
            ArtistsRepository artistsRepo = new ArtistsRepository(Constants.ArtistsPath);
            SongsRepository songsRepo = new SongsRepository(Constants.SongsPath);
            List<PlaylistsSongs> playlistsSongsEntities = playlistsSongsRepo.GetAll(pse => pse.PlaylistId == playlistId);
            List<Song> songs = new List<Song>();
            foreach (PlaylistsSongs playlistsSongsEntity in playlistsSongsEntities)
            {
                Song song = songsRepo.GetAll(s => s.Id == playlistsSongsEntity.SongId).FirstOrDefault();
                songs.Add(song);
            }

            Console.Clear();
            foreach (Song song in songs)
            {
                Console.WriteLine("********************************");
                Console.WriteLine("Id: {0}", song.Id);
                Console.WriteLine("Song title: {0}", song.Title);
                Console.WriteLine("Song release year: {0}", song.Year);
                List<SongsArtists> songsArtists = songsArtistsRepo.GetAll(sa => sa.SongId == song.Id);
                List<Artist> artists = new List<Artist>();
                foreach (SongsArtists songArtistItem in songsArtists)
                {
                    int artistId = songArtistItem.ArtistId;
                    Artist artist = artistsRepo.GetAll(a => a.Id == songArtistItem.ArtistId).FirstOrDefault();
                    artists.Add(artist);
                }

                if (artists.Count == 1)
                {
                    Console.WriteLine("Artist: {0}", artists[0].Name);
                }
                else
                {
                    Console.Write("Artists: ");
                    int end = artists.Count - 1;
                    for (int i = 0; i < end; i++)
                    {
                        Console.WriteLine("{0}, ", artists[i].Name);
                    }
                    Console.WriteLine(artists[end]);
                }
                Console.WriteLine("********************************");
            }

            Console.ReadKey(true);
        }
    }
}
