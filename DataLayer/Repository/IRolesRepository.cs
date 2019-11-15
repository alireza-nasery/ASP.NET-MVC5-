using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace DataLayer
{
    public interface IRolesRepository:IDisposable
    {
        IEnumerable<Roles> GetRoles();
        Roles FindRolesByID(int ID);
    }
    public class RolesRepository : IRolesRepository
    {
        WebMarket_Entity db;
        public RolesRepository(WebMarket_Entity db)
        {
            this.db = db;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            disposed = true;
        }
        public IEnumerable<Roles> GetRoles()
        {
            return db.Roles.ToList();
        }

        public Roles FindRolesByID(int ID)
        {
            return db.Roles.Find(ID);
        }
    }
}
