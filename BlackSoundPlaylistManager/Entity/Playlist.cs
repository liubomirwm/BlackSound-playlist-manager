using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackSound_playlist_manager.Entity
{
    public class Playlist : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<int> Songs { get; set; }
        public bool IsPublic { get; set; }
    }
}
