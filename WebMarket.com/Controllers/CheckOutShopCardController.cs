using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using WebMarket.com.ZarinPal_MVC;

namespace WebMarket.com.Controllers
{
    public class CheckOutShopCardController : Controller
    {
        WebMarket_Entity db;
        Order_CustomerAddressRepositiry order_CustomerAddressRepositiry;
        Order_CustomerProductsRepository order_CustomerProductsRepository;
        public CheckOutShopCardController()
        {
            db = new WebMarket_Entity();
            order_CustomerAddressRepositiry = new Order_CustomerAddressRepositiry(db);
            order_CustomerProductsRepository = new Order_CustomerProductsRepository(db);
        }
        /*=============================================
         * get information from customer for send product
         ==============================================*/
        public ActionResult Address()
        {
            if (Session["ShopCart"] == null)
            {
                ViewBag.Description = "سبد خرید شما خالی میباشد";
                ViewBag.ModalCode = 201;
                return RedirectToAction("Index", "Home");
            }
            ViewBag.PageTitle = "اطلاعات مشتری";
            return View();
        }
        /*=============================================
         * post customer data to this function
         ==============================================*/
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Address([Bind(Include = "FirstName,LastName,Address,Email,PhoneNumber,City,PostalCode,Description")] Order_CustomerAddress order_CustomerAddres)
        {
            if (ModelState.IsValid)
            {
                Order_CustomerAddress model = new Order_CustomerAddress
                {
                    Address = order_CustomerAddres.Address,
                    City = order_CustomerAddres.City,
                    CreateDate = DateTime.Now,
                    Description = order_CustomerAddres.Description,
                    Email = order_CustomerAddres.Email,
                    FirstName = order_CustomerAddres.FirstName,
                    LastName = order_CustomerAddres.LastName,
                    PostalCode = order_CustomerAddres.PostalCode,
                    PhoneNumber = order_CustomerAddres.PhoneNumber,
                    IsFinally = false,
                    Guid = Guid.NewGuid().ToString(),
                    Server_Error_IsFinally = false
                };
                if (order_CustomerAddressRepositiry.Add(model))
                {
                    var ShopCart = Session["ShopCart"] as List<ShopCartModel>;
                    if (ShopCart == null)
                    {
                        return View("NotFound");
                    }
                    ViewBag.Order_CustomerGUID = model.Guid;
                    return View("SubmitShopBascket", ShopCart);
                }
            }
            return View(order_CustomerAddres);
        }
        /*=============================================
         * 1-add products to db for current order_customer
         * 2-send customer to zarinpal port
         ==============================================*/
        [ValidateAntiForgeryToken]
        [HttpPost]
        public string SubmitShopBascket(string guid)
        {
            var order_customer = order_CustomerAddressRepositiry.GetOrder_CustomerByGuid(guid);
            if (order_customer == null)
            {
                return "order_customer is null";
            }
            //get total $
            List<ShopCartModel> shopCart = Session["ShopCart"] as List<ShopCartModel>;
            var total = 0.0;
            foreach (var item in shopCart)
            {
                total += item.Product.Price * item.Number;
            }
            //add product to list
            var order_CustomerProductsList = new List<Order_CustomerProducts>();
            foreach (var item in shopCart)
            {
                Order_CustomerProducts order_Products = new Order_CustomerProducts()
                {
                    Number = item.Number,
                    ProductID = item.Product.ProductID,
                    Order_CustomerAddressID = order_customer.Order_CustomerAddressID,
                    Features = Wrench.GetStringFromList(item.FeaturesSummeries_Temp),
                    ProductName=item.Product.ProductName
                };
                order_CustomerProductsList.Add(order_Products);
            }
            //add list to db
            if (order_CustomerProductsRepository.AddRange(order_CustomerProductsList))
            {
                // add features product ---> success
                //go to bank
                System.Net.ServicePointManager.Expect100Continue = false;
                ZarinPal_MVC.PaymentGatewayImplementationServicePortTypeClient zp = new ZarinPal_MVC.PaymentGatewayImplementationServicePortTypeClient();
                string Authority;
                int Status = zp.PaymentRequest("YOUR-ZARINPAL-MERCHANT-CODE", (int)total, "خرید از فروشگاه ایمترنتی وب مارکت",
                    "alirezanaseri383@yahoo.com", "09050613342", $"{System.Configuration.ConfigurationManager.AppSettings["MyDomain"]}/Home/Verify?id=" + order_customer.Order_CustomerAddressID, out Authority);
                if (Status == 100)
                {
                    Response.Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + Authority);
                }
                return "Error:" + Status;
            }
            else
            {
                return "order_products dont add to db";
            }
        }








        //dispose area
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                order_CustomerAddressRepositiry.Dispose();
                order_CustomerProductsRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}