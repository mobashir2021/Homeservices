using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using OnDemandService.Models;
using FiftyOne.Foundation;
using System.Text.RegularExpressions;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.Net.Mime;
using Newtonsoft.Json.Linq;
using System.Net;
using WhatsAppApi;
using Newtonsoft.Json;

namespace OnDemandService.Controllers
{
    public class HomeController : Controller
    {
        HomeServicesEntities db = new HomeServicesEntities();
        // GET: Home
        public ActionResult Index(bool isActive = false)
        {
            return View();
        }

        public ActionResult LoginMobileUser(UserViewModel userViewModel)
        {
            if (userViewModel == null)
                userViewModel = new UserViewModel();
            return View(userViewModel);
        }

        [HttpPost]
        public ActionResult LoginMobileUser(UserViewModel userViewModel, string username, string password, string SubCategoryId, string Pincodes, string loginvalue, string registervalue)
        {
            if (registervalue != null && registervalue == "Register")
            {
                UserViewModel userView = new UserViewModel();
                userView.SubCategoryId = SubCategoryId;
                userView.PincodeId = Pincodes;
                ViewBag.Pincodes = new SelectList(db.Pincodes, "PincodeId", "Pincode1");
                return View("RegisterMobileUser", userView);
            }
            var userlist = db.Users.ToList();
            User user = db.Users.ToList().Where(x => x.UserName.ToLower().Replace(" ", "") == username.ToLower().Replace(" ", "") && x.Password == password).FirstOrDefault();

            string roles = "";
            if (user != null)
            {
                Role role = db.Roles.ToList().Where(x => x.UserId == user.UserId).FirstOrDefault();
                if (role != null)
                    roles = role.Roles;
                if (roles == "Customer")
                {
                    FormsAuthentication.SetAuthCookie(user.UserId.ToString(), false);

                    if (!string.IsNullOrEmpty(SubCategoryId))
                    {
                        var customer = db.Customers.ToList().Where(x => x.UserId == user.UserId).First();
                        Order order = new Order();
                        order.SubCategoryId = Convert.ToInt32(SubCategoryId);
                        order.Customer = customer;
                        order.OrderPlacedOn = DateTime.Now;
                        order.OrderDate = DateTime.Now;
                        db.Orders.Add(order);
                        db.SaveChanges();
                        ViewBag.Message = "Order Submitted Successfully! Our professional will notify with date and time availibility.";
                        ViewBag.Status = true;
                        ViewBag.UserLoggedin = true;
                        return View("OrderPlaced");
                    }
                    else
                    {

                        ViewBag.UserLoggedin = true;
                        return RedirectToAction("Index", "Home", new { isActive = true });
                    }
                }

            }

            return View();
        }

        

        public ActionResult AboutUs()
        {
            string roles = "";
            if (!string.IsNullOrEmpty(User.Identity.Name))
            {
                Role role = db.Roles.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).FirstOrDefault();
                if (role != null)
                    roles = role.Roles;
                if (roles == "Customer")
                {
                    ViewBag.UserLoggedin = true;
                }
            }
            return View();
        }

