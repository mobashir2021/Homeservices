using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OnDemandService.Models;
using WhatsAppApi;

namespace OnDemandService.Areas.Professional.Controllers
{
    public class UserController : Controller
    {
        HomeServicesEntities db = new HomeServicesEntities();
        // GET: Professional/User
        [HttpGet]
        public ActionResult Registration()
        {
            ViewBag.Pincodes = new SelectList(db.Pincodes, "PincodeId", "Pincode1");
            
            return View();
        }

        [HttpPost]
        public JsonResult GetPincodes(string Prefix)
        {
            if (PincodeStaticModel.lstStaticPincodes == null || PincodeStaticModel.lstStaticPincodes.Count == 0)
            {
                PincodeStaticModel.lstStaticPincodes = db.Pincodes.ToList();
                PincodeStaticModel.lstStaticCategories = db.Categories.ToList();
                PincodeStaticModel.lstStaticSubcategories = db.SubCategories.ToList();
            }
            //Note : you can bind same list from database  
            List<Pincode> ObjList = PincodeStaticModel.lstStaticPincodes.ToList();
            //Searching records from list using LINQ query  
            var CityList = (from N in ObjList
                            where N.Pincode1.StartsWith(Prefix)
                            select new { N.Pincode1 });
            return Json(CityList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GotoHomePage()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home", new { Area = "" });
        }

        [HttpGet]
        public ActionResult RegistrationProfessional()
        {
            ViewBag.Pincodes = new SelectList(db.Pincodes, "PincodeId", "Pincode1");
            ViewBag.Service = new SelectList(db.Categories, "CategoryId", "CategoryName");
            UserViewModel user = new UserViewModel();
            user.ServiceModel = PopulateServices();
            return View(user);
        }

        private List<SelectListItem> PopulateServices()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            var services = db.Categories.ToList().Select(x => new SelectListItem()
            {
                Text = x.CategoryName, Value = x.CategoryId.ToString()
            }).ToList();

            return services;
        }


