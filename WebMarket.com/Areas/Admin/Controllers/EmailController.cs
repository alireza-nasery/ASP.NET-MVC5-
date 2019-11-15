using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using System.Threading;

namespace WebMarket.com.Areas.Admin.Controllers
{
    public class EmailController : Controller
    {
        WebMarket_Entity db = new WebMarket_Entity();
        UserRepository userRepository;
        public EmailController()
        {
            userRepository = new UserRepository(db);
        }
        /*============================================
         * select users email for send advertising
         =============================================*/
        public ActionResult SelectEmailTable()
        {
            ViewBag.PageTitle = "ارسال ایمیل";
            return View(userRepository.WhereUserByRoleID(RoleID: 5).ToList());
        }
        [HttpPost]
        public ActionResult SendEmailPage(List<int> IDList)
        {
            ViewBag.PageTitle = "ارسال ایمیل";
            List<string> Emails = new List<string>();
            foreach (var item in IDList)
            {
                Emails.Add(userRepository.FindUserByID(item).Email);
            }
            return View(Emails);
        }
        /*============================================
         * when email chosen, then send description and
         * other thing to this function 
         =============================================*/
        int counter = 0;
        int number = 0;
        List<string> emails;
        [ValidateInput(false)]
        [HttpPost]
        public int SendEmailToUser(SendEmailViewModel ViewModel)
        {
            this.emails = ViewModel.Emails;

            ViewModel.Image64 = Convert.ToBase64String(Wrench.GetByteAsHttpPostedFileBase(ViewModel.image));
            string Body = PartialToStringClass.RenderPartialView("ManageEmail", "SendAdvertising", ViewModel);

            for (int i = 0; i < 5; i++)
            {
                new Thread(new ThreadStart(() => ThreadMakerForSendEmail(i, Body, ViewModel.Title))).Start();
                number++;
                Thread.Sleep(200);
            }

            while (counter < number)
            {
                //main thread loop
            }

            return 0;
        }
        void ThreadMakerForSendEmail(int id, string body, string title)
        {
            try
            {
                SendEmail.Send(emails[id], title, body);
            }
            catch (Exception e)
            {
                counter++;
                Thread.CurrentThread.Abort();
            }
            id += 5;
            ThreadMakerForSendEmail(id, body, title);
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