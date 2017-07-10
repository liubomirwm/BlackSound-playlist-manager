using BlackSound_playlist_manager.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackSound_playlist_manager.Repository
{
    public abstract class BaseRepository<T> where T : Entity.BaseEntity, new()
    {
        protected readonly string filePath;

        public BaseRepository(string filePath)
        {
            this.filePath = filePath;
        }

        public void Save(T item)
        {
            if (item.Id > 0)
            {
                //Update(item);
            }
            else
            {
                Insert(item);
            }
        }

        public void Insert(T item)
        {
            item.Id = GetNextId();
            FileStream fs = new FileStream(this.filePath, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            try
            {
                WriteEntity(sw, item);
            }
            finally
            {
                sw.Dispose();
                fs.Dispose();
            }
        }

        private int GetNextId()
        {
            FileStream fs = new FileStream(this.filePath, FileMode.OpenOrCreate);
            StreamReader sr = new StreamReader(fs);
            int id = 1;
            try
            {
                while (!sr.EndOfStream)
                {
                    T item = new T();
                    PopulateEntity(sr, item);
                    if (id <= item.Id)
                    {
                        id = item.Id + 1;
                    }
                }
            }
            finally
            {
                sr.Dispose();
                fs.Dispose();
            }

            return id;
        }

        public abstract void PopulateEntity(StreamReader sr, T item);
        public abstract void WriteEntity(StreamWriter sw, T item);
    }
}
