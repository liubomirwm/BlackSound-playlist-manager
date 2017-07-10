using BlackSound_playlist_manager.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BlackSound_playlist_manager.Repository
{
    public class SongsRepository : BaseRepository<Song>
    {
        public SongsRepository(string filePath) : base(filePath)
        {}

        public override void PopulateEntity(StreamReader sr, Song song)
        {
            song.Id = Convert.ToInt32(sr.ReadLine());
            song.Title = sr.ReadLine();
            song.Year = Convert.ToInt16(sr.ReadLine());
        }

        public override void WriteEntity(StreamWriter sw, Song song)
        {
            sw.WriteLine(song.Id);
            sw.WriteLine(song.Title);
            sw.WriteLine(song.Year);
        }
    }
}
