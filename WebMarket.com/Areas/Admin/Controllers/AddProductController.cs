using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;

namespace WebMarket.com.Areas.Admin.Controllers
{
    public class AddProductController : Controller
    {
        WebMarket_Entity db;
        ProductRepository productRepository;
        ProductGalleryRepository productGalleryRepository;
        public AddProductController()
        {
            db = new WebMarket_Entity();
            productRepository = new ProductRepository(db);
            productGalleryRepository = new ProductGalleryRepository(db);
        }
        /*============================================
         * open add product page
         =============================================*/
        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.PageTitle = "افزودن محصول";
            return View(new ProductViewModel());
        }
        /*============================================
         * in this function when user post data for add
         * product and rel btw PG & PF PG
         =============================================*/
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Add(ProductViewModel ViewModel)
        {
            //if (ViewModel.FeaturesSummeries_Temp == null)
            //{
            //    ViewBag.Result = 101;
            //    ViewBag.Message = "از جدول ویؤگی ویؤگی مورد نظر خود را انتخاب کنید";
            //    ViewBag.Type = "warning";
            //    ViewBag.Icon = "";
            //    return View(ViewModel);
            //}
            if (ModelState.IsValid)
            {
                //create product
                Product product = new Product()
                {
                    Guid = Guid.NewGuid().ToString(),
                    FullDescription = ViewModel.FullDescription,
                    OldPrice = ViewModel.OldPrice,
                    Price = ViewModel.NowPrice,
                    ProductName = ViewModel.ProductName,
                    Rate = ViewModel.Rate,
                    ShortDescription = ViewModel.ShortDescription,
                    CreateDate = DateTime.Now,
                    Available = true
                };
                if (productRepository.Add(product))
                {
                    int productId = productRepository.FindProductWithGuid(product.Guid).ProductID;

                    TableService.ChangeTable_PGR(ViewModel.GroupsID, productId);
                    TableService.ChangeTable_PFE(ViewModel.FeaturesSummeries_Temp, productId);
                    TableService.ChangeTable_PGA(ViewModel.Gallery, productId);
                    
                    //success
                    ViewBag.Result = 100;
                    ViewBag.Message = "محصول مورد نظر با موفقیت اضافه شد";
                    ViewBag.Type = "success";
                    ViewBag.Icon = "add_alert";
                    return View(new ProductViewModel());
                }
                ViewBag.Result = 101;
                ViewBag.Message = "محصول مورد نظر ذخیره نشد";
                ViewBag.Type = "warning";
                ViewBag.Icon = "";
                return View(ViewModel);
            }
            return View(ViewModel);
        }






        //dispose area
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                productRepository.Dispose();
                productGalleryRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}