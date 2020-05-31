using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    [MetadataType(typeof(GroupMetaData))]
    public partial class Group
    {

    }
    public class GroupMetaData
    {
        [Required]
        public string GroupName { get; set; }
    }
    [MetadataType(typeof(Order_CustomerAddressMetaData))]
    public partial class Order_CustomerAddress
    {

    }
    public class Order_CustomerAddressMetaData
    {
        [Display(Name="شماره تلفن")]
        [Required(ErrorMessage = "{0} را وارد کانید")]
        public string PhoneNumber { get; set; }
        [Display(Name="ایمیل")]
        public string Email { get; set; }
        [Display(Name="ادرس")]
        [Required(ErrorMessage = "{0} را وارد کانید")]
        public string Address { get; set; }
        [Display(Name="استان")]
        [Required(ErrorMessage = "{0} را وارد کانید")]
        public string City { get; set; }
        [Display(Name="کد پستی")]
        [Required(ErrorMessage = "{0} را وارد کانید")]
        public string PostalCode { get; set; }
        [Display(Name="توضیحات")]
        public string Description { get; set; }
        [Display(Name = "نام")]
        [Required(ErrorMessage = "{0} را وارد کانید")]
        public string FirstName { get; set; }
        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "{0} را وارد کانید")]
        public string LastName { get; set; }

    }
}
