using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using System.Web.Security;

namespace WebMarket.com.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        WebMarket_Entity db = new WebMarket_Entity();
        UserRepository userRepository;
        public HomeController()
        {
            userRepository = new UserRepository(db);
        }
        /*========================================
         * open admin profile page
         =========================================*/
        public ActionResult Index()
        {
            ViewBag.PageTitle = "پروفایل";
            string name = User.Identity.Name;
            return View(userRepository.FindUserByEmailOrUsername(name));
        }
        /*======================================
         * open edit page i admin panel for edit
         * user registered in webmarketcom
         =======================================*/
        [HttpGet]
        public ActionResult ProfileComplete(int id)
        {
            User user = userRepository.FindUserByID(id);
            EditUserViewModel ViewModel = new EditUserViewModel()
            {
                ID = user.ID,
                Picture64 = user.Picture == null ? null : Convert.ToBase64String(user.Picture),
                RoleID = user.RoleID,
                Username = user.Usename,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhonNumber = user.PhonNumber,
                IsActive = user.IsActive,
                Address = user.Address,
                City = user.City,
                PostalCode = user.PostalCode,
                AboutMe = user.AboutMe
            };
            return View(ViewModel);
        }
        /*======================================
         * view model for edite users post to this
         * function and this func edit user by 
         * view model
         =======================================*/
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ProfileComplete(EditUserViewModel ViewModel)
        {
            if (ModelState.IsValid)
            {
                User user = userRepository.FindUserByID(ViewModel.ID);
                if (user == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    //make temp for GetByteAsHttpPostedFileBase
                    byte[] temp = Wrench.GetByteAsHttpPostedFileBase(ViewModel.Image);

                    user.Picture = ViewModel.Image == null ? Convert.FromBase64String(ViewModel.Picture64) : temp;
                    user.Usename = ViewModel.Username;
                    user.Email = ViewModel.Email;
                    user.FirstName = ViewModel.FirstName;
                    user.LastName = ViewModel.LastName;
                    user.PhonNumber = ViewModel.PhonNumber;
                    user.Address = ViewModel.Address;
                    user.City = ViewModel.City;
                    user.PostalCode = ViewModel.PostalCode;
                    user.AboutMe = ViewModel.AboutMe;
                    if (userRepository.Update(user))
                    {
                        //update view model
                        ViewModel.Picture64 = Convert.ToBase64String(temp);

                        ViewBag.Result = 100;
                        ViewBag.Icon = "";
                        ViewBag.Message = "اطلاعات مورد نظر  شما موفقیت تغییر داده شد";
                        ViewBag.Type = "success";
                        return View(ViewModel);
                    }
                    else
                    {
                        ViewBag.Result = 101;
                        ViewBag.Icon = "";
                        ViewBag.Message = "مشکلی در بروز رسانی اطلاعات موجود است";
                        ViewBag.Type = "warning";
                        return View(ViewModel);
                    }
                }
            }
            return View(ViewModel);
        }







        //dispose area
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