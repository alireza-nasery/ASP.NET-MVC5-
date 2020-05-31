using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer;

namespace WebMarket.com.Areas.Admin.Controllers
{
    public class FeaturesAndGroupingController : Controller
    {
        WebMarket_Entity db = new WebMarket_Entity();
        [HttpGet]
        /*====================================
         * return one page : that page contains
         * js function that possible for all thing
         * do for features and grouping indb
         =====================================*/
        public ActionResult Index()
        {
            ViewBag.PageTitle = "گروه بندی و ویژگی ها";
            return View();
        }
        /*============================================
         * add grouping to group table in db
         =============================================*/
        [ValidateAntiForgeryToken]
        [HttpPost]
        public string AddGroup([Bind(Include = "GroupID,GroupName")] Group group)
        {
            if (ModelState.IsValid)
            {
                if (!db.Group.Any(g => g.GroupName == group.GroupName))
                {
                    db.Group.Add(group);
                    db.SaveChanges();
                    return "success.";
                }
                else
                {
                    return "already exist.";
                }
            }
            return "model not valid.";
        }
        /*============================================
         * return all group in db
         =============================================*/
        public ActionResult DisplayGrouping()
        {
            return PartialView(db.Group.ToList());
        }
        /*============================================
         * delete group with grouping id
         =============================================*/
        [ValidateAntiForgeryToken]
        [HttpPost]
        public string DeleteGroup(int id)
        {
            Group group = db.Group.Find(id);
            List<Product_Groups> Product_GroupsList = db.Product_Groups.Where(pg => pg.GroupID == group.GroupID).ToList();
            foreach (var item in Product_GroupsList.Select(pg => pg.ProductID))
            {
                var counter = db.Product_Groups.Where(pg => pg.ProductID == item).ToList().Count;
                if (counter == 1)
                {
                    //without Casecade
                    //db.Product_Groups.Remove(db.Product_Groups.Single(pg => pg.ProductID == item));
                    //delete product
                    //db.ProductGallery.RemoveRange(db.ProductGallery.Where(pg => pg.ProductID == item));
                    //db.Product_Features.RemoveRange(db.Product_Features.Where(pg => pg.ProductID == item));
                    db.Product.Remove(db.Product.Find(item));
                }
                else
                {
                    //db.Product_Groups.RemoveRange(db.Product_Groups.Where(pg => pg.ProductID == item && pg.GroupID == group.GroupID));
                }
            }
            db.Group.Remove(db.Group.Find(group.GroupID));
            db.SaveChanges();
            return "success.";
        }
        /*============================================
         * edit group with that object
         =============================================*/
        [ValidateAntiForgeryToken]
        [HttpPost]
        public string EditeGrouping([Bind(Include = "GroupID,GroupName")] Group group)
        {
            if (ModelState.IsValid)
            {
                db.Entry(group).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return "success.";
            }
            return "model not valid.";
        }








        /*============================================
         * add grouping to group table in db
        =============================================*/
        [ValidateAntiForgeryToken]
        [HttpPost]
        public string AddFeatures([Bind(Include = "FeaturesName")] FeaturesViewModel ViewModel)
        {
            if (ModelState.IsValid)
            {
                if (!db.Features.Any(f => f.FeaturesName == ViewModel.FeaturesName))
                {
                    db.Features.Add(new Features()
                    {
                        FeaturesName = ViewModel.FeaturesName,
                    });
                    db.SaveChanges();
                    return "success.";
                }
                else
                {
                    return "already exist.";
                }
            }
            return "model not valid.";
        }
        /*============================================
         * return all Features in db
         =============================================*/
        public ActionResult DisplayFeatures()
        {
            return PartialView(db.Features.ToList());
        }
        /*============================================
         * return summery feature wityh feature id
         =============================================*/
        public string GetSummeryFeatures(int? id)
        {
            if (id == null)
            {
                return null;
            }
            var list = db.FeaturesSummery.Where(sf => sf.FeaturesID == id).ToList();
            string summery = string.Empty;
            foreach (var item in list)
            {
                summery += item.FS_Name + ",";
            }
            return summery;
        }
        /*============================================
         * edit feature summery
         =============================================*/
        [ValidateAntiForgeryToken]
        [HttpPost]
        public string EditFeatures([Bind(Include = "FeaturesName,Summery,FeaturesID")] FeaturesViewModel ViewModel)
        {
            if (ModelState.IsValid)
            {
                Features features = db.Features.Find(ViewModel.FeaturesID);
                if (features != null)
                {
                    db.Entry(features).State = System.Data.Entity.EntityState.Modified;
                    var list = GetSummery(ViewModel.Summery);
                    if (list != null)
                    {
                        ResetFeaturesSummery(ViewModel.FeaturesID);
                        foreach (var item in list)
                        {
                            db.FeaturesSummery.Add(new FeaturesSummery()
                            {
                                FS_Name = item,
                                FeaturesID = ViewModel.FeaturesID
                            });
                        }
                    }
                    else
                    {
                        return "summery has bad format.";
                    }
                    db.SaveChanges();
                    return "success.";
                }
                return "feature not found.";
            }
            return "model not valid.";
        }
        private void ResetFeaturesSummery(int FeaturesID)
        {
            var list = db.FeaturesSummery.Where(fs => fs.FeaturesID == FeaturesID).ToList();
            db.FeaturesSummery.RemoveRange(list);
            db.SaveChanges();
        }
        private List<string> GetSummery(string summery)
        {
            List<string> summeryList = new List<string>();
            try
            {
                if (!summery.Contains(','))
                {
                    summeryList.Add(summery);
                }
                else
                {
                    var length = summery.Split(',').Length;
                    for (int i = 0; i < length; i++)
                    {
                        if (summery.Split(',')[i] != "")
                        {
                            summeryList.Add(summery.Split(',')[i]);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                summeryList = null;
            }
            return summeryList;
        }
        /*============================================
         * delete feature by feature id
         =============================================*/
        [ValidateAntiForgeryToken]
        [HttpPost]
        public string DeleteFeature(int? id)
        {
            if (id == null)
            {
                return null;
            }
            Features features = db.Features.Find(id);
            if (features == null)
            {
                return "features not valid.";
            }
            //without Casecade
            //var list = db.FeaturesSummery.Where(fs => fs.FeaturesID == features.FeaturesID).ToList();
            //db.FeaturesSummery.RemoveRange(list);
            //db.Product_Features.RemoveRange(db.Product_Features.Where(pf => pf.FeaturesID == features.FeaturesID));
            db.Features.Remove(features);
            db.SaveChanges();
            return "success.";
        }







        //disposing area
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }

}