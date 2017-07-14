using BlackSound_playlist_manager.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BlackSound_playlist_manager.Repository
{
    public class UsersPlaylistsRepository : BaseRepository<UsersPlaylists>
    {
        public UsersPlaylistsRepository(string filePath) : base(filePath)
        {}

        public override void PopulateEntity(StreamReader sr, UsersPlaylists item)
        {
            item.Id = Convert.ToInt32(sr.ReadLine());
            item.UserId = Convert.ToInt32(sr.ReadLine());
            item.PlaylistId = Convert.ToInt32(sr.ReadLine());
        }

        public override void WriteEntity(StreamWriter sw, UsersPlaylists item)
        {
            sw.WriteLine(item.Id);
            sw.WriteLine(item.UserId);
            sw.WriteLine(item.PlaylistId);
        }
    }
}
