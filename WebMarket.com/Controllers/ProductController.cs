using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;

namespace WebMarket.com.Controllers
{
    public class ProductController : Controller
    {
        WebMarket_Entity db;
        ProductRepository productRepository;
        FeaturesRepository featuresRepository;
        ProductGalleryRepository productGalleryRepository;
        public ProductController()
        {
            db = new WebMarket_Entity();
            productRepository = new ProductRepository(db);
            featuresRepository = new FeaturesRepository(db);
            productGalleryRepository = new ProductGalleryRepository(db);
        }
        /*==================================================
         * OPEN SINGLE PRODUCT PAGE FOR DISPLAY PRODUCT DETAIL
         ===================================================*/
        public ActionResult ProductPage(int id)
        {
            Product product = productRepository.GetProductByID(id);
            if (product == null)
            {
                return RedirectToAction("NotFound","Error");
            }
            ViewBag.PageTitle = "محصول";
            return View(product);
        }
        /*==================================================
         * partial view for display product gallery
         ===================================================*/
        public ActionResult ProductGallery(Product product)
        {
            return PartialView(product);
        }
        /*==================================================
         * partial view for display form for add product to
         * shop card
         ===================================================*/
        public ActionResult AddToBasketForm(Product product)
        {
            AddToBasketViewModel ViewModel = new AddToBasketViewModel()
            {
                Features = featuresRepository.GetFeatures() as List<Features>,
                Product_Features = product.Product_Features.ToList(),
                ProductID = product.ProductID
            };
            ViewBag.Available = product.Available;
            return PartialView(ViewModel);
        }
        /*==================================================
         * add product to shop card
         ===================================================*/
        [HttpPost]
        public string AddToBasketForm(AddToBasketViewModel ViewModel)
        {
            List<ShopCartModel> shopCartModels;
            ShopCartModel model = new ShopCartModel()
            {
                Number = ViewModel.Number,
                Product = productRepository.GetProductByID(ViewModel.ProductID),
                ProductGalleryList = productGalleryRepository.GetGalleryByProductID(ViewModel.ProductID).ToList(),
                FeaturesSummeries_Temp = ViewModel.FeaturesSummeries_Temp
            };
            if (Session["ShopCart"] == null)
            {
                shopCartModels = new List<ShopCartModel>();
                shopCartModels.Add(model);
                Session["ShopCart"] = shopCartModels;
            }
            else
            {
                shopCartModels = (List<ShopCartModel>)Session["ShopCart"];
                shopCartModels.Add(model);
                Session["ShopCart"] = shopCartModels;
            }
            return "success.";
        }
        /*==========================================
         * display shop card in partial view
         ===========================================*/
        public ActionResult ShopCardPanel()
        {
            return PartialView(Session["ShopCart"] as List<ShopCartModel>);
        }
        /*==========================================
         * remove item in shop card
         ===========================================*/
        [HttpPost]
        public string RemoveProductFromShopCard(int id)
        {
            List<ShopCartModel> models = Session["ShopCart"] as List<ShopCartModel>;
            models.RemoveAt(id);
            return "success.";
        }
        /*==========================================
         * display related product in same group
         * 1-filtering by product grouping
         ===========================================*/
        public ActionResult RelatedProduct(int ProductID)
        {
            var product = productRepository.GetProductByID(ProductID);
            var relatedProduct = productRepository.GetRelatedProductByGroupID(product.Product_Groups.Select(pg => pg.GroupID).ToList(), product.ProductID);
            return PartialView(relatedProduct);
        }







        //dispose area
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                productRepository.Dispose();
                featuresRepository.Dispose();
                productGalleryRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}