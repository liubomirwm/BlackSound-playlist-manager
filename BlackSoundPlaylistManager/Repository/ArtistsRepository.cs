using BlackSound_playlist_manager.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BlackSound_playlist_manager.Repository
{
    public class ArtistsRepository : BaseRepository<Artist>
    {
        public ArtistsRepository(string filePath) : base(filePath)
        {}

        public override void PopulateEntity(StreamReader sr, Artist item)
        {
            item.Id = Convert.ToInt32(sr.ReadLine());
            item.Name = sr.ReadLine();
        }

        public override void WriteEntity(StreamWriter sw, Artist item)
        {
            sw.WriteLine(item.Id);
            sw.WriteLine(item.Name);
        }
    }
}
