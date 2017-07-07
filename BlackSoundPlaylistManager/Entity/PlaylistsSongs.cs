using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackSound_playlist_manager.Entity
{
    public class PlaylistsSongs : Entity.BaseEntity
    {
        public int PlaylistId { get; set; }
        public int SongId { get; set; }
    }
}
