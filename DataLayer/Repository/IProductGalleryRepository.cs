using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public interface IProductGalleryRepository:IDisposable
    {
        IEnumerable<ProductGallery> GetGalleryByProductID(int id);
        bool Add(ProductGallery productGallery);
        bool Addrange(List<ProductGallery> productGalleryList);
        void Save();
    }
    public class ProductGalleryRepository : IProductGalleryRepository
    {
        WebMarket_Entity db;
        public ProductGalleryRepository(WebMarket_Entity db)
        {
            this.db = db;
        }
        public bool Add(ProductGallery productGallery)
        {
            try
            {
                db.Entry(productGallery).State = EntityState.Added;
                Save();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
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

        public void Save()
        {
            db.SaveChanges();
        }

        public bool Addrange(List<ProductGallery> productGalleryList)
        {
            try
            {
                db.ProductGallery.AddRange(productGalleryList);
                Save();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public IEnumerable<ProductGallery> GetGalleryByProductID(int id)
        {
            return db.ProductGallery.Where(pg => pg.ProductID == id);
        }
    }
}
