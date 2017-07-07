using BlackSound_playlist_manager.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackSound_playlist_manager.Repository
{
    public class BaseRepository<T> where T : Entity.BaseEntity, new()
    {
        protected readonly string filePath;

        public BaseRepository(string filePath)
        {
            this.filePath = filePath;
        }


    }
}
