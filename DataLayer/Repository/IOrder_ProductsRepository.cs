using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public interface IOrder_CustomerProductsRepository : IDisposable
    {
        bool Add(Order_CustomerProducts order_CustomerProducts);
        bool AddRange(List<Order_CustomerProducts> order_CustomerProductsList);
        void Save();
    }
    public class Order_CustomerProductsRepository : IOrder_CustomerProductsRepository
    {
        WebMarket_Entity db;
        public Order_CustomerProductsRepository(WebMarket_Entity db)
        {
            this.db = db;
        }
        public void Save()
        {
            db.SaveChanges();
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
        public bool Add(Order_CustomerProducts Order_CustomerProducts)
        {
            try
            {
                db.Entry(Order_CustomerProducts).State = EntityState.Added;
                Save();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AddRange(List<Order_CustomerProducts> Order_CustomerProductsList)
        {
            try
            {
                db.Order_CustomerProducts.AddRange(Order_CustomerProductsList);
                Save();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
