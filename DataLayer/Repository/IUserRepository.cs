using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DataLayer
{
    public interface IUserRepository : IDisposable
    {
        IEnumerable<User> GetUsers();
        IEnumerable<User> WhereUserByRoleID(int RoleID);
        User GetUserID(int ID);
        bool Add(User user);
        bool Update(User user);
        bool Delete(User user);
        bool IsUserExistByID(int id);
        bool IsUserExistByEmail(string Email);
        bool IsUserExistByUsernameOrEmail(string Username,string Email);
        bool IsUserExistByRoleID(int RoleID);
        User FindUserByID(int ID);
        User FindUserByActiveCode(string ActiveCode);
        User FindUserByUsernameAndEmail(string Username, string Email);
        User FindUserByEmail(string Email);
        User FindUserByEmailOrUsername(string IdentityName);
        void Save();
    }
    public class UserRepository : IUserRepository
    {
        WebMarket_Entity db;
        public UserRepository(WebMarket_Entity db)
        {
            this.db = db;
        }
        public bool Add(User user)
        {
            try
            {
                db.Entry(user).State = EntityState.Added;
                Save();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public User GetUserID(int ID)
        {
            return db.User.SingleOrDefault(u => u.ID == ID);
        }


        public bool Delete(User user)
        {
            try
            {
                db.Entry(user).State = EntityState.Deleted;
                Save();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public IEnumerable<User> GetUsers()
        {
            return db.User.ToList();
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public bool Update(User user)
        {
            try
            {
                db.Entry(user).State = EntityState.Modified;
                Save();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool IsUserExistByID(int id)
        {
            return db.User.Any(u => u.ID == id);
        }
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public User FindUserByID(int ID)
        {
            return db.User.Find(ID);
        }

        public bool IsUserExistByEmail(string Email)
        {
            return db.User.Any(u => u.Email.ToLower() == Email.Trim().ToLower());
        }

        public User FindUserByActiveCode(string ActiveCode)
        {
            return db.User.SingleOrDefault(u => u.ActiveCode == ActiveCode);
        }

        public User FindUserByUsernameAndEmail(string Username, string Email)
        {
            return db.User.SingleOrDefault(u => u.Usename == Username || u.Email == Email);
        }

        public User FindUserByEmail(string Email)
        {
            return db.User.SingleOrDefault(u => u.Email.ToLower() == Email.Trim().ToLower());
        }

        public bool IsUserExistByRoleID(int RoleID)
        {
            return db.User.Any(u => u.RoleID == RoleID);
        }

        public bool IsUserExistByUsernameOrEmail(string Username, string Email)
        {
            return db.User.Any(u => u.Usename == Username || u.Email == Email);
        }

        public IEnumerable<User> WhereUserByRoleID(int RoleID)
        {
            return db.User.Where(u => u.RoleID == RoleID).ToList();
        }

        public User FindUserByEmailOrUsername(string String_Data)
        {
            return db.User.SingleOrDefault(u => u.Email == String_Data || u.Usename == String_Data);
        }
    }
}
