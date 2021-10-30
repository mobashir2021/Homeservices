using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using OnDemandService.Models;

namespace OnDemandService.Areas.Professional.Controllers
{
    public class BillPaymentController : Controller
    {
        private HomeServicesEntities db = new HomeServicesEntities();
        // GET: Professional/BillPayment
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateBill()
        {

            var userobj = db.Users.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).First();
            var orderPartner = db.PartnerProfessionals.ToList().Where(x => x.UserId == userobj.UserId).First();
            //var categories = db.Categories.ToList();
            var orderRequests = db.OrderRequests.Where(x => x.Partnerid == orderPartner.PartnerProfessionalId && x.IsApproved.HasValue && x.IsApproved.Value && x.Status == "Accepted").ToList();
            List<OrderWithProfessional> profOrders = db.OrderWithProfessionals.Where(x => x.ProfessionalId == orderPartner.PartnerProfessionalId).ToList();
            List<Order> deliveredOrder = db.Orders.Where(x => x.IsDelivered.HasValue && x.IsDelivered.Value == true).ToList();
            List<int> intersectedOrders = deliveredOrder.Select(x => x.OrderId).Intersect(profOrders.Select(x => x.OrderId)).ToList();
            var totalorders = (from ord in deliveredOrder
                               join pro in intersectedOrders on ord.OrderId equals pro
                               select ord).ToList();
            HashSet<int> bills = new HashSet<int>(db.BillPayments.Where(x => x.ProfessionalId == orderPartner.PartnerProfessionalId).Select(x => x.OrderId).ToList());
            //List<BillOrderSelection> order = new List<BillOrderSelection>();
            List<Order> orders = new List<Order>();
            List<CustomerViewModel> customers = new List<CustomerViewModel>();
            List<Customer> tempCustomer = db.Customers.ToList();
            List<Customer> allowedCustomer = (from cc in tempCustomer
                            join ord in totalorders on cc.CustomerId equals ord.CustomerId
                            select cc).ToList();
            List<User> tempusers = db.Users.ToList();
            List<UserViewModelOrder> allowedUser = new List<UserViewModelOrder>();
            foreach (var cs in allowedCustomer)
            {
                User us = tempusers.Where(x => x.UserId == cs.UserId).First();
                UserViewModelOrder user = new UserViewModelOrder()
                {
                    CustomerId = cs.CustomerId,
                    UserId = us.UserId,
                    Customername = cs.CustomerName,
                    Useremail = us.UserName,
                    Mobileno = cs.MobileNo
                };
                allowedUser.Add(user);

            }
            
            foreach (var data in totalorders)
            {
                if (!bills.Contains(data.OrderId))
                {

                    var orderReqObj = orderRequests.Where(x => x.OrderId == data.OrderId).FirstOrDefault();
                    if (orderReqObj != null)
                    {
                        //Category cat = categories.Where(x => x.CategoryId == data.SubCategoryId).FirstOrDefault();
                        //BillOrderSelection bill = new BillOrderSelection();
                        //bill.OrderId = data.OrderId;
                        //bill.Ordername = cat.CategoryName + " - " + orderReqObj.SelectedDate.Value.ToString("HH-MMM-yyyy") + " " + orderReqObj.SelectedTime;
                        //order.Add(bill);
                        CustomerViewModel customer = new CustomerViewModel();
                        UserViewModelOrder user = allowedUser.Where(x => x.CustomerId == data.CustomerId).First();
                        customer.Customerid = user.CustomerId.ToString() + "~" + data.OrderId.ToString();
                        customer.CustomerEmail = user.Useremail + "   -- Order Date: " + orderReqObj.SelectedDate.Value.ToString("dd-MMM-yyyy") + " " + orderReqObj.SelectedTime;

                        customers.Add(customer);
                    }
                }
            }

            //ViewBag.Services = new SelectList(order, "OrderId", "Ordername");

            ViewBag.Customers = new SelectList(customers, "Customerid", "CustomerEmail");
            ViewBag.Services = new SelectList(db.Categories, "CategoryId", "CategoryName");
            return View();
        }

