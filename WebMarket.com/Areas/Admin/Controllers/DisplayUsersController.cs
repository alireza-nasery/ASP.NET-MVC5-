using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;

namespace WebMarket.com.Areas.Admin.Controllers
{
    public class DisplayUsersController : Controller
    {
        WebMarket_Entity db = new WebMarket_Entity();
        UserRepository userRepository;
        RolesRepository rolesRepository;
        public DisplayUsersController()
        {
            userRepository = new UserRepository(db);
            rolesRepository = new RolesRepository(db);
        }
        /*===================================
         * display page for show all user
         * send all roles to page
         ====================================*/
        public ActionResult DisplayUsersPage()
        {
            ViewBag.PageTitle = "کاربران";
            return View(rolesRepository.GetRoles());
        }
        /*===================================
         * fill table in show all user page
         * request kind : ajax
         ====================================*/
        [HttpPost]
        public ActionResult ShowUsers(int RoleCode)
        {
            switch (RoleCode)
            {
                case 4:
                    ViewBag.GroupID = 4;
                    return PartialView(userRepository.WhereUserByRoleID(4));
                case 5:
                    ViewBag.GroupID = 5;
                    return PartialView(userRepository.WhereUserByRoleID(5));
                default: break;
            }
            return null;
        }
        /*===================================
         * function for delete user with id 
         * in showusers template there is
         * my custome way for do that
         ====================================*/
        [HttpPost]
        public int DeleteUser(int id)
        {
            if (userRepository.IsUserExistByID(id))
            {
                User user = db.User.Find(id);
                if (userRepository.Delete(user))
                {
                    return 0;
                }
                return 1;
            }
            else
            {
                return 2;
            }
        }



        /*======================================
         * open edit page i admin panel for edit
         * user registered in webmarketcom
         =======================================*/
        [HttpGet]
        public ActionResult EditUser(int id)
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
        public ActionResult EditUser(EditUserViewModel ViewModel)
        {
            if (ModelState.IsValid)
            {
                User user = userRepository.FindUserByID(ViewModel.ID);
                if (user == null)
                {
                    return View("NotFound");
                }
                else
                {
                    byte[] data = Wrench.GetByteAsHttpPostedFileBase(ViewModel.Image);
                    user.Picture = user.Picture==null ? null:(ViewModel.Image == null ? Convert.FromBase64String(ViewModel.Picture64) : data);
                    user.RoleID = ViewModel.RoleID;
                    user.Usename = ViewModel.Username;
                    user.Email = ViewModel.Email;
                    user.FirstName = ViewModel.FirstName;
                    user.LastName = ViewModel.LastName;
                    user.PhonNumber = ViewModel.PhonNumber;
                    user.IsActive = ViewModel.IsActive;
                    user.Address = ViewModel.Address;
                    user.City = ViewModel.City;
                    user.PostalCode = ViewModel.PostalCode;
                    user.AboutMe = ViewModel.AboutMe;
                    if (userRepository.Update(user))
                    {
                        ViewModel.Picture64=data == null ? "": Convert.ToBase64String(data);
                        ViewBag.Result = 100;
                        ViewBag.Icon = "";
                        ViewBag.Message = "کاربر مورد نظر با موفقیت تغییر داده شد";
                        ViewBag.Type = "success";
                        return View(ViewModel);
                    }
                    else
                    {
                        ViewBag.Result = 101;
                        ViewBag.Icon = "";
                        ViewBag.Message = "مشکلی در یروز رسانی اطلاعات موجود است";
                        ViewBag.Type = "warning";
                        return View(ViewModel);
                    }
                }
            }
            return View(ViewModel);
        }






        //disposing area
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                rolesRepository.Dispose();
                userRepository.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}