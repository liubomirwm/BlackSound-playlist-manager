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
    public class SongsView
    {
        public void Show()
        {
            while (true)
            {
                SongsViewOptions selectedOption = RenderMenu();
                switch (selectedOption)
                {
                    case SongsViewOptions.AddSong:
                        AddSong();
                        break;
                    case SongsViewOptions.EditSong:
                        EditSong();
                        break;
                    case SongsViewOptions.ViewSongs:
                        //ViewArtists();
                        break;
                    case SongsViewOptions.DeleteSong:
                        //DeleteArtist();
                        break;
                    case SongsViewOptions.Back:
                        return;
                    default:
                        throw new NotImplementedException("Reached default: this shouldn't happen");
                }
            }
        }

        public SongsViewOptions RenderMenu()
        {
            while (true)
            {
                RenderMenu:
                Console.Clear();
                Console.WriteLine("[A]dd Song");
                Console.WriteLine("[V]iew Songs");
                Console.WriteLine("[E]dit Song");
                Console.WriteLine("[D]elete Song");
                Console.WriteLine("[B]ack");
                Console.WriteLine("Press one of the keys above to select option.");
                ConsoleKeyInfo cki = Console.ReadKey(true);
                switch (cki.Key)
                {
                    case ConsoleKey.A:
                        return SongsViewOptions.AddSong;
                    case ConsoleKey.V:
                        return SongsViewOptions.ViewSongs;
                    case ConsoleKey.E:
                        return SongsViewOptions.EditSong;
                    case ConsoleKey.D:
                        return SongsViewOptions.DeleteSong;
                    case ConsoleKey.B:
                        return SongsViewOptions.Back;
                    default:
                        Console.WriteLine("You have pressed an invalid option. Try again with one of the available ones above.");
                        Console.ReadKey(true);
                        goto RenderMenu;
                }
            }
        }

        public void AddSong()
        {
            //AddSong:
            //Console.Clear();
            //Console.Write("Enter new song name: ");
            //string inputSongTitle = Console.ReadLine();
            //if (inputSongTitle.Length < 1)
            //{
            //    Console.WriteLine("Song title cannot be empty. Try again!!");
            //    Console.ReadKey(true);
            //    goto AddSong;
            //}

            string inputSongTitle;
            bool isEmptyName = false;
            do
            {
                Console.Clear();
                Console.Write("Enter new song name: ");
                inputSongTitle = Console.ReadLine();
                isEmptyName = (String.IsNullOrWhiteSpace(inputSongTitle));
                if (isEmptyName == true)
                {
                    Console.WriteLine("Song title cannot be empty. Try again!!");
                    Console.ReadKey(true);
                }
            }
            while (isEmptyName == true);

            short inputSongReleaseYear;
            bool isShortYear;
            bool isBeforeCurrentYear = false;
            do
            {
                Console.Write("Enter song's release year: ");
                isShortYear = short.TryParse(Console.ReadLine(), out inputSongReleaseYear);
                if (DateTime.Now.Year >= inputSongReleaseYear)
                {
                    isBeforeCurrentYear = true;
                }
                if (isShortYear == false)
                {
                    Console.WriteLine("Year can only be an integer number. Try again!!");
                    Console.ReadKey(true);
                }

                if (isBeforeCurrentYear == false)
                {
                    Console.WriteLine("Year should not be after the current year.");
                }
            } while (isShortYear == false || isBeforeCurrentYear == false);

            Song newSong = new Song();
            newSong.Title = inputSongTitle;
            newSong.Year = inputSongReleaseYear;
            SongsRepository songsRepo = new SongsRepository(Constants.SongsPath);
            if (songsRepo.EntityExists(s => s.Title == inputSongTitle && s.Year == inputSongReleaseYear))
            {
                Console.WriteLine("The song already exists in the database!");
                Console.ReadKey(true);
            }
            int songId = songsRepo.Save(newSong);
            ArtistsRepository artistsRepo = new ArtistsRepository(Constants.ArtistsPath);
            List<Artist> artists = artistsRepo.GetAll();
            Console.Clear();
            int artistId;
            if (artists.Count == 0)
            {
                artistId = ArtistsView.AddArtist();
            }
            else
            {
                foreach (Artist artistEntity in artists)
                {
                    Console.WriteLine("************************************");
                    Console.WriteLine("Id: {0}", artistEntity.Id);
                    Console.WriteLine("Artist name: {0}", artistEntity.Name);
                    Console.WriteLine("************************************");
                }

                Console.WriteLine();
                EnterArtistId:
                Console.Write("Enter new song's artist id (type \"0\" if not in the list): ");
                bool isInt = int.TryParse(Console.ReadLine(), out artistId);
                while (isInt == false)
                {
                    Console.WriteLine("IDs can only be integer numbers. Try Again!!");
                    Console.ReadKey(true);
                    goto EnterArtistId;
                }

                if (artistId == 0)
                {
                    artistId = ArtistsView.AddArtist();
                }
            }
            SongsArtists songArtistEntity = new SongsArtists();
            songArtistEntity.SongId = songId;
            songArtistEntity.ArtistId = artistId;
            SongsArtistsRepository songsArtistsRepo = new SongsArtistsRepository(Constants.SongsArtistsPath);
            songsArtistsRepo.Save(songArtistEntity);
            Console.WriteLine("Song saved successfully!");
            Console.ReadKey(true);
        }

        public void EditSong()
        {
            SongsRepository songsRepo = new SongsRepository(Constants.SongsPath);
            List<Song> songs = songsRepo.GetAll();
            Console.Clear();
            foreach (Song songEntity in songs)
            {
                Console.WriteLine("************************************");
                Console.WriteLine("Id: {0}", songEntity.Id);
                Console.WriteLine("Song Title: {0}", songEntity.Title);
                Console.WriteLine("Song release year: {0}", songEntity.Year);
                Console.WriteLine("************************************");
            }
            Console.WriteLine();
            int editId;
            bool isIntId;
            do
            {
                Console.Write("Enter song id to edit: ");
                isIntId = int.TryParse(Console.ReadLine(), out editId);
                if (isIntId == false)
                {
                    Console.WriteLine("IDs can only be integer numbers. Try again!!");
                    Console.ReadKey(true);
                }
            }
            while (isIntId == false);

            SongsRepository songRepo = new SongsRepository(Constants.SongsPath);
            Song song = songRepo.GetAll(s => s.Id == editId).FirstOrDefault();
            if (song == null)
            {
                Console.WriteLine("No song with that Id exists!");
                Console.ReadKey(true);
                return;
            }
            Console.WriteLine("Old song title: {0}", song.Title);
            string newSongTitle;
            bool isEmptyName;
            do
            {
                Console.Write("New song title: ");
                newSongTitle = Console.ReadLine();
                isEmptyName = string.IsNullOrWhiteSpace(newSongTitle);
                if (isEmptyName)
                {
                    Console.WriteLine("Song name cannot be empty. Try again!!");
                    Console.ReadKey();
                }
            } while (isEmptyName == true);

            song.Title = newSongTitle;

            short newSongReleaseYear;
            bool isIntYear = false;
            bool isCurrentOrPastYear = false;
            Console.WriteLine("Old song release year: {0}", song.Year);
            do
            {
                Console.Write("New song release year: ");
                isIntYear = short.TryParse(Console.ReadLine(), out newSongReleaseYear);
                if (isIntYear == false)
                {
                    Console.WriteLine("Song release year can only be an integer number. Try Again!!");
                    Console.ReadKey();
                }

                if (DateTime.Now.Year >= newSongReleaseYear)
                {
                    isCurrentOrPastYear = true;
                }

                if (isCurrentOrPastYear == false)
                {
                    Console.WriteLine("Song release year cannot be after the current year. Try again!!");
                    Console.ReadKey(true);
                }
            } while (isIntYear == false || isCurrentOrPastYear == false);

            song.Year = newSongReleaseYear;
            songsRepo.Save(song);
            Console.WriteLine("Song edited successfully!");
            Console.ReadKey(true);


        }
    }
}

