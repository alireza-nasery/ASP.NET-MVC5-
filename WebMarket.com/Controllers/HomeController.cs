using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DataLayer;
using WebMarket.com.ZarinPal_MVC;
<<<<<<< HEAD
using DataLayer.Last;
=======
>>>>>>> 770cb427c67960f8c83f0d679eb3924da8fdc4e3

namespace WebMarket.com.Controllers
{
    public class HomeController : Controller
    {
<<<<<<< HEAD
        readonly Context _context;
        readonly Repository<NewsEntity> _repository;
        readonly NewsService _newsService;



=======
>>>>>>> 770cb427c67960f8c83f0d679eb3924da8fdc4e3
        WebMarket_Entity db;
        ProductRepository productRepository;
        Order_CustomerAddressRepositiry order_CustomerRepositiry;
        public HomeController()
        {
<<<<<<< HEAD
            _context = new Context();
            _repository = new Repository<NewsEntity>(_context);
            _newsService = new NewsService(_repository);


=======
>>>>>>> 770cb427c67960f8c83f0d679eb3924da8fdc4e3
            db = new WebMarket_Entity();
            productRepository = new ProductRepository(db);
            order_CustomerRepositiry = new Order_CustomerAddressRepositiry(db);
        }
        /*================================
         * open index page 
         =================================*/
        public ActionResult Index()
        {
            ViewBag.PageTitle = "فروشگاه وب مارکت";
            return View();
        }
        /*================================
         * disscount partial page
         =================================*/
        public ActionResult DiscountedProducts()
        {
            var data = productRepository.GetDiscountedProducts().ToList();
            return PartialView(data);
        }
        /*================================
         * new products partial page
         =================================*/
        public ActionResult NewProducts()
        {
            var data = productRepository.GetNewProducts();
            return PartialView(data);
        }
        /*================================
         * most popular product partial page
         =================================*/
        public ActionResult MostPopularProducts()
        {
            var data = productRepository.GetMostPopularProducts();
            return PartialView(data.ToList());
        }
        /*================================
         * redirect from bank for verify
         * customer in here
         =================================*/
        public ActionResult Verify(int id)
        {
            if (Request.QueryString["Status"] != "" && Request.QueryString["Status"] != null && Request.QueryString["Authority"] != "" && Request.QueryString["Authority"] != null)
            {
                if (Request.QueryString["Status"].ToString().Equals("OK"))
                {
                    int total = (int)order_CustomerRepositiry.GetFinalPriceOrderCustomer(id);
                    long RefID;
                    System.Net.ServicePointManager.Expect100Continue = false;
                    ZarinPal_MVC.PaymentGatewayImplementationServicePortTypeClient zp = new ZarinPal_MVC.PaymentGatewayImplementationServicePortTypeClient();
                    int Status = zp.PaymentVerification("YOUR-ZARINPAL-MERCHANT-CODE", Request.QueryString["Authority"].ToString(), total, out RefID);

                    if (Status == 100)
                    {
                        if (order_CustomerRepositiry.SetIsFinallyTrue(id))
                        {
                            //successfully done
                        }
                        else
                        {
<<<<<<< HEAD
                            using (WebMarket_Entity db = new WebMarket_Entity())
=======
                            using (WebMarket_Entity db=new WebMarket_Entity())
>>>>>>> 770cb427c67960f8c83f0d679eb3924da8fdc4e3
                            {
                                var order_customerAddress = db.Order_CustomerAddress.Find(id);
                                order_customerAddress.Server_Error_IsFinally = true;
                                db.SaveChanges();
                            }
                        }
                    }
                    var data = "";
                    switch (Status)
                    {
                        case -1: data = "-اطلاعات ارسال شده ناقص است."; break;
                        case -2: data = "-IP و يا مرچنت كد پذيرنده صحيح نيست."; break;
                        case -3: data = "با توجه به محدوديت هاي شاپرك امكان پرداخت با رقم درخواست شده ميسر نمي باشد"; break;
                        case -4: data = "سطح تاييد پذيرنده پايين تر از سطح نقره اي است."; break;
                        case -11: data = "درخواست مورد نظر يافت نشد"; break;
                        case -12: data = "امكان ويرايش درخواست ميسر نمي باشد"; break;
                        case -21: data = "-هيچ نوع عمليات مالي براي اين تراكنش يافت نشد."; break;
                        case -22: data = "تراكنش نا موفق ميباشد."; break;
                        case -33: data = "-رقم تراكنش با رقم پرداخت شده مطابقت ندارد"; break;
                        case -34: data = "سقف تقسيم تراكنش از لحاظ تعداد يا رقم عبور نموده است"; break;
                        case -40: data = "-اجازه دسترسي به متد مربوطه وجود ندارد."; break;
                        case -41: data = "اطلاعات ارسال شده مربوط به AdditionalData غيرمعتبر ميباشد."; break;
                        case -42: data = "مدت زمان معتبر طول عمر شناسه پرداخت بايد بين 30 دقيه تا 45 روز مي باشد."; break;
                        case -54: data = "درخواست مورد نظر آرشيو شده است"; break;
                        case 100: data = "عمليات با موفقيت انجام گرديده است.شماره ارجاع:" + RefID; break;
                        case 101: data = "عمليات پرداخت موفق بوده و قبلا PaymentVerification تراكنش انجام شده است."; break;
                        default: data = "_default_"; break;
                    }
                    ViewBag.Description = data;
                    ViewBag.ModalCode = 201;
                    return View("Index");
                }
                else
                {
                    ViewBag.Description = "Error! Authority: " + Request.QueryString["Authority"].ToString() + " Status: " + Request.QueryString["Status"].ToString();
                }
            }
            ViewBag.Description = "پرداخت موفقیت امیز نبود";
            ViewBag.ModalCode = 201;
            return View("Index");
        }

        /*==========================================
         * about web site : description 
         * about language programing and other thing of 
         * webapplication (web market) and website faetures
         ============================================*/
        public ActionResult AboutWebSite()
        {
            ViewBag.PageTitle = "درباره سایت وب مارکت";
            return View();
        }







<<<<<<< HEAD
        public ActionResult News()
        {
            var news = _newsService.GetAllNews().Select(n=>new NewsViewModel()
            {
                Description=n.Description,
                PublishDate=n.PublishDate,
                Title=n.Title
            });

            return PartialView(news);
        }




=======
>>>>>>> 770cb427c67960f8c83f0d679eb3924da8fdc4e3
        //dispose area
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                productRepository.Dispose();
                order_CustomerRepositiry.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
