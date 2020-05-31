using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;

namespace WebMarket.com.Areas.Admin.Controllers
{
    public class DisplayOrdersController : Controller
    {
        WebMarket_Entity db = new WebMarket_Entity();
        Order_CustomerAddressRepositiry order_CustomerRepositiry;
        ProductRepository productRepository;
        public DisplayOrdersController()
        {
            order_CustomerRepositiry = new Order_CustomerAddressRepositiry(db);
            productRepository = new ProductRepository(db);
        }
        /*=========================================
         * open orders page and display them
         ==========================================*/
        public ActionResult DisplayOrdersPage()
        {
            ViewBag.PageTitle = "سفارشات";
            return View(order_CustomerRepositiry.GetAll());
        }
        /*=========================================
         * delete order by order_customer id
         ==========================================*/
        [ValidateAntiForgeryToken]
        [HttpPost]
        public string DeleteOrder(int id)
        {
            var order_customer = order_CustomerRepositiry.GetOrder_CustomerById(id);
            if (order_customer == null)
            {
                return "order do not exist.";
            }
            if (order_CustomerRepositiry.Delete(order_customer))
            {
                return "success.";
            }
            return "server return fasle for delete current order.";
        }
        /*=========================================
         * open order_customer details page
         ==========================================*/
        public ActionResult OrderDetails(int id)
        {
            var order_customer = order_CustomerRepositiry.GetOrder_CustomerById(id);
            if (order_customer == null)
            {
                return HttpNotFound();
            }
            return View(order_customer);
        }







        //dispose area
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                order_CustomerRepositiry.Dispose();
                productRepository.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}