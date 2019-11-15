using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public interface IGroupRepository:IDisposable
    {
        IEnumerable<Group> GetGroup();
    }
    /// <summary>
    /// just make instance for [ShopController]
    /// </summary>
    public class GroupRepository : IGroupRepository
    {
        WebMarket_Entity db;
        public GroupRepository(WebMarket_Entity db)
        {
            this.db = db;
        }
        private bool disposing = false;
        public void Dispose(bool disposing)
        {
            if (!disposing)
            {
                db.Dispose();
            }
            this.disposing = true;
        }
        public void Dispose()
        {
            Dispose(disposing);
            GC.SuppressFinalize(this);
        }

        public IEnumerable<Group> GetGroup()
        {
            return db.Group.ToList();
        }
    }
}
