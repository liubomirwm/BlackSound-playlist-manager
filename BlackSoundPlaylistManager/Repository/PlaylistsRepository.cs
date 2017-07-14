using BlackSound_playlist_manager.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BlackSound_playlist_manager.Repository
{
    public class PlaylistsRepository : BaseRepository<Playlist>
    {
        public PlaylistsRepository(string filePath) : base(filePath)
        {}

        public override void PopulateEntity(StreamReader sr, Playlist item)
        {
            item.Id = Convert.ToInt32(sr.ReadLine());
            item.Name = sr.ReadLine();
            item.Description = sr.ReadLine();
            item.IsPublic = Convert.ToBoolean(sr.ReadLine());
        }

        public override void WriteEntity(StreamWriter sw, Playlist item)
        {
            sw.WriteLine(item.Id);
            sw.WriteLine(item.Name);
            sw.WriteLine(item.Description);
            sw.WriteLine(item.IsPublic);
        }
    }
}