        public ActionResult ContactUs()
        {
            string roles = "";
            if (!string.IsNullOrEmpty(User.Identity.Name))
            {
                Role role = db.Roles.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).FirstOrDefault();
                if (role != null)
                    roles = role.Roles;
                if (roles == "Customer")
                {
                    ViewBag.UserLoggedin = true;
                }
            }
            return View();
        }

        public ActionResult Services()0 
        {
            string roles = "";
            if (!string.IsNullOrEmpty(User.Identity.Name))
            {
                Role role = db.Roles.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).FirstOrDefault();
                if (role != null)
                    roles = role.Roles;
                if (roles == "Customer")
                {
                    ViewBag.UserLoggedin = true;
                }
            }
            return View();
        }

        public ActionResult TrackServiceStatus()
        {
            string roles = "";
            if (!string.IsNullOrEmpty(User.Identity.Name))
            {
                Role role = db.Roles.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).FirstOrDefault();
                if (role != null)
                    roles = role.Roles;
                if (roles == "Customer")
                {
                    ViewBag.UserLoggedin = true;
                }
            }
            ViewBag.Pincodes = new SelectList(db.Pincodes, "PincodeId", "Pincode1");
            return View();
        }

        [HttpPost]
        public ActionResult TrackStatus()
        {
            if (!string.IsNullOrEmpty(User.Identity.Name))
            {
                return RedirectToAction("Orders");
            }
            else
            {
                return RedirectToAction("LoginUser");
            }
        }

        public ActionResult Orders()
        {
            string roles = "";
            if (!string.IsNullOrEmpty(User.Identity.Name))
            {
                Role role = db.Roles.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).FirstOrDefault();
                if (role != null)
                    roles = role.Roles;
                if (roles == "Customer")
                {
                    ViewBag.UserLoggedin = true;
                }
            }

            return View();
        }

        public ActionResult ProfessionalDetails(int id)
        {
            string roles = "";
            if (!string.IsNullOrEmpty(User.Identity.Name))
            {
                Role role = db.Roles.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).FirstOrDefault();
                if (role != null)
                    roles = role.Roles;
                if (roles == "Customer")
                {
                    ViewBag.UserLoggedin = true;
                }
            }
            OrderViewModel order = new OrderViewModel();
            var orderrequest = db.OrderRequests.ToList().Where(x => x.OrderId == id && x.IsApproved.HasValue && x.IsApproved.Value).First();
            var partner = db.PartnerProfessionals.ToList().Where(x => x.PartnerProfessionalId == orderrequest.PartnerProfessional.PartnerProfessionalId).First();
            order.OrderId = id;
            order.PartnerProfName = partner.PartnerName; order.PartnerphoneNumber = partner.MobileNo;
            order.AppointmentDate = orderrequest.SelectedDate.Value.ToString("dd-MMM-yyyy") + " " + orderrequest.SelectedTime;


            return View(order);
        }

        public ActionResult ProfessionalDetailsJson()
        {
            var orders = db.Orders.ToList();
            var userobj = db.Users.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).First();
            var customerdata = db.Customers.ToList().Where(x => x.UserId == userobj.UserId).First();

            List<OrderViewModel> data = new List<OrderViewModel>();
            var resultdata = orders.Where(x => x.CustomerId == customerdata.CustomerId).ToList();
            var orderrequestdetails = db.OrderRequests.ToList();
            int ij = 1;
            foreach (var tempdata in resultdata)
            {

                OrderViewModel model = new OrderViewModel();
                model.SNo = ij;
                var subcats = db.Categories.ToList().Where(x => x.CategoryId == tempdata.SubCategoryId).First();
                model.Service = subcats.CategoryName;
                model.Amount = "";//subcats.Price.Value.ToString();
                model.PlacedOn = tempdata.OrderPlacedOn.Value.ToString("dd-MMM-yyyy HH:mm");
                //model.CustomerName = tempdata.Customer.CustomerName;
                //model.AppointmentDate = tempdata.OrderPlacedOn.Value.ToString("dd-MMM-yyyy") + " " + tempdata.OrderTime.ToString();
                model.OrderId = tempdata.OrderId;
                if (tempdata.IsDelivered.HasValue && tempdata.IsDelivered.Value == true)
                    model.Status = "Completed";
                else if (tempdata.IsCancelled.HasValue && tempdata.IsCancelled.Value == true)
                    model.Status = "Cancelled";
                else
                {
                    var orderrqs = orderrequestdetails.Where(x => x.OrderId == tempdata.OrderId).ToList();
                    int cnt = orderrqs.Where(x => x.Status == "Accepted").Count();
                    if (cnt > 0)
                    {
                        //see professional details
                        model.Status = "Accepted";
                    }
                    else
                    {
                        model.Status = "SeeRequests";
                    }

                }
                model.StatusValue = model.Status + "--" + model.OrderId.ToString();
                model.Area = tempdata.Pincode.Pincode1;
                data.Add(model);

            }


            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ApproveProfessional(string id)
        {
            string[] arr = id.Split('-');
            int orderid = Convert.ToInt32(arr[0]);
            int professionalid = Convert.ToInt32(arr[1]);
            var orderFull = db.OrderRequests.Where(x => x.Order.OrderId == orderid).ToList();
            var orderrequest = orderFull.Where(x => x.PartnerProfessional.PartnerProfessionalId == professionalid).First();
            var exceptRequest = orderFull.Where(x => x.OrderRequestId != orderrequest.OrderRequestId).ToList();
            //OrderRequest orderrequest = db.OrderRequests.ToList().Where(x => x.OrderId == orderid && x.PartnerProfessional.PartnerProfessionalId == professionalid).First();
            orderrequest.IsApproved = true;

            db.Entry(orderrequest).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            foreach (var objectreq in exceptRequest)
            {
                db.OrderRequests.Remove(objectreq);
            }
            db.SaveChanges();

            var order = db.Orders.Where(x => x.OrderId == orderrequest.OrderId).First();
            order.IsLocked = 1;
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();


            ViewBag.Message = "Your order has been approved and send for acceptance.Kindly wait for notification";
            string roles = "";
            if (!string.IsNullOrEmpty(User.Identity.Name))
            {
                Role role = db.Roles.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).FirstOrDefault();
                if (role != null)
                    roles = role.Roles;
                if (roles == "Customer")
                {
                    ViewBag.UserLoggedin = true;
                }
            }
            return View("OrderPlaced");
        }

        public ActionResult OrderRequests(int id)
        {
            ViewBag.OrderId = id;
            string roles = "";
            if (!string.IsNullOrEmpty(User.Identity.Name))
            {
                Role role = db.Roles.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).FirstOrDefault();
                if (role != null)
                    roles = role.Roles;
                if (roles == "Customer")
                {
                    ViewBag.UserLoggedin = true;
                }
            }
            return View();
        }

        public ActionResult WaitingForApprovalProf(int id)
        {
            ViewBag.OrderId = id;
            string roles = "";
            if (!string.IsNullOrEmpty(User.Identity.Name))
            {
                Role role = db.Roles.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).FirstOrDefault();
                if (role != null)
                    roles = role.Roles;
                if (roles == "Customer")
                {
                    ViewBag.UserLoggedin = true;
                }
            }
            return View();
        }

        public ActionResult ProvideComment(int id)
        {
            OrderViewModel order = new OrderViewModel();
            ViewBag.OrderId = id;
            List<OrderRating> lst = db.OrderRatings.ToList();
            if (lst.Count > 0)
            {
                order.isRatingDone = true;
                order.Usercomments = lst.First().Usercomments;
                order.Rating = Convert.ToInt32(lst.First().Ratings);
                order.OrderId = id;
            }
            else
            {
                order.isRatingDone = false;
            }


            string roles = "";
            if (!string.IsNullOrEmpty(User.Identity.Name))
            {
                Role role = db.Roles.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).FirstOrDefault();
                if (role != null)
                    roles = role.Roles;
                if (roles == "Customer")
                {
                    ViewBag.UserLoggedin = true;
                }
            }
            return View(order);
        }

        [HttpPost]
        public ActionResult ProvideComment(int id, string txtComments, int Rating)
        {
            ViewBag.OrderId = id;
            string roles = "";
            if (!string.IsNullOrEmpty(User.Identity.Name))
            {
                Role role = db.Roles.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).FirstOrDefault();
                if (role != null)
                    roles = role.Roles;
                if (roles == "Customer")
                {
                    ViewBag.UserLoggedin = true;
                }
            }
            OrderRating order = new OrderRating();
            order.OrderId = id;
            order.Ratings = Rating.ToString();
            order.Usercomments = txtComments;
            db.OrderRatings.Add(order);
            db.SaveChanges();

            return View("Orders");
        }

        public void GenerateInvoicePDF(OrderViewModel order)
        {
            //Dummy data for Invoice (Bill).
            string companyName = "Nearby Services";
            int orderNo = order.OrderId;
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[5] {
                            new DataColumn("ProductId", typeof(string)),
                            new DataColumn("Product", typeof(string)),
                            new DataColumn("Price", typeof(int)),
                            new DataColumn("Quantity", typeof(int)),
                            new DataColumn("Total", typeof(int))});
            dt.Rows.Add(101, "Sun Glasses", 200, 5, 1000);
            dt.Rows.Add(102, "Jeans", 400, 2, 800);
            dt.Rows.Add(103, "Trousers", 300, 3, 900);
            dt.Rows.Add(104, "Shirts", 550, 2, 1100);

            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    StringBuilder sb = new StringBuilder();

                    //Generate Invoice (Bill) Header.
                    sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>");
                    sb.Append("<tr><td align='center' style='background-color: #18B5F0' colspan = '2'><b>Order Sheet</b></td></tr>");
                    sb.Append("<tr><td colspan = '2'></td></tr>");
                    sb.Append("<tr><td><b>Order No: </b>");
                    sb.Append(orderNo);
                    sb.Append("</td><td align = 'right'><b>Date: </b>");
                    sb.Append(DateTime.Now);
                    sb.Append(" </td></tr>");
                    sb.Append("<tr><td colspan = '2'><b>Company Name: </b>");
                    sb.Append(companyName);
                    sb.Append("</td></tr>");
                    sb.Append("</table>");
                    sb.Append("<br />");

                    //Generate Invoice (Bill) Items Grid.
                    sb.Append("<table border = '1'>");
                    sb.Append("<tr>");
                    foreach (DataColumn column in dt.Columns)
                    {
                        sb.Append("<th style = 'background-color: #D20B0C;color:#ffffff'>");
                        sb.Append(column.ColumnName);
                        sb.Append("</th>");
                    }
                    sb.Append("</tr>");
                    foreach (DataRow row in dt.Rows)
                    {
                        sb.Append("<tr>");
                        foreach (DataColumn column in dt.Columns)
                        {
                            sb.Append("<td>");
                            sb.Append(row[column]);
                            sb.Append("</td>");
                        }
                        sb.Append("</tr>");
                    }
                    sb.Append("<tr><td align = 'right' colspan = '");
                    sb.Append(dt.Columns.Count - 1);
                    sb.Append("'>Total</td>");
                    sb.Append("<td>");
                    sb.Append(dt.Compute("sum(Total)", ""));
                    sb.Append("</td>");
                    sb.Append("</tr></table>");

                    //Export HTML String as PDF.
                    StringReader sr = new StringReader(sb.ToString());
                    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                    pdfDoc.Open();
                    htmlparser.Parse(sr);
                    pdfDoc.Close();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=Invoice_" + orderNo + ".pdf");
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Write(pdfDoc);
                    Response.End();
                }
            }
        }

        private string BuildInvoiceFileName(OrderViewModel orderViewModel, string fileExtension)
        {
            return string.Format("Invoice_{0}_{1}.{2}", orderViewModel.PartnerProfName, orderViewModel.OrderId, fileExtension);
        }

        private FileStreamResult BuildFileStream(string fileContent, string fileName, string contentType, bool showInline)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(fileContent);

            using (var input = new MemoryStream(bytes))
            {
                var output = new MemoryStream(); // this MemoryStream is closed by FileStreamResult

                var document = new Document(PageSize.A4, 50, 50, 50, 50);
                var writer = PdfWriter.GetInstance(document, output);
                writer.CloseStream = false;
                document.Open();

                var xmlWorker = XMLWorkerHelper.GetInstance();
                xmlWorker.ParseXHtml(writer, document, input, System.Text.Encoding.Default);
                document.Close();
                output.Position = 0;

                var cd = new ContentDisposition
                {
                    // for example foo.bak
                    FileName = fileName,

                    // always prompt the user for downloading, set to true if you want 
                    // the browser to try to show the file inline
                    Inline = showInline,
                };
                Response.AppendHeader("Content-Disposition", cd.ToString());

                //return new FileStreamResult(output, "application/pdf");
                return File(output, contentType);
            }
        }


        public string ToHtml(string viewToRender, ViewDataDictionary viewData, ControllerContext controllerContext)
        {
            var result = ViewEngines.Engines.FindView(controllerContext, viewToRender, null);

            StringWriter output;
            using (output = new StringWriter())
            {
                var viewContext = new ViewContext(controllerContext, result.View, viewData, controllerContext.Controller.TempData, output);
                result.View.Render(viewContext, output);
                result.ViewEngine.ReleaseView(controllerContext, result.View);
            }

            return output.ToString();
        }

        public ActionResult OrderRequestJson(int id)
        {
            var resultdata = db.OrderRequests.ToList().Where(x => x.OrderId == id).ToList();
            var profs = db.PartnerProfessionals.ToList();
            List<OrderViewModel> data = new List<OrderViewModel>();
            
            
            int ij = 1;
            foreach (var tempdata in resultdata)
            {

                OrderViewModel model = new OrderViewModel();
                model.SNo = ij;
                var partner = profs.Where(x => x.PartnerProfessionalId == tempdata.PartnerProfessional.PartnerProfessionalId).First();
                
                model.OrderId = tempdata.OrderId;
                model.PartnerProfName = partner.PartnerName;
                model.AppointmentDate = tempdata.SelectedDate.Value.ToString("dd-MMM-yyyy") + " " + tempdata.SelectedTime;
                model.Orderwithpartner = tempdata.OrderId.ToString() + "-" + partner.PartnerProfessionalId.ToString();
                data.Add(model);

            }


            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult OrdersJson()
        {
            var orders = db.Orders.ToList();
            var userobj = db.Users.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).First();
            var customerdata = db.Customers.ToList().Where(x => x.UserId == userobj.UserId).First();
            
            List<OrderViewModel> data = new List<OrderViewModel>();
            var resultdata = orders.Where(x => x.CustomerId == customerdata.CustomerId).ToList();
            var orderrequestdetails = db.OrderRequests.ToList();
            int ij = 1;
            List<OrderRating> lstOrderRating = db.OrderRatings.ToList();
            foreach (var tempdata in resultdata)
            {
                
                OrderViewModel model = new OrderViewModel();
                model.SNo = ij;
                var subcats = db.SubCategories.ToList().Where(x => x.SubCategoryId == tempdata.SubCategoryId).First();
                model.Service = subcats.SubCategoryName;
                model.Amount = ""; //subcats.Price.Value.ToString();
                model.PlacedOn = tempdata.OrderPlacedOn.Value.ToString("dd-MMM-yyyy HH:mm");
                //model.CustomerName = tempdata.Customer.CustomerName;
                //model.AppointmentDate = tempdata.OrderPlacedOn.Value.ToString("dd-MMM-yyyy") + " " + tempdata.OrderTime.ToString();
                model.OrderId = tempdata.OrderId;
                string ratingdone = "NotDone";
                var rating = lstOrderRating.Where(x => x.OrderId == tempdata.OrderId).ToList();
                if (rating.Count > 0)
                {
                    model.isRatingDone = true;
                    ratingdone = "Done";
                    model.Usercomments = rating.First().Usercomments;
                    model.Rating = Convert.ToInt32(rating.First().Ratings);
                }
                else
                {
                    model.isRatingDone = false;
                }

                if (tempdata.IsDelivered.HasValue && tempdata.IsDelivered.Value == true)
                    model.Status = "Completed";
                else if (tempdata.IsCancelled.HasValue && tempdata.IsCancelled.Value == true)
                    model.Status = "Cancelled";
                else
                {
                    var orderrqs = orderrequestdetails.Where(x => x.OrderId == tempdata.OrderId).ToList();
                    int cnt = orderrqs.Where(x => x.Status == "Accepted").Count();
                    int cntIsApproved = orderrqs.Where(x => x.IsApproved.HasValue && x.IsApproved.Value == true && x.IsCustomerNotify.HasValue && x.IsCustomerNotify.Value == 1).Count();
                    if (cnt > 0)
                    {
                        //see professional details
                        model.Status = "Accepted";
                    }
                    else if(cntIsApproved > 0)
                    {
                        model.Status = "Waiting for Approval from Professional";
                    }
                    else
                    {
                        model.Status = "See Requests";
                    }

                }
                model.StatusValue = model.Status + "--" + model.OrderId.ToString() + "--" + ratingdone;
                model.Area = customerdata.Pincode.Pincode1;
                model.Remarks = tempdata.Description;
                data.Add(model);
                
            }


            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoginUser(UserViewModel userViewModel)
        {
            if (userViewModel == null)
                userViewModel = new UserViewModel();
            return View(userViewModel);
        }

        [HttpPost]
        public ActionResult LoginUser(UserViewModel userViewModel, string username, string password, string SubCategoryId, string Pincodes, string loginvalue, string registervalue)
        {
            if(registervalue != null && registervalue == "Register")
            {
                UserViewModel userView = new UserViewModel();
                userView.SubCategoryId = SubCategoryId;
                userView.PincodeId = Pincodes;
                ViewBag.Pincodes = new SelectList(db.Pincodes, "PincodeId", "Pincode1");
                return View("RegisterUser", userView);
            }
            var userlist = db.Users.ToList();
            User user = db.Users.ToList().Where(x => x.UserName.ToLower().Replace(" ", "") == username.ToLower().Replace(" ", "") && x.Password == password).FirstOrDefault();
            
            string roles = "";
            if (user != null)
            {
                Role role = db.Roles.ToList().Where(x => x.UserId == user.UserId).FirstOrDefault();
                if (role != null)
                    roles = role.Roles;
                if (roles == "Customer")
                {
                    FormsAuthentication.SetAuthCookie(user.UserId.ToString(), false);
                    
                    if (!string.IsNullOrEmpty(SubCategoryId))
                    {
                        var customer = db.Customers.ToList().Where(x => x.UserId == user.UserId).First();
                        Order order = new Order();
                        order.SubCategoryId = Convert.ToInt32(SubCategoryId);
                        order.Customer = customer;
                        order.OrderPlacedOn = DateTime.Now;
                        order.OrderDate = DateTime.Now;
                        db.Orders.Add(order);
                        db.SaveChanges();
                        ViewBag.Message = "Order Submitted Successfully! Our professional will notify with date and time availibility.";
                        ViewBag.Status = true;
                        ViewBag.UserLoggedin = true;
                        return View("OrderPlaced");
                    }
                    else
                    {

                        ViewBag.UserLoggedin = true;
                        return RedirectToAction("Index", new { isActive = true });
                    }
                }
                
            }

            return View();
        }

        public ActionResult OrderPlaced()
        {
            string roles = "";
            if (!string.IsNullOrEmpty(User.Identity.Name))
            {
                Role role = db.Roles.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).FirstOrDefault();
                if (role != null)
                    roles = role.Roles;
                if (roles == "Customer")
                {
                    ViewBag.UserLoggedin = true;
                }
            }
            return View();
        }

        public ActionResult RegisterMobileUser()
        {
            ViewBag.Pincodes = new SelectList(db.Pincodes, "PincodeId", "Pincode1");
            return View();
        }

        [HttpPost]
        public ActionResult RegisterMobileUser(UserViewModel userViewModel, string Pincodes)
        {

            string Message = "";
            bool isValid = true;
            if (userViewModel != null)
            {
                User user = db.Users.Where(x => x.UserName.ToLower().Replace(" ", "") == userViewModel.Email.ToLower().Replace(" ", "")).FirstOrDefault();
                int cnt = db.Customers.ToList().Where(x => x.MobileNo == userViewModel.MobileNo).Count();
                if (cnt == 0)
                {
                    cnt = db.PartnerProfessionals.ToList().Where(x => x.MobileNo == userViewModel.MobileNo).Count();
                }
                if (user != null)
                {
                    isValid = false;
                    Message = "Email Already exists";
                    ViewBag.StatusMessage = Message;
                    ViewBag.Status = false;
                }
                else if (string.IsNullOrEmpty(userViewModel.Password))
                {
                    isValid = false;
                    Message = "Kindly enter Password";
                    ViewBag.StatusMessage = Message;
                    ViewBag.Status = false;
                }
                else if (string.IsNullOrEmpty(userViewModel.ConfirmPassword) && userViewModel.Password != userViewModel.ConfirmPassword)
                {
                    isValid = false;
                    Message = "Confirm Password does not match";
                    ViewBag.StatusMessage = Message;
                    ViewBag.Status = false;
                }
                else if (string.IsNullOrEmpty(userViewModel.MobileNo))
                {
                    isValid = false;
                    Message = "Kindly provide Mobile No";
                    ViewBag.StatusMessage = Message;
                    ViewBag.Status = false;
                }
                else if(cnt > 0)
                {
                    isValid = false;
                    Message = "Mobile No already exists";
                    ViewBag.StatusMessage = Message;
                    ViewBag.Status = false;
                }
                else if(string.IsNullOrEmpty(Pincodes))
                {
                    isValid = false;
                    Message = "Kindly provide Pincode";
                    ViewBag.StatusMessage = Message;
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
                    role.Roles = "Customer";
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

                    Customer customer = new Customer();
                    customer.Address = userViewModel.Address;
                    customer.CustomerName = userViewModel.Name;
                    customer.MobileNo = userViewModel.MobileNo;
                    customer.UserId = user1.UserId;
                    customer.Pincode = pincode;
                    db.Customers.Add(customer);
                    db.SaveChanges();
                    ViewBag.UserLoggedin = true;
                    FormsAuthentication.SetAuthCookie(user1.UserId.ToString(), false);

                    if (!string.IsNullOrEmpty(userViewModel.SubCategoryId))
                    {
                        Order order = new Order();
                        order.OrderDate = DateTime.Now;
                        order.SubCategoryId = Convert.ToInt32(userViewModel.SubCategoryId);
                        order.Customer = customer;
                        order.OrderPlacedOn = DateTime.Now;

                        db.Orders.Add(order);
                        db.SaveChanges();
                        ViewBag.Message = "Order Submitted Successfully! Our professional will notify with date and time availibility.";
                        ViewBag.Status = true;
                        return View("OrderPlaced");
                    }
                    else
                    {
                        return View("Orders");
                    }







                }
            }


            ViewBag.Pincodes = new SelectList(db.Pincodes, "PincodeId", "Pincode1");
            return View(userViewModel);
        }

        

        public ActionResult RegisterUser()
        {
            ViewBag.Pincodes = new SelectList(db.Pincodes, "PincodeId", "Pincode1");
            return View();
        }

        [HttpPost]
        public ActionResult RegisterUser(UserViewModel userViewModel, string Pincodes)
        {

            string Message = "";
            bool isValid = true;
            if (userViewModel != null)
            {
                User user = db.Users.Where(x => x.UserName.ToLower().Replace(" ","") == userViewModel.Email.ToLower().Replace(" ", "")).FirstOrDefault();
                if (user != null)
                {
                    isValid = false;
                    Message = "Username Already exists";
                    ViewBag.StatusMessage = Message;
                    ViewBag.Status = false;
                }
                else if(string.IsNullOrEmpty(userViewModel.Password))
                {
                    isValid = false;
                    Message = "Kindly enter Password";
                    ViewBag.StatusMessage = Message;
                    ViewBag.Status = false;
                }
                else if(string.IsNullOrEmpty(userViewModel.ConfirmPassword) && userViewModel.Password != userViewModel.ConfirmPassword)
                {
                    isValid = false;
                    Message = "Confirm Password does not match";
                    ViewBag.StatusMessage = Message;
                    ViewBag.Status = false;
                }
                else if(string.IsNullOrEmpty(userViewModel.MobileNo))
                {
                    isValid = false;
                    Message = "Kindly provide Mobile No";
                    ViewBag.StatusMessage = Message;
                    ViewBag.Status = false;
                }
                else if (string.IsNullOrEmpty(Pincodes))
                {
                    isValid = false;
                    Message = "Kindly provide Pincode";
                    ViewBag.StatusMessage = Message;
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
                    role.Roles = "Customer";
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

                    Customer customer = new Customer();
                    customer.Address = userViewModel.Address;
                    customer.CustomerName = userViewModel.Name;
                    customer.MobileNo = userViewModel.MobileNo;
                    customer.UserId = user1.UserId;
                    customer.Pincode = pincode;
                    db.Customers.Add(customer);
                    db.SaveChanges();
                    ViewBag.UserLoggedin = true;
                    FormsAuthentication.SetAuthCookie(user1.UserId.ToString(), false);

                    if (!string.IsNullOrEmpty(userViewModel.SubCategoryId))
                    {
                        Order order = new Order();
                        order.OrderDate = DateTime.Now;
                        order.SubCategoryId = Convert.ToInt32(userViewModel.SubCategoryId);
                        order.Customer = customer;
                        order.OrderPlacedOn = DateTime.Now;

                        db.Orders.Add(order);
                        db.SaveChanges();
                        ViewBag.Message = "Order Submitted Successfully! Our professional will notify with date and time availibility.";
                        ViewBag.Status = true;
                        return View("OrderPlaced");
                    }
                    else
                    {
                        return View("Orders");
                    }

                    
                    
                    

                    

                }
            }


            ViewBag.Pincodes = new SelectList(db.Pincodes, "PincodeId", "Pincode1");
            return View(userViewModel);
        }

        public ActionResult Logout()
        {

            FormsAuthentication.SignOut();
            if (PincodeStaticModel.lstStaticPincodes == null || PincodeStaticModel.lstStaticPincodes.Count == 0)
            {
                PincodeStaticModel.lstStaticPincodes = db.Pincodes.ToList();
            }
            ViewBag.Services = new SelectList(db.SubCategories, "SubCategoryId", "SubCategoryName");
            ViewBag.Pincodes = new SelectList(db.Pincodes, "PincodeId", "Pincode1");
            return RedirectToAction("Index");
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
            if (prefrence.Count > 0)
            {
                PreferOTP preferOTP = db.PreferOTPs.First();
                if (preferOTP.Watsapp.HasValue && preferOTP.Watsapp.Value == true)
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
                    using (var webclient = new WebClient())
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
                return RedirectToAction("LoginUser");
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
            return RedirectToAction("LoginUser");
        }
    }
}