        [HttpPost]
        public ActionResult RegistrationProfessional(UserViewModel userViewModel, string Pincodes, object Services)
        {
            string Message = "";
            bool isValid = true;
            bool isRegistrationSuccess = false;
            User user1 = new User();
            if (userViewModel != null)
            {
                User user = db.Users.Where(x => x.Email == userViewModel.Email).FirstOrDefault();
                int cnt = db.PartnerProfessionals.ToList().Where(x => x.MobileNo == userViewModel.MobileNo).Count();
                if(cnt == 0)
                {
                    cnt = db.Customers.ToList().Where(x => x.MobileNo == userViewModel.MobileNo).Count();
                }
                if (user != null)
                {
                    isValid = false;
                    Message = "Email Already exists";
                    ViewBag.Message = Message;
                    ViewBag.Status = false;
                }
                else if (cnt > 0)
                {
                    isValid = false;
                    Message = "MobileNo Already exists";
                    ViewBag.Message = Message;
                    ViewBag.Status = false;
                }
                else if (Services == null)
                {
                    isValid = false;
                    Message = "Kindly choose any Services";
                    ViewBag.Message = Message;
                    ViewBag.Status = false;
                }
                else
                {
                    string[] servicesdata = (string[])Services;
                    user1 = new User();
                    user1.Email = userViewModel.Email;
                    user1.Password = userViewModel.Password;
                    user1.UserName = userViewModel.Email;
                    db.Users.Add(user1);
                    db.SaveChanges();

                    Role role = new Role();
                    role.Roles = "Professional";
                    role.User = user1;
                    db.Roles.Add(role);
                    db.SaveChanges();

                    //Customer cs = new Customer();
                    //cs.Address = userViewModel.Address;
                    //cs.CustomerName = userViewModel.Name;
                    //cs.MobileNo = userViewModel.MobileNo;

                    Pincode pincode = db.Pincodes.ToList().Where(x => x.Pincode1 == Pincodes).FirstOrDefault();
                    //cs.Pincode = pincode;
                    //cs.UserId = user1.UserId;

                    PartnerProfessional partner = new PartnerProfessional();
                    partner.Address = userViewModel.Address;
                    partner.PartnerName = userViewModel.Name;
                    partner.MobileNo = userViewModel.MobileNo;
                    partner.UserId = user1.UserId;
                    partner.Pincode = pincode;
                    //var subcat = db.SubCategories.ToList().Where(x => x.SubCategoryId == Convert.ToInt32(Service)).First();
                    SubCategory subcat = new SubCategory();
                    partner.SubCategory = db.SubCategories.ToList().Where(x => x.SubCategoryId == Convert.ToInt32(servicesdata[0])).First();
                    var subcatdata = db.SubCategories.ToList();
                    var catdata = db.Categories.ToList();

                    db.PartnerProfessionals.Add(partner);
                    db.SaveChanges();

                    if (servicesdata != null)
                    {
                        foreach (var data in servicesdata)
                        {
                            ServiceProvided service = new ServiceProvided();
                            service.PartnerProfessional = partner;
                            
                            service.CategoryId = catdata.Where(x => x.CategoryId == Convert.ToInt32(data)).First().CategoryId;
                            db.ServiceProvideds.Add(service);
                            db.SaveChanges();
                        }
                    }
                    ViewBag.Message = "Registration Success";
                    ViewBag.Status = true;
                    isRegistrationSuccess = true;
                }
            }
            if (isRegistrationSuccess)
            {
                if (user1 != null)
                {
                    FormsAuthentication.SetAuthCookie(user1.UserId.ToString(), true);
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Pincodes = new SelectList(db.Pincodes, "PincodeId", "Pincode1");

                userViewModel.ServiceModel = PopulateServices();
                return View(userViewModel);
            }
        }

        [HttpPost]
        public ActionResult Registration(UserViewModel userViewModel, string Pincodes)
        {
            string Message = "";
            bool isValid = true;
            bool isRegistrationSuccess = false;
            if(userViewModel != null)
            {
                User user = db.Users.Where(x => x.Email == userViewModel.Email).FirstOrDefault();
                if(user != null)
                {
                    isValid = false;
                    Message = "Email Already exists";
                    ViewBag.Message = Message;
                    ViewBag.Status = false;
                }
                else
                {
                    User user1 = new User();
                    user1.Email = userViewModel.Email;
                    user1.Password = userViewModel.Password;
                    user1.UserName = userViewModel.Email;
                    db.Users.Add(user1);
                    db.SaveChanges();

                    Role role = new Role();
                    role.Roles = "Professional";
                    role.User = user1;
                    db.Roles.Add(role);
                    db.SaveChanges();

                    //Customer cs = new Customer();
                    //cs.Address = userViewModel.Address;
                    //cs.CustomerName = userViewModel.Name;
                    //cs.MobileNo = userViewModel.MobileNo;

                    Pincode pincode = db.Pincodes.ToList().Where(x => x.Pincode1 == Pincodes).FirstOrDefault();
                    //cs.Pincode = pincode;
                    //cs.UserId = user1.UserId;

                    PartnerProfessional partner = new PartnerProfessional();
                    partner.Address = userViewModel.Address;
                    partner.PartnerName = userViewModel.Name;
                    partner.MobileNo = userViewModel.MobileNo;
                    partner.UserId = user1.UserId;
                    partner.Pincode = pincode;

                    db.PartnerProfessionals.Add(partner);
                    db.SaveChanges();
                    ViewBag.Message = "Registration Success";
                    ViewBag.Status = true;
                    isRegistrationSuccess = true;
                }
            }
            
             
            ViewBag.Pincodes = new SelectList(db.Pincodes, "PincodeId", "Pincode1");
            return View(userViewModel);
        }

        public ActionResult LoginProfessional()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginProfessional(string username, string password)
        {
            User user = db.Users.ToList().Where(x => x.UserName.ToLower().Replace(" ", "") == username.ToLower().Replace(" ", "") && x.Password == password).FirstOrDefault();
            string roles = "";
            int profid = 0;
            if (user != null)
            {
                var prof = db.PartnerProfessionals.Where(x => x.UserId == user.UserId).FirstOrDefault();
                if (prof != null)
                {
                    profid = prof.PartnerProfessionalId;
                }
            }
            if (user != null && profid > 0)
            {
                Role role = db.Roles.ToList().Where(x => x.UserId == user.UserId).FirstOrDefault();
                if (role != null)
                    roles = role.Roles;
                if (roles == "Professional")
                {
                    FormsAuthentication.SetAuthCookie(user.UserId.ToString(), true);
                    return RedirectToAction("Index", "Home");
                }
                else if(roles == "Admin")
                {
                    FormsAuthentication.SetAuthCookie(user.UserId.ToString(), true);
                    return RedirectToAction("Index", "Home", new { Area = "Admin" });
                }
            }

            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("LoginProfessional");
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            User user = db.Users.ToList().Where(x => x.UserName.ToLower() == username.ToLower() && x.Password == password).FirstOrDefault();
            int profid = 0;
            if(user != null)
            {
                var prof = db.PartnerProfessionals.Where(x => x.UserId == user.UserId).FirstOrDefault();
                if(prof != null)
                {
                    profid = prof.PartnerProfessionalId;
                }
            }
            string roles = "";
            if(user != null && profid > 0)
            {
                Role role = db.Roles.ToList().Where(x => x.UserId == user.UserId).FirstOrDefault();
                if (role != null)
                    roles = role.Roles;
                if(roles == "Professional")
                {
                    FormsAuthentication.SetAuthCookie(user.UserId.ToString(), false);
                    return RedirectToAction("Index", "Home");
                }
            }

            return View();
        }

        [HttpGet]
        public JsonResult MatchMobileNo(string mobileno)
        {
            int data = db.PartnerProfessionals.Where(x => x.MobileNo == mobileno).Count();
            var json = JsonConvert.SerializeObject(data);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string mobileno)
        {
           
            Session["Mobilenumber"] = mobileno;
            TempData["Mobileno"] = mobileno;
            SendOTP(mobileno);
            return RedirectToAction("EnterOTP");
        }

        public ActionResult EnterOTP()
        {
            ViewBag.Mobileno = Session["Mobilenumber"];
            ViewBag.Otpnumber = Session["OTPVALUE"];
            return View();
        }

        public void SendOTP(string mobileno)
        {
            CompanyInfo company = db.CompanyInfoes.First();
            int optvalue = new Random().Next(100000, 999999);
            Session["OTPVALUE"] = optvalue;
            var prefrence = db.PreferOTPs.ToList();
            string message = "Your OTP number is " + optvalue + ". Sent by " + company.Companyname;
            String encodedmessaged = HttpUtility.UrlEncode(message);
            if(prefrence.Count > 0)
            {
                PreferOTP preferOTP = db.PreferOTPs.First();
                if(preferOTP.Watsapp.HasValue && preferOTP.Watsapp.Value == true)
                {
                    WatsappCredential watsapp = db.WatsappCredentials.First();
                    WhatsApp app = new WhatsApp(watsapp.WatsappUsername, watsapp.WatsappPassword, company.Companyname, false, false);
                    app.OnConnectSuccess += () =>
                    {
                        app.OnLoginSuccess += (phonenumber, data) =>
                        {
                            app.SendMessage(mobileno, message);
                        };
                        app.OnLoginFailed += (data) =>
                        {

                        };
                        app.Login();
                    };

                    app.OnConnectFailed += (ex) =>
                    {

                    };
                    app.Connect();
                }
                else   //SMS
                {
                    SMSApi api = db.SMSApis.First();
                    using(var webclient = new WebClient())
                    {
                        byte[] response = webclient.UploadValues(api.SMSUrl, new System.Collections.Specialized.NameValueCollection()
                        {
                            {"apikey", api.SMSApiKey },
                            {"numbers", mobileno },
                            {"message", encodedmessaged },
                            {"sender", api.Sender }
                        });
                        string result = System.Text.Encoding.UTF8.GetString(response);
                        var jsonobject = JObject.Parse(result);
                        var status = jsonobject["status"].ToString();
                    }
                }
            }
        }

        [HttpPost]
        public ActionResult EnterOTP(string mobileno, string otpnumber)
        {
            TempData["Mobileno"] = mobileno;
            if (otpnumber == Session["OTPVALUE"].ToString())
            {
                return RedirectToAction("ChangePassword");
            }
            else
            {
                return View("LoginProfessional");
            }
        }

        public ActionResult ChangePassword()
        {
            ViewBag.Mobileno = TempData["Mobileno"];
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(string Mobileno, string password)
        {
            var profs = db.PartnerProfessionals.Where(x => x.MobileNo == Mobileno).First();
            var user = db.Users.Where(x => x.UserId == profs.UserId).First();
            user.Password = password;
            db.Entry(user).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return View("LoginProfessional");
        }
    }
}