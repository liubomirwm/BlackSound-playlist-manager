using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackSound_playlist_manager.Entity
{
    public class Song : BaseEntity
    {
        public string Title { get; set; }
        public short Year { get; set; }
    }
}
