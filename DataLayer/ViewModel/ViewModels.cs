using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataLayer
{
    public class NewsViewModel
    {
        [Display(Name = "متن خبر")]
        [Required(ErrorMessage = "لطفا متن خبر خود را وارد کنید")]
        public string Description { get; set; }
        [Display(Name = "موضوع")]
        [Required(ErrorMessage = "لطفا موضوع را وارد کنید")]
        public string Title { get; set; }
        [Display(Name = "تاریخ انتشار")]
        public string PublishDate { get; set; }
    }
    public class RegisterViewModel
    {
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا نام کاربری خود را وارد کنید")]
        public string Username { get; set; }
        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا کلمه عبور خود را وارد کنید")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "تکرار کلمه عبور")]
        [Required(ErrorMessage = "تکرار کلمه عبور الزامیست")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = " لطفا ایمیل خود را وارد کنید")]
        [RegularExpression("^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$", ErrorMessage = "ایمیل وارد شده اشتباه است")]
        public string Email { get; set; }
    }
    public class LogInViewModel
    {
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        public string Username_Email { get; set; }
        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا کلمه عبور خود را وارد کنید")]
        [DataType(DataType.Password)]
        public string Password_log { get; set; }
        [Display(Name = "مرا به خاطر بسپار")]
        public bool RememberMe { get; set; }
    }
    public class ForgotPasswordViewModel
    {
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        public string Email_Recovery { get; set; }
    }
    public class ActivationLinkViewModel
    {
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        public string Email_Activation { get; set; }

    }
    public class RecoverPasswordViewModel
    {
        public int ID { get; set; }
        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا کلمه عبور خود را وارد کنید")]
        [DataType(DataType.Password)]
        public string Password_Recover { get; set; }
        [Display(Name = "تکرار کلمه عبور ")]
        [DataType(DataType.Password)]
        [Compare("Password_Recover", ErrorMessage = "نکرار کلمه عبور اشتباه است")]
        [Required(ErrorMessage = "تکرار کلمه عبور الزامیست")]
        public string ConfirmPassword_Recover { get; set; }
    }


    /*========================================
     * areas view model : admin area
     =========================================*/

    public class AddUserViewModel
    {
        private List<Roles> Data;
        public List<Roles> Roles
        {
            get
            {
                using (WebMarket_Entity Connection = new WebMarket_Entity())
                {
                    Data = Connection.Roles.ToList();
                }
                return Data;
            }
        }
        [Display(Name = "نقش کاربر")]
        [Required(ErrorMessage = "لطفا نقش کاربر را وارد کنید")]
        public int RoleID { get; set; }
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا نام کاربری خود را وارد کنید")]
        public string Username { get; set; }
        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا کلمه عبور خود را وارد کنید")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "تکرار کلمه عبور")]
        [Required(ErrorMessage = "تکرار کلمه عبور الزامیست")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا ایمیل خود را وارد کنید")]
        [RegularExpression("^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$", ErrorMessage = "ایمیل وارد شده اشتباه است")]
        public string Email { get; set; }
        [Display(Name = "نام")]
        public string FirstName { get; set; }
        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }
        [Display(Name = "شماره همراه")]
        public string PhonNumber { get; set; }
        public System.Web.HttpPostedFileBase Image { get; set; }
    }
    public class EditUserViewModel
    {
        [Required]
        public int ID { get; set; }
        [Display(Name = "نقش کاربر")]
        [Required(ErrorMessage = "لطفا نقش کاربر را وارد کنید")]
        public int RoleID { get; set; }
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا نام کاربری خود را وارد کنید")]
        public string Username { get; set; }
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا ایمیل خود را وارد کنید")]
        [RegularExpression("^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$", ErrorMessage = "ایمیل وارد شده اشتباه است")]
        public string Email { get; set; }
        [Display(Name = "نام")]
        public string FirstName { get; set; }
        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }
        [Display(Name = "شماره همراه")]
        public string PhonNumber { get; set; }
        public System.Web.HttpPostedFileBase Image { get; set; }
        public string Picture64 { get; set; }
        [Display(Name = "ادرس")]
        public string Address { get; set; }
        [Display(Name = "شهر")]
        public string City { get; set; }
        [Display(Name = "کدپستی")]
        public string PostalCode { get; set; }
        [Display(Name = "درباره من")]
        public string AboutMe { get; set; }
        [Display(Name = "وضعیت کاربر")]
        public bool IsActive { get; set; }
    }
    public class SendEmailViewModel
    {
        public string Image64 { get; set; }
        public System.Web.HttpPostedFileBase image { get; set; }
        [Required]
        public List<string> Emails { get; set; }
        [Required]
        public string Description { get; set; }
        public string Title { get; set; }
    }

    //_________________________________________________________________________________________________ end user models
    //---------------------------------------------remove unneccesery viewmode for product
    public class ProductViewModel
    {
        public int ProductID { get; set; }
        private List<Group> groupData;
        private List<Features> featuresData;
        private List<FeaturesSummery> featuresSummeriesData;
        public List<Group> Groups
        {
            get
            {
                if (groupData == null)
                {
                    using (WebMarket_Entity db = new WebMarket_Entity())
                    {
                        groupData = db.Group.ToList();
                    }
                }
                return groupData;
            }
        }
        public List<Features> Features
        {
            get
            {
                if (featuresData == null)
                {
                    using (WebMarket_Entity db = new WebMarket_Entity())
                    {
                        featuresData = db.Features.ToList();
                    }
                }
                return featuresData;
            }
        }
        public List<FeaturesSummery> FeaturesSummeries
        {
            get
            {
                if (featuresSummeriesData == null)
                {
                    using (WebMarket_Entity db = new WebMarket_Entity())
                    {
                        featuresSummeriesData = db.FeaturesSummery.ToList();
                    }
                }
                return featuresSummeriesData;
            }
        }
        public bool Available { get; set; }
        public List<string> Gallery64 { get; set; }
        public List<HttpPostedFileBase> Gallery { get; set; }
        [Display(Name = "دسته بندی")]
        [Required(ErrorMessage = "{0} را وارد کنید", AllowEmptyStrings = false)]
        public List<int> GroupsID { get; set; }
        [Display(Name = "نام محصول")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        public string ProductName { get; set; }
        [Display(Name = "قیمت قبلی")]
        public double? OldPrice { get; set; }
        [Display(Name = "قیمت الان")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        public double NowPrice { get; set; }
        [Display(Name = "امتیاز")]
        public int? Rate { get; set; }
        [Display(Name = "توضیح مختصر")]
        public string ShortDescription { get; set; }
        [Display(Name = "توضیح کامل")]
        [Required(ErrorMessage = "{0} را وارد کنید")]
        public string FullDescription { get; set; }
        public List<string> FeaturesSummeries_Temp { get; set; }
    }
    public class AddFeatures
    {
        [Display(Name = "ویژگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string FeaturesName { get; set; }
        public string FeaturesProperty { get; set; }
    }
    public class AddGroupViewModel
    {
        [Display(Name = "دسته بندی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string GroupName { get; set; }
    }
    public class FeaturesViewModel
    {
        public int FeaturesID { get; set; }
        [Required]
        public string FeaturesName { get; set; }
        public string Summery { get; set; }
    }
    //_________________________________________________________________________________________________ end product models
    public class ShopPageViewModel
    {
        public List<Product> Products { get; set; }
        public List<Features> Features { get; set; }
        public List<Group> Groups { get; set; }
    }
    public class AddToBasketViewModel
    {
        public List<Product_Features> Product_Features { get; set; }
        public List<Features> Features { get; set; }
        public int ProductID { get; set; }
        public List<string> FeaturesSummeries_Temp { get; set; }
        public int Number { get; set; }
    }
    public class ShopCartModel
    {
        public Product Product { get; set; }
        public List<ProductGallery> ProductGalleryList { get; set; }
        public int Number { get; set; }
        public List<string> FeaturesSummeries_Temp { get; set; }
    }

}
