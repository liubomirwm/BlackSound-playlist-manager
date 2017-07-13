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

        public int Save(T item)
        {
            int returnId;
            if (item.Id > 0)
            {
                returnId = Update(item);
            }
            else
            {
                returnId = Insert(item);
            }
            return returnId;
        }

        public int Insert(T item)
        {
            item.Id = GetNextId();
            FileStream fs = new FileStream(this.filePath, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            try
            {
                WriteEntity(sw, item);
                return item.Id;
            }
            finally
            {
                sw.Dispose();
                fs.Dispose();
            }
        }

        public int Update(T item)
        {
            FileStream ifs = new FileStream(this.filePath, FileMode.OpenOrCreate);
            StreamReader sr = new StreamReader(ifs);
            string tempFilePath = "temp." + this.filePath;
            FileStream ofs = new FileStream(tempFilePath, FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(ofs);
            try
            {
                while (!sr.EndOfStream)
                {
                    T currentItem = new T();
                    PopulateEntity(sr, currentItem);
                    if (currentItem.Id == item.Id)
                    {
                        WriteEntity(sw, item);
                    }
                    else
                    {
                        WriteEntity(sw, currentItem);
                    }
                }
            }
            finally
            {
                sr.Dispose();
                ifs.Dispose();
                sw.Dispose();
                ofs.Dispose();
            }

            File.Delete(this.filePath);
            File.Move(tempFilePath, this.filePath);
            return item.Id;
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

        public List<T> GetAll(Func<T, bool> filter = null)
        {
            List<T> items = new List<T>();
            FileStream fs = new FileStream(this.filePath, FileMode.OpenOrCreate);
            StreamReader sr = new StreamReader(fs);
            try
            {
                while (!sr.EndOfStream)
                {
                    T item = new T();
                    PopulateEntity(sr, item);
                    items.Add(item);
                }

                if (filter == null)
                {
                    return items;
                }
                else
                {
                    var query = items.Where(filter);

                    List<T> filteredItems = new List<T>();
                    foreach (var item in query)
                    {
                        filteredItems.Add(item);
                    }
                    return filteredItems;
                }
            }
            finally
            {
                sr.Dispose();
                fs.Dispose();
            }
        }

        public void Delete(T itemForDeletion)
        {
            string tempFilePath = "temp." + this.filePath;
            FileStream ifs = new FileStream(this.filePath, FileMode.OpenOrCreate);
            StreamReader sr = new StreamReader(ifs);

            FileStream ofs = new FileStream(tempFilePath, FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(ofs);
            try
            {
                while (!sr.EndOfStream)
                {
                    T currentItem = new T();
                    PopulateEntity(sr, currentItem);
                    if (currentItem.Id != itemForDeletion.Id)
                    {
                        WriteEntity(sw, currentItem);
                    }
                }
            }
            finally
            {
                sr.Dispose();
                ifs.Dispose();
                sw.Dispose();
                ofs.Dispose();
            }

            File.Delete(this.filePath);
            File.Move(tempFilePath, this.filePath);
        }

        public bool EntityExists(Func<T, bool> filter)
        {
            List<T> items = new List<T>();
            FileStream fs = new FileStream(this.filePath, FileMode.OpenOrCreate);
            StreamReader sr = new StreamReader(fs);
            try
            {
                while (!sr.EndOfStream)
                {
                    T item = new T();
                    PopulateEntity(sr, item);
                    items.Add(item);
                }

                bool isExisting = items.Any(filter);
                return isExisting;
            }
            finally
            {
                sr.Dispose();
                fs.Dispose();
            }
        }

        public abstract void PopulateEntity(StreamReader sr, T item);
        public abstract void WriteEntity(StreamWriter sw, T item);
    }
}
