using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace WebMarket.com
{
    public static class Wrench
    {
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

        public static void SetCustomeProperty(Object obj,Object Value)
        {
            PropertyInfo[] properties = obj.GetType().GetProperties();

            foreach (var propertyInfo in properties)
            {
                if (propertyInfo.PropertyType == typeof(string))
                {
                    propertyInfo.SetValue(obj, Value);
                }
            }
        }

        private static string defaultImage = "http://cliquecities.com/assets/no-image-e3699ae23f866f6cbdf8ba2443ee5c4e.jpg";//set default image for product
        /// <summary>
        /// convert product to image (if any image exist in product gallery) use this function for get src for img HTML tag
        /// </summary>
        /// <param name="product">item or model</param>
        /// <returns></returns>
        public static string FoundSRC(DataLayer.Product product)
        {
            if (product.ProductGallery.Count != 0)
            {
                return "data:image/*;base64," + Convert.ToBase64String(product.ProductGallery.First().Image);
            }
            return defaultImage;
        }
        public static string FoundSRC(List<DataLayer.ProductGallery> productGalleryList)
        {
            if (productGalleryList.Count != 0)
            {
                return "data:image/*;base64," + Convert.ToBase64String(productGalleryList.First().Image);
            }
            return defaultImage;
        }

        public static string FoundSRC(DataLayer.Product product, int i)
        {
            try
            {
                if (product.ProductGallery.Count != 0)
                {
                    return "data:image/*;base64," + Convert.ToBase64String(product.ProductGallery.ToList()[i].Image);
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                //i is out of range (product has no or no more image)
            }
            return defaultImage;
        }
        //utilities function for SubmitShopBascket
        public static string GetStringFromList(List<string> list)
        {
            string data = string.Empty;
            foreach (var item in list)
            {
                data += item + ",";
            }
            return data;
        }
        /// <summary>
        /// convert byte to string base 64 and return css property to display that
        /// </summary>
        /// <param name="Picture64">string base 64 as current image</param>
        /// <returns></returns>
        public static string GetBackgroundImage(string Picture64)
        {
            if (Picture64 == null || Picture64 == string.Empty)
            {
                return string.Empty;
            }
            else
            {
                return string.Format("background-image:url('data:image;base64,{0}')", Picture64);
            }
        }

    }
}