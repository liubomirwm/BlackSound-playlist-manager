using BlackSound_playlist_manager.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BlackSound_playlist_manager.Repository
{
    public class PlaylistsSongsRepository : BaseRepository<PlaylistsSongs>
    {
        public PlaylistsSongsRepository(string filePath) : base(filePath)
        { }

        public override void PopulateEntity(StreamReader sr, PlaylistsSongs item)
        {
            item.Id = Convert.ToInt32(sr.ReadLine());
            item.PlaylistId = Convert.ToInt32(sr.ReadLine());
            item.SongId = Convert.ToInt32(sr.ReadLine());
        }

        public override void WriteEntity(StreamWriter sw, PlaylistsSongs item)
        {
            sw.WriteLine(item.Id);
            sw.WriteLine(item.PlaylistId);
            sw.WriteLine(item.SongId);
        }
    }
}
