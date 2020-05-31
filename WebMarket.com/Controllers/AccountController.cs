using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace WebMarket.com.Controllers
{
    public class AccountController : Controller
    {
        WebMarket_Entity db = new WebMarket_Entity();
        UserRepository userRepository;
        public AccountController()
        {
            userRepository = new UserRepository(db);
        }
        /*====================================
         when user register on site and finish form
         data data send here and use register in
         db with false isactive
         =====================================*/
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Register(RegisterViewModel ViewModel)
        {
            if (ModelState.IsValid)
            {
                if (!userRepository.IsUserExistByEmail(ViewModel.Email))
                {
                    User user = new User()
                    {
                        Usename = ViewModel.Username,
                        Password = FormsAuthentication.HashPasswordForStoringInConfigFile(ViewModel.Password, "MD5"),
                        Email = ViewModel.Email,
                        IsActive = false,
                        ActiveCode = Guid.NewGuid().ToString(),
                        RoleID = 5,
                        RegisterDate = DateTime.Now
                    };
                    userRepository.Add(user);

                    string Body = PartialToStringClass.RenderPartialView("ManageEmail", "ActivateEmail", user);
                    SendEmail.Send(user.Email, "Subject", Body);

                    ViewBag.Description = "پیامی خاوی لینکی برای فعال سازی به ایمیل شما فرستاده شد لطفا به ایمیل حود رفته و بر روی لینک مورد نطر کلیل منید";
                    ViewBag.ModalCode = 201;
                    return View("~/Views/Home/Index.cshtml");
                }
                else
                {
                    ModelState.AddModelError("Email", "ایمیل وارد شده تکراریست");
                    ViewBag.ModalCode = 101;
                }

            }
            else
            {
                ViewBag.ModalCode = 101;
            }
            return View("~/Views/Home/Index.cshtml");
        }



        /*====================================
         active user with activate code in his
         email
         =====================================*/
        public ActionResult ActiveUser(string ActiveCode)
        {
            User user = userRepository.FindUserByActiveCode(ActiveCode);
            if (user == null)
            {
                return View("NotFound");
            }
            user.IsActive = true;
            user.ActiveCode = Guid.NewGuid().ToString();
            userRepository.Update(user);
            ViewBag.Description = "ثبت نام شما با موفقیت انجام شد شما میتوانید از قسمت ورود وارد اکانت خود شوید";
            ViewBag.ModalCode = 201;
            return View("~/Views/Home/Index.cshtml");
        }




        /*====================================
         * login :when user put the right data
         * in login modal data post to this function
         =====================================*/
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult LogIn(LogInViewModel ViewModel)
        {
            if (ModelState.IsValid)
            {
                User user = userRepository.FindUserByUsernameAndEmail(ViewModel.Username_Email, ViewModel.Username_Email);
                if (user == null)
                {
                    ModelState.AddModelError("Username_Email", "فردی با این نام وجود ندارد");
                    ViewBag.ModalCode = 102;
                    return View("~/Views/Home/Index.cshtml");
                }
                else
                {
                    if (user.Password == FormsAuthentication.HashPasswordForStoringInConfigFile(ViewModel.Password_log, "MD5"))
                    {
                        if (user.IsActive)
                        {
                            FormsAuthentication.SetAuthCookie(user.Usename, ViewModel.RememberMe);
                            return RedirectToAction("Index", "Home");
                        }
                        ViewBag.ModalCode = 201;
                        ViewBag.Description = "کاربر گرامی ثبت نام شما کامل نشده لطفا با مراصه به ایمیل خود کاملش کنید";
                        return View("~/Views/Home/Index.cshtml");
                    }
                    else
                    {
                        ViewBag.ModalCode = 102;
                        ModelState.AddModelError("Username_Email", "کاربری با اطلاعات وارد شده یافت نشد");
                    }
                }

            }
            else
            {
                ViewBag.ModalCode = 102;
            }
            return View("~/Views/Home/Index.cshtml");
        }




        /*===========================================
         post data in forgot password when user submit
         data in forgot password modal and data will
         be send here
         ============================================*/
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordViewModel ViewModel)
        {
            if (ModelState.IsValid)
            {
                if (!userRepository.IsUserExistByEmail(ViewModel.Email_Recovery))
                {
                    ModelState.AddModelError("Email_Recovery", "ایمیل وارد شده اشتباه است");
                    ViewBag.ModalCode = 103;
                    return View("~/Views/Home/Index.cshtml");
                }
                //user found
                User user = userRepository.FindUserByEmail(ViewModel.Email_Recovery);
                if (user.IsActive == false)
                {
                    ViewBag.Description = "ایمیل وارو شده فعال نشده ابتدا اقدام به فعال کردن ان نمایید !";
                    ViewBag.ModalCode = 201;
                    return View("~/Views/Home/Index.cshtml");
                }
                string Body = PartialToStringClass.RenderPartialView("ManageEmail", "RecoverPassword", user);
                SendEmail.Send(user.Email, "Subject", Body);

                ViewBag.Description = "پیامی حاوی لینک فعال سازی ببه ایمیل شما فرستاده شد برای ویرایش رمز عبور  لطفا به ایمیل خود رفته وبر روی ان کلیک کنید";
                ViewBag.ModalCode = 201;
                return View("~/Views/Home/Index.cshtml");

            }
            else
            {
                ViewBag.ModalCode = 103;
            }
            return View("~/Views/Home/Index.cshtml");
        }




        /*============================================
         * when user click on recover pass in own emaail
         * user send with active code here
         =============================================*/
        public ActionResult RecoverPassword(string ActiveCode)
        {
            User user = userRepository.FindUserByActiveCode(ActiveCode);
            if (user == null)
            {
                return View("NotFound");
            }
            user.ActiveCode = Guid.NewGuid().ToString();
            userRepository.Update(user);
            ViewBag.ActionCode = 101;
            ViewBag.UserID = user.ID;
            return View("~/Views/Home/Index.cshtml");

        }
        /*==========================================
         * post data as recover modal
         ===========================================*/
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult RecoverPassword(RecoverPasswordViewModel ViewModel)
        {
            if (ModelState.IsValid)
            {
                User user = userRepository.FindUserByID(ViewModel.ID);
                user.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(ViewModel.Password_Recover, "MD5");
                userRepository.Update(user);
                ViewBag.Description = $"عریز پسورد شما با موفقیت تغییر یافت میتوانید از قسمت ورود وارد اکنت خود شوید{user.Usename}";
                ViewBag.ModalCode = 201;
                return View("~/Views/Home/Index.cshtml");
            }
            else
            {
                ViewBag.ActionCode = 101;
                ViewBag.UserId = ViewModel.ID;
            }
            return View("~/Views/Home/Index.cshtml");
        }




        /*==============================================
         if user dont receve any mail that contain activation
         link this function send that again , with 
         send activation modal form
         ===============================================*/
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult SendActivationLink(ActivationLinkViewModel ViewModel)
        {
            if (ModelState.IsValid)
            {
                if (!userRepository.IsUserExistByEmail(ViewModel.Email_Activation))
                {
                    ModelState.AddModelError("Email_Activation", "ایمیل وارد شده اشتباه است");
                    ViewBag.ModalCode = 104;
                    return View("~/Views/Home/Index.cshtml");
                }
                //user found
                User user = userRepository.FindUserByEmail(ViewModel.Email_Activation);
                if (user.IsActive == true)
                {
                    ViewBag.Description = "ایمیل وارد شده فعال است!";
                    ViewBag.ModalCode = 201;
                    return View("~/Views/Home/Index.cshtml");
                }
                string Body = PartialToStringClass.RenderPartialView("ManageEmail", "ActivateEmail", user);
                SendEmail.Send(user.Email, "Subject", Body);

                ViewBag.Description = "پیامی حاوی لینکی برای فعال سازی به ایمیل شما فرستاده شد لطفا به ایمیل خود رفته و بر روی لینک مورد نطر کلیل کنید";
                ViewBag.ModalCode = 201;
                return View("~/Views/Home/Index.cshtml");

            }
            else
            {
                ViewBag.ModalCode = 104;
            }
            return View("~/Views/Home/Index.cshtml");
        }




        /*==========================================
         * sign out of account
         ===========================================*/
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
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