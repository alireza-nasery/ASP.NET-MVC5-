using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public interface IOrder_CustomerAddressRepository : IDisposable
    {
        IEnumerable<Order_CustomerAddress> GetAll();
        Order_CustomerAddress GetOrder_CustomerByGuid(string Guid);
        Order_CustomerAddress GetOrder_CustomerById(int id);

        double GetFinalPriceOrderCustomer(int id);
        bool SetIsFinallyTrue(int id);
        bool Add(Order_CustomerAddress order_CustomerAddress);
        bool Delete(Order_CustomerAddress order_CustomerAddress);
        void Save();
    }
    public class Order_CustomerAddressRepositiry : IOrder_CustomerAddressRepository
    {
        WebMarket_Entity db;
        public Order_CustomerAddressRepositiry(WebMarket_Entity db)
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

        public bool Add(Order_CustomerAddress order_Customer)
        {
            try
            {
                db.Entry(order_Customer).State = EntityState.Added;
                Save();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Order_CustomerAddress GetOrder_CustomerByGuid(string Guid)
        {
            return db.Order_CustomerAddress.SingleOrDefault(oc => oc.Guid == Guid);
        }

        public double GetFinalPriceOrderCustomer(int id)
        {
            var order_customerAddress = db.Order_CustomerAddress.Find(id);
            var total = 0.0;
            foreach (var item in order_customerAddress.Order_CustomerProducts)
            {
                total += db.Product.Find(item.ProductID).Price * item.Number;
            }
            return total;
        }

        public bool SetIsFinallyTrue(int id)
        {
            try
            {
                var order_customer = db.Order_CustomerAddress.Find(id);
                order_customer.IsFinally = true;
                Save();
                return true;
            }
            catch (Exception e)
            {
                var order_customer = db.Order_CustomerAddress.Find(id);
                order_customer.Guid = "SPECIAL-CASE-OCCURE-ISFINALLY";
                Save();
                return false;
            }
        }

        public IEnumerable<Order_CustomerAddress> GetAll()
        {
            return db.Order_CustomerAddress;
        }

        public bool Delete(Order_CustomerAddress order_Customer)
        {
            try
            {
                db.Order_CustomerAddress.Remove(order_Customer);
                Save();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Order_CustomerAddress GetOrder_CustomerById(int id)
        {
            return db.Order_CustomerAddress.Find(id);
        }
    }
}
