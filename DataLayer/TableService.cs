using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataLayer
{
    /// <summary>
    /// for refresh tables and edit and add data in that(like product_features and etc...)
    /// </summary>
    public static class TableService
    {
        public static bool ChangeTable_PGR(List<int> GroupsID, int ProductID)
        {
            using (WebMarket_Entity db = new WebMarket_Entity())
            {
                db.Product_Groups.RemoveRange(db.Product_Groups.Where(pg => pg.ProductID == ProductID));
                foreach (var item in GroupsID)
                {
                    db.Product_Groups.Add(new Product_Groups()
                    {
                        ProductID = ProductID,
                        GroupID = item,
                    });
                }
                db.SaveChanges();
                return true;
            }
        }
        public static bool ChangeTable_PFE(List<string> FeaturesSummeries_Temp, int ProductID)
        {
            using (WebMarket_Entity db = new WebMarket_Entity())
            {
                db.Product_Features.RemoveRange(db.Product_Features.Where(pf => pf.ProductID == ProductID));
                try
                {
                    foreach (var item in FeaturesSummeries_Temp)
                    {
                        var FS_ID = Int32.Parse(item);
                        db.Product_Features.Add(new Product_Features()
                        {
                            ProductID = ProductID,
                            FeaturesID = db.FeaturesSummery.Single(fs => fs.FS_ID == FS_ID).FeaturesID,
                            Value = db.FeaturesSummery.Find(FS_ID).FS_Name
                        });
                    }
                }
                catch (NullReferenceException e)
                {
                    //FeaturesSummeries_Temp is null admin dont choice any of features
                }
                db.SaveChanges();
                return true;
            }
        }
        public static bool ChangeTable_PGA(List<HttpPostedFileBase> Gallery, int ProductID)
        {
            using (WebMarket_Entity db = new WebMarket_Entity())
            {
                for (int i = 0; i < Gallery.Count; i++)
                {
                    if (Gallery[i] != null)
                    {
                        var product = db.ProductGallery.SingleOrDefault(pg => pg.ProductID == ProductID && pg.HomeNumber == i);
                        if (product != null)
                            db.ProductGallery.Remove(product);
                        db.ProductGallery.Add(new ProductGallery()
                        {
                            Image = GetByteAsHttpPostedFileBase(Gallery[i]),
                            ImageName = Guid.NewGuid().ToString(),
                            HomeNumber = i,
                            ProductID = ProductID
                        });
                    }
                }
                db.SaveChanges();
                return true;
            }
        }
        /// <summary>
        /// please make temp when user this function, this function using (using) code and close stream !
        /// </summary>
        /// <param name="File">current http posted file base objects in (request)</param>
        /// <returns></returns>
        public static byte[] GetByteAsHttpPostedFileBase(System.Web.HttpPostedFileBase File)
        {
            try
            {
                byte[] Data = null;
                using (Stream InputStream = File.InputStream)
                {
                    MemoryStream memoryStream = InputStream as MemoryStream;
                    if (memoryStream == null)
                    {
                        memoryStream = new MemoryStream();
                        InputStream.CopyTo(memoryStream);
                    }
                    Data = memoryStream.ToArray();
                }
                return Data;
            }
            catch (NullReferenceException)//exception detail mode __ mode __
            {
                return null;
            }
        }

    }
}
