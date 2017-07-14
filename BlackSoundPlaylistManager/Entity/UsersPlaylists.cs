using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackSound_playlist_manager.Entity
{
    public class UsersPlaylists : BaseEntity
    {
        public int UserId { get; set; }
        public int PlaylistId { get; set; }
    }
}
