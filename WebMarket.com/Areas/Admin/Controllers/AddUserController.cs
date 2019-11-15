using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using System.Web.Security;

namespace WebMarket.com.Areas.Admin.Controllers
{
    public class AddUserController : Controller
    {
        WebMarket_Entity db=new WebMarket_Entity();
        UserRepository userRepository;
        public AddUserController()
        {
            userRepository = new UserRepository(db);
        }
        /*================================
         * return added page for add user 
         * in admin panel
         =================================*/
        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.PageTitle = "افزودن کاربر";
            return View(new AddUserViewModel());
        }
        /*================================
         * post viewmodel for add user to
         * this function
         =================================*/
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Add(AddUserViewModel ViewModel)
        {
            if (ModelState.IsValid)
            {
                if(!(userRepository.IsUserExistByUsernameOrEmail(ViewModel.Username,ViewModel.Email) && userRepository.IsUserExistByRoleID(ViewModel.RoleID)))
                {
                    User user=new User()
                    {
                        Picture = Wrench.GetByteAsHttpPostedFileBase(ViewModel.Image),
                        RoleID = ViewModel.RoleID,
                        Usename = ViewModel.Username,
                        Email = ViewModel.Email,
                        Password = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(ViewModel.Password, "MD5"),
                        FirstName = ViewModel.FirstName,
                        LastName = ViewModel.LastName,
                        PhonNumber = ViewModel.PhonNumber,
                        ActiveCode=Guid.NewGuid().ToString(),
                        IsActive=false,
                        RegisterDate=DateTime.Now
                    };
                    if (userRepository.Add(user))
                    {
                        ViewBag.Result = 100;
                        ViewBag.Message = "کاربر مورد نظر با موفقیت اضافه شد";
                        ViewBag.Type = "success";
                        ViewBag.Icon = "all_alert";
                        return View(ViewModel);
                    }
                    else
                    {
                        ViewBag.Result = 101;
                        ViewBag.Message = "مشکل در سرور روی است";
                        ViewBag.Type = "warning";  
                        ViewBag.Icon = "error_alert";
                        return View(ViewModel);
                    }
                }
                else
                {
                    ModelState.AddModelError("Username", "این کاربر در سامانه موجود میباشد");
                    return View(ViewModel);
                }
            }
            return View(ViewModel);
        }






        //disposing area
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                userRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}