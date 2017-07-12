using BlackSound_playlist_manager.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BlackSound_playlist_manager.Repository
{
    public class SongsArtistsRepository : BaseRepository<SongsArtists>
    {
        public SongsArtistsRepository(string filePath) : base(filePath)
        {}

        public override void PopulateEntity(StreamReader sr, SongsArtists item)
        {
            item.Id = Convert.ToInt32(sr.ReadLine());
            item.SongId = Convert.ToInt32(sr.ReadLine());
            item.ArtistId = Convert.ToInt32(sr.ReadLine());
        }

        public override void WriteEntity(StreamWriter sw, SongsArtists item)
        {
            sw.WriteLine(item.Id);
            sw.WriteLine(item.SongId);
            sw.WriteLine(item.ArtistId);
        }
    }
}
