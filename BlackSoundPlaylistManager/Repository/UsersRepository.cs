using BlackSound_playlist_manager.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackSound_playlist_manager.Repository
{
    public class UsersRepository : BaseRepository<User>
    {
        public UsersRepository(string filePath) : base(filePath)
        { }

        internal User GetUserByUserNameAndPassword(string email, string password)
        {
            FileStream fs = new FileStream(this.filePath, FileMode.OpenOrCreate);
            StreamReader sr = new StreamReader(fs);

            try
            {
                while (!sr.EndOfStream)
                {
                    User currentUser = new User();
                    currentUser.Id = Convert.ToInt32(sr.ReadLine());
                    currentUser.Email = sr.ReadLine();
                    currentUser.Password = sr.ReadLine();
                    currentUser.DisplayName = sr.ReadLine();
                    currentUser.IsAdministrator = Convert.ToBoolean(sr.ReadLine());
                    if (currentUser.Email == email && currentUser.Password == password)
                    {
                        return currentUser;
                    }
                }
                return null; //If no credentials found
            }
            finally
            {
                sr.Dispose();
                fs.Dispose();
            }
        }

        public override void PopulateEntity(StreamReader sr, User item)
        {
            throw new NotImplementedException();
        }

        public override void WriteEntity(StreamWriter sw, User item)
        {
            throw new NotImplementedException();
        }
    }
}
