using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public interface IProductRepository : IDisposable
    {
        IEnumerable<Product> GetProducts();
        IEnumerable<Product> GetDiscountedProducts();
        IEnumerable<Product> GetNewProducts();
        IEnumerable<Product> GetMostPopularProducts();
        IEnumerable<Product> GetRelatedProductByGroupID(List<int> GroupID,int productId);
        Product GetProductByID(int ID);
        bool Add(Product product);
        bool Update(Product product);
        bool Delete(Product product);
        bool IsProductExistByID(int id);
        Product FindProductByID(int ID);
        Product FindProductWithGuid(string Guid);
        void Save();
    }
    public class ProductRepository : IProductRepository
    {
        WebMarket_Entity db;
        public ProductRepository(WebMarket_Entity db)
        {
            this.db = db;
        }
        public bool Add(Product product)
        {
            try
            {
                db.Entry(product).State = EntityState.Added;
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

        public Product FindProductWithGuid(string Guid)
        {
            return db.Product.SingleOrDefault(p => p.Guid == Guid);
        }

        public IEnumerable<Product> GetProducts()
        {
            var data = db.Product.ToList();
            return db.Product.ToList();
        }

        public bool Delete(Product Product)
        {
            try
            {
                db.Entry(Product).State = EntityState.Deleted;
                Save();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool IsProductExistByID(int id)
        {
            return db.Product.Any(p => p.ProductID == id);
        }

        public Product FindProductByID(int ID)
        {
            return db.Product.SingleOrDefault(p => p.ProductID == ID);
        }

        public bool Update(Product Product)
        {
            try
            {
                db.Entry(Product).State = EntityState.Modified;
                Save();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public Product GetProductByID(int ID)
        {
            return db.Product.Find(ID);
        }

        public IEnumerable<Product> GetDiscountedProducts()
        {
            return db.Product.Where(p => p.OldPrice != 0 && p.OldPrice != null);
        }

        public IEnumerable<Product> GetNewProducts()
        {
            return db.Product.OrderByDescending(p => p.CreateDate).Take(8);
        }

        public IEnumerable<Product> GetMostPopularProducts()
        {
            return db.Product.Where(p => p.Rate >= 4).OrderByDescending(p => p.CreateDate).Take(4);
        }

        public IEnumerable<Product> GetRelatedProductByGroupID(List<int> GroupsId, int productId)
        {
            List<Product> targetList = new List<Product>();
            foreach (var item in GroupsId)
            {
                foreach (var productid in db.Product_Groups.Where(pg => pg.GroupID == item && pg.ProductID != productId).Select(pg => pg.ProductID).ToList())
                {
                    targetList.Add(db.Product.Find(productid));
                }
            }
            return targetList.Distinct().ToList().Take(4);
        }
    }
}
