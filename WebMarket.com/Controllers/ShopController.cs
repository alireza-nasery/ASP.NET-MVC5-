using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;

namespace WebMarket.com.Controllers
{
    public class ShopController : Controller
    {
        WebMarket_Entity db;
        ProductRepository productRepository;
        FeaturesRepository featuresRepository;
        GroupRepository groupRepository;
        public ShopController()
        {
            db = new WebMarket_Entity();
            productRepository = new ProductRepository(db);
            featuresRepository = new FeaturesRepository(db);
            groupRepository = new GroupRepository(db);
        }
        //open shop page
        public ActionResult ShopPage()
        {
            ViewBag.TitlePage = "محصولات";
            return View();
        }
        //right slid for filtering grouping
        public ActionResult CategoriesPartialView()
        {
            return PartialView(groupRepository.GetGroup());
        }
        //right slide for fotering featires
        public ActionResult FeaturesPartialView()
        {
            return PartialView(featuresRepository.GetFeatures());
        }
        //product partial view
        public ActionResult ProductPartialView()
        {
            var data = productRepository.GetProducts();
            return PartialView(data);
        }




        public ActionResult test()
        {
            return View();
        }

        //dispose area
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                productRepository.Dispose();
                featuresRepository.Dispose();
                groupRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}