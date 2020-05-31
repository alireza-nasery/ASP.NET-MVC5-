using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using DataLayer.Last;

namespace WebMarket.com.Areas.Admin.Controllers
{
    public class NewsController : Controller
    {
        readonly Context _context;
        readonly Repository<NewsEntity> _repository;
        readonly NewsService _newsService;
        public NewsController()
        {
            _context = new Context();
            _repository = new Repository<NewsEntity>(_context);
            _newsService = new NewsService(_repository);
        }
        public ActionResult AboutNews()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AboutNews(NewsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.PublishDate = PersianDateExtensionMethods.ToPeString(DateTime.Now);
                if (_newsService.AddNews(viewModel))
                {
                    ViewBag.Result = 100;
                    ViewBag.Message = "محصول مورد نظر با موفقیت اضافه شد";
                    ViewBag.Type = "success";
                    ViewBag.Icon = "add_alert";
                    return View();
                }
                else
                {
                    ViewBag.Result = 100;
                    ViewBag.Message = "مشکلی در سرور وجود دارد";
                    ViewBag.Type = "danger";
                    ViewBag.Icon = "add_alert";
                    return View(viewModel);
                }
            }
            return View(viewModel);
        }
    }
}