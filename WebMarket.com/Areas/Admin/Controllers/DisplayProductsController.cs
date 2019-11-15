using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer;

namespace WebMarket.com.Areas.Admin.Controllers
{
    public class DisplayProductsController : Controller
    {
        WebMarket_Entity db = new WebMarket_Entity();
        ProductRepository productRepository;
        public DisplayProductsController()
        {
            productRepository = new ProductRepository(db);
        }
        /*==============================================
         * this function open products page in admin panel
         * for delete and edit product
         ===============================================*/
        public ActionResult DisplayProductsPage()
        {
            ViewBag.PageTitle = "محصولات";
            return View(productRepository.GetProducts());
        }
        /*==============================================
         * function delete product whith id
         ===============================================*/
        [ValidateAntiForgeryToken]
        [HttpPost]
        public string DeleteProduct(int id)
        {
            if (productRepository.IsProductExistByID(id))
            {
                try
                {
                    //without CaseCade
                    //using (WebMarket_Entity db = new WebMarket_Entity())
                    //{
                    //    db.Product_Features.RemoveRange(db.Product_Features.Where(pf => pf.ProductID == id));
                    //    db.Product_Groups.RemoveRange(db.Product_Groups.Where(pg => pg.ProductID == id));
                    //    db.ProductGallery.RemoveRange(db.ProductGallery.Where(pg => pg.ProductID == id));
                    //    db.SaveChanges();
                    //}
                }
                catch (Exception e)
                {
                    return "server return exception for delete product rel.";
                }
                Product product = db.Product.Find(id);
                if (productRepository.Delete(product))
                {
                    return "success.";
                }
                return "server return fasle for delete current product.";
            }
            else
            {
                return "product do not exist.";
            }
        }
        /*==============================================
         * open edit page for product
         ===============================================*/
        [HttpGet]
        public ActionResult EditProduct(int id)
        {
            Product product = productRepository.FindProductByID(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            var data = new List<string>();
            using (WebMarket_Entity db = new WebMarket_Entity())
            {
                foreach (var item in product.Product_Features.Select(pf => pf.Value))
                {
                    foreach (var item_2 in db.FeaturesSummery.Where(fs => fs.FS_Name == item))
                    {
                        data.Add(item_2.FS_ID.ToString());
                    }
                }
            }
            ProductViewModel productViewModel = new ProductViewModel()
            {
                ProductID = product.ProductID,
                FullDescription = product.FullDescription,
                OldPrice = product.OldPrice,
                NowPrice = product.Price,
                ProductName = product.ProductName,
                Rate = product.Rate,
                ShortDescription = product.ShortDescription,
                FeaturesSummeries_Temp = data,
                GroupsID = product.Product_Groups.Select(pg => pg.GroupID).ToList(),
                Available = product.Available
            };
            productViewModel = SetGallery64(productViewModel, product);
            return View(productViewModel);
        }


        /*==============================================
         * post data to this function for edit product
         ===============================================*/
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditProduct(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                Product product = productRepository.FindProductByID(productViewModel.ProductID);
                if (product == null)
                {
                    return HttpNotFound();
                }
                else
                {

                    TableService.ChangeTable_PGR(productViewModel.GroupsID, product.ProductID);
                    TableService.ChangeTable_PFE(productViewModel.FeaturesSummeries_Temp, product.ProductID);
                    TableService.ChangeTable_PGA(productViewModel.Gallery, product.ProductID);
                    productViewModel = SetGallery64(productViewModel, product);
                    product.ProductName = productViewModel.ProductName;
                    product.OldPrice = productViewModel.OldPrice;
                    product.Price = productViewModel.NowPrice;
                    product.Rate = productViewModel.Rate;
                    product.ShortDescription = productViewModel.ShortDescription;
                    product.Available = productViewModel.Available;
                    product.FullDescription = productViewModel.FullDescription;
                    productRepository.Update(product);

                    ViewBag.Result = 100;
                    ViewBag.Icon = "";
                    ViewBag.Description = "کاربر مورد نظر با موفقیت تغییر داده شد";
                    ViewBag.Type = "success";
                    return View(productViewModel);

                }
            }
            return View(productViewModel);
        }
        /*==============================================
         * custome func for this class and set image in 
         * gallery64 in viewmodel
         ===============================================*/
        private ProductViewModel SetGallery64(ProductViewModel viewModel, Product product)
        {
            var data = new List<string>();
            for (int i = 0; i < 3; i++)
            {
                var pro = product.ProductGallery.SingleOrDefault(pg => pg.HomeNumber == i);
                data.Add(pro != null ? Convert.ToBase64String(pro.Image) : null);
            }
            viewModel.Gallery64 = data;
            return viewModel;
        }



        //dispose area
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                productRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}