        public JsonResult GetSubcategory(int categoryid)
        {
            db.Configuration.ProxyCreationEnabled = false;

            List<SubCategory> subs = db.SubCategories.Where(x => x.CategoryId == categoryid).ToList();
            List<BillOrderSubSelection> subsdata = new List<BillOrderSubSelection>();
            foreach(var data in subs)
            {
                BillOrderSubSelection bill = new BillOrderSubSelection();
                bill.SubCategoryId = data.SubCategoryId.ToString() + "~" + data.Price.Value.ToString();
                bill.SubCategoryName = data.SubCategoryName;
                subsdata.Add(bill);
            }
            return Json(subsdata, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveBillDetails(string billArray)
        {
            int Categoryid = 0;
            int Customerid = 0;
            int OrderId = 0;
            string totalprice = "";
            string cgst = "";
            string sgst = "";
            //string textBody = " <table border=" + 1 + " cellpadding=" + 0 + " cellspacing=" + 0 + " width = " + 400 + "><tr bgcolor='#4da6ff'><td><b>Column 1</b></td> <td> <b> Column 2</b> </td></tr>";
            //for (int loopCount = 0; loopCount < data_table.Rows.Count; loopCount++)
            //{
            //    textBody += "<tr><td>" + data_table.Rows[loopCount]["RowName"] + "</td><td> " + data_table.Rows[loopCount]["RowName2"] + "</td> </tr>";
            //}
            //textBody += "</table>";

            

            string[] splitAnd = billArray.Split('&');
            for(int i = 0; i < splitAnd.Length; i++)
            {
                if(i == 0)
                {
                    string[] tempsplit = splitAnd[0].Split('=');
                    Categoryid = Convert.ToInt32(tempsplit[1]);
                }
                else if(i == 1)
                {
                    string[] tempsplit = splitAnd[i].Split('=');
                    string[] innerTempsplit = tempsplit[1].Split('~');
                    Customerid = Convert.ToInt32(innerTempsplit[0]);
                    OrderId = Convert.ToInt32(innerTempsplit[1]);
                }
                else if (i == splitAnd.Length - 1)
                {
                    string[] tempsplit = splitAnd[i].Split('=');
                    totalprice = tempsplit[1];
                }
                else if (i == splitAnd.Length - 2)
                {
                    string[] tempsplit = splitAnd[i].Split('=');
                    sgst = tempsplit[1];
                }
                else if (i == splitAnd.Length - 3)
                {
                    string[] tempsplit = splitAnd[i].Split('=');
                    cgst = tempsplit[1];
                }
            }

            string Categroyname = db.Categories.Where(x => x.CategoryId == Categoryid).First().CategoryName;
            OrderRequest ord = db.OrderRequests.Where(x => x.OrderId == OrderId && x.IsApproved.HasValue && x.IsApproved.Value == true && x.Status == "Accepted").First();
            string orderdate = ord.SelectedDate.Value.ToString("dd-MMM-yyyy") + " " + ord.SelectedTime;
            string Message = "<p>Category : "+ Categroyname + "</p><br />";
            Message += "<p> Order Date : "+ orderdate +" </p> <br />";
            Message += "<p> Total price : " + totalprice + " </p> <br />";

            string textBody = " <table border=" + 1 + " cellpadding=" + 0 + " cellspacing=" + 0 + " width = " + 400 + ">" +
                "<tr bgcolor='#4da6ff'><td><b>Subcategory</b></td> <td> <b> Quantity</b> </td> <td> <b> Base Price</b> </td></tr>";
            //for (int loopCount = 0; loopCount < data_table.Rows.Count; loopCount++)
            //{
            //    textBody += "<tr><td>" + data_table.Rows[loopCount]["RowName"] + "</td><td> " + data_table.Rows[loopCount]["RowName2"] + "</td> </tr>";
            //}
            


            int professionalId = db.OrderWithProfessionals.Where(x => x.OrderId == OrderId).First().ProfessionalId;
            BillPayment bill = new BillPayment();
            bill.ProfessionalId = professionalId;
            bill.CategoryId = Categoryid;
            bill.CustomerId = Customerid;
            bill.OrderId = OrderId;
            bill.IsApproved = true;
            bill.CGST = cgst;
            bill.SGST = sgst;
            bill.TotalPrice = Convert.ToInt32(totalprice);
            db.BillPayments.Add(bill);
            db.SaveChanges();

            var subcates = db.SubCategories.ToList();

            for (int i = 0; i < splitAnd.Length; i++)
            {
                int SubCategoryid = 0;
                int quantity = 0;
                int price = 0;
                if (splitAnd[i].StartsWith("Subcategory"))
                {
                    string[] tempsplit = splitAnd[0].Split('=');
                    string[] innerTempsplit = tempsplit[1].Split('~');
                    SubCategoryid = Convert.ToInt32(innerTempsplit[0]);
                    SubCategory sub = subcates.Where(x => x.SubCategoryId == SubCategoryid).First();
                    string s1 = splitAnd[i + 1];
                    string s2 = splitAnd[i + 2];
                    string[] tempsplit1 = s1.Split('=');
                    string[] tempsplit2 = s2.Split('=');

                    quantity = Convert.ToInt32(tempsplit1[1]);
                    //price = Convert.ToInt32(tempsplit2[1]);

                    BillPaymentDetail detail = new BillPaymentDetail();
                    detail.BillPaymentId = bill.BillPaymentId;
                    detail.SubCategoryId = SubCategoryid;
                    detail.Price = price.ToString();
                    detail.IsApproved = true;
                    detail.Quantity = quantity;
                    db.BillPaymentDetails.Add(detail);
                    db.SaveChanges();
                    textBody += "<tr><td>" + sub.SubCategoryName + "</td><td> " + quantity.ToString() + "</td><td> " + tempsplit2[1].ToString() + "</td> </tr>";

                }
            }
            textBody += "</table>";
            textBody = Message + textBody;
            Customer cust = db.Customers.Where(x => x.CustomerId == Customerid).First();
            User user = db.Users.Where(x => x.UserId == cust.UserId).First();
            SendMail(user.UserName, "Order Receipt No : " + OrderId.ToString(), textBody);


            return Json(new { success = true });
        }

        public void SendMail(string To, string Subject,   string textBody)
        {
            
            var Mailcredentials = db.MailCredentials.First();
            var companyinfo = db.CompanyInfoes.First();
            var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
            var message = new MailMessage();
            message.To.Add(new MailAddress(To));  // replace with valid value 
            message.From = new MailAddress(Mailcredentials.Email);  // replace with valid value
            message.Subject = "Order Receipt";
            message.Body = string.Format(body, companyinfo.Companyname, Mailcredentials.Email, textBody);
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                smtp.UseDefaultCredentials = false;
                var credential = new NetworkCredential
                {
                    UserName = Mailcredentials.Email,  // replace with valid value
                    Password = Mailcredentials.Password  // replace with valid value
                };
                smtp.Credentials = credential;
                
                smtp.Host = Mailcredentials.Host; //"smtp-mail.outlook.com";
                smtp.Port = Mailcredentials.PortNo.Value; //587;
                smtp.EnableSsl = Mailcredentials.EnableSSL.Value;
                smtp.Send(message);
                
            }
        }

        public ActionResult BillPaymentListJson()
        {
            var userobj = db.Users.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).First();
            var partnerprof = db.PartnerProfessionals.ToList().Where(x => x.UserId == userobj.UserId).First();
            var billpayments = db.BillPayments.Where(x => x.ProfessionalId == partnerprof.PartnerProfessionalId).OrderByDescending(x => x.BillCreationDate.Value).ToList();

            List<OrderViewModel> data = new List<OrderViewModel>();
            var categories = db.Categories.ToList();
            var customers = db.Customers.ToList();
            var ordersrequest = db.OrderRequests.ToList();
            int ij = 1;
            foreach (var tempdata in billpayments)
            {
                OrderViewModel model = new OrderViewModel();
                model.SNo = ij;
                var subcats = categories.Where(x => x.CategoryId == tempdata.CategoryId).First();
                model.Service = subcats.CategoryName;
                model.Amount = ""; // subcats.Price.Value.ToString();
                Customer cust = customers.Where(x => x.CustomerId == tempdata.CustomerId).First();
                model.CustomerName = cust.CustomerName;
                OrderRequest orderRequest = ordersrequest.Where(x => x.OrderId == tempdata.OrderId).First();
                model.AppointmentDate = orderRequest.SelectedDate.Value.ToString("dd-MMM-yyyy") + " " + orderRequest.SelectedTime;
                model.BillPaymentId = tempdata.BillPaymentId;
                model.BillTotalPrice = tempdata.TotalPrice.Value.ToString();
                data.Add(model);
                
            }


            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BillPaymentDetailsData(int id)
        {
            var userobj = db.Users.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).First();
            var partnerprof = db.PartnerProfessionals.ToList().Where(x => x.UserId == userobj.UserId).First();
            var billpayments = db.BillPayments.Where(x => x.BillPaymentId == id).First();

            
            var categories = db.Categories.ToList();
            var subcategories = db.SubCategories.ToList();
            var customers = db.Customers.ToList();
            var ordersrequest = db.OrderRequests.ToList();
            OrderViewModel model = new OrderViewModel();
            
            var subcats = categories.Where(x => x.CategoryId == billpayments.CategoryId).First();
            model.Service = subcats.CategoryName;
            model.Amount = ""; // subcats.Price.Value.ToString();
            Customer cust = customers.Where(x => x.CustomerId == billpayments.CustomerId).First();
            model.CustomerName = cust.CustomerName;
            OrderRequest orderRequest = ordersrequest.Where(x => x.OrderId == billpayments.OrderId).First();
            model.AppointmentDate = orderRequest.SelectedDate.Value.ToString("dd-MMM-yyyy") + " " + orderRequest.SelectedTime;
            model.BillPaymentId = billpayments.BillPaymentId;
            model.BillTotalPrice = billpayments.TotalPrice.Value.ToString();
            model.CGST = billpayments.CGST;
            model.SGST = billpayments.SGST;

            List<BillPaymentDetailsInnerVM> lst = new List<BillPaymentDetailsInnerVM>();
            var billdetails = db.BillPaymentDetails.Where(x => x.BillPaymentId == id);
            foreach(var tempdata in billdetails)
            {
                BillPaymentDetailsInnerVM vM = new BillPaymentDetailsInnerVM();
                vM.SubcategoryId = tempdata.SubCategoryId.Value;
                SubCategory sub = subcategories.Where(x => x.SubCategoryId == vM.SubcategoryId).First();
                vM.Subcategoryname = sub.SubCategoryName;
                vM.Price = tempdata.Price;
                vM.quantity = tempdata.Quantity.Value;
                lst.Add(vM);
            }
            model.paymentDetails = lst;


            return View(model);
        }
    }
}