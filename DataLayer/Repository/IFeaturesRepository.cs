using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public interface IFeaturesRepository : IDisposable
    {
        IEnumerable<Features> GetFeatures();
    }
    public class FeaturesRepository : IFeaturesRepository
    {
        WebMarket_Entity db;
        public FeaturesRepository(WebMarket_Entity db)
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

        public IEnumerable<Features> GetFeatures()
        {
            return db.Features.ToList();
        }
    }
}
