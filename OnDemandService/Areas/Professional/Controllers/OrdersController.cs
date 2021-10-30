﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnDemandService.Models;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.Net.Mime;

namespace OnDemandService.Areas.Professional.Controllers
{
    public class OrdersController : Controller
    {
        private HomeServicesEntities db = new HomeServicesEntities();

        // GET: Professional/Orders

        public ActionResult Index()
        {
             
            return View();
        }

        public ActionResult OngoingAppointment()
        {
            var orders = db.Orders.Include(o => o.Customer).Include(o => o.PaymentMethod).Include(o => o.Pincode).ToList();
            var userobj = db.Users.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).First();
            var partner = db.PartnerProfessionals.ToList().Where(x => x.UserId == userobj.UserId).First();
            var orderPartner = db.OrderWithProfessionals.ToList().Where(x => x.ProfessionalId == partner.PartnerProfessionalId).ToList();

            var orderreq = db.OrderRequests.ToList();
          

            List<OrderViewModel> data = new List<OrderViewModel>();
            var resultdata = (from mainorder in orders
                              join pat in orderPartner on mainorder.OrderId equals pat.OrderId
                              select mainorder).ToList();
            int ij = 1;
            foreach(var tempdata in resultdata)
            {
                if (tempdata.IsGoingOn.HasValue && tempdata.IsGoingOn.Value == true)
                {
                    OrderRequest request = orderreq.Where(x => x.OrderId == tempdata.OrderId && x.PartnerProfessional.PartnerProfessionalId == partner.PartnerProfessionalId).First();
                    OrderViewModel model = new OrderViewModel();
                    model.SNo = ij;
                    var subcats = db.Categories.ToList().Where(x => x.CategoryId == tempdata.SubCategoryId).First();
                    model.Service = subcats.CategoryName;
                    model.Amount = "";// subcats.Price.Value.ToString();
                    model.CustomerName = tempdata.Customer.CustomerName;
                    model.AppointmentDate = request.SelectedDate.Value.ToString("dd-MMM-yyyy") + " " + request.SelectedTime;
                    model.OrderId = tempdata.OrderId;
                    model.Status = "Ongoing";
                    model.Area = tempdata.Pincode.Pincode1;
                    data.Add(model);
                }
            }


            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult NewAppointmentJson()
        {
            var profesionalorders = db.OrderWithProfessionals.ToList();
            var orders = db.Orders.Include(o => o.Customer).Include(o => o.PaymentMethod).Include(o => o.Pincode).Where(x => x.IsLocked.HasValue == false || x.IsLocked.Value == 0);
            //orders = orders.ToList();
            var ordersids = orders.ToList().Select(x => x.OrderId).ToList();
            var profids = profesionalorders.Select(x => x.OrderId).ToList();
            var neworderids = ordersids.Except(profids).ToList();
            var pincodes = db.Pincodes.ToList();
            var userobj = db.Users.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).First();
            var orderPartner = db.PartnerProfessionals.ToList().Where(x => x.UserId == userobj.UserId).First();
            var orderrequest = db.OrderRequests.ToList().Where(x => (!x.IsApproved.HasValue || x.IsApproved.Value == false) && x.PartnerProfessional.PartnerProfessionalId == orderPartner.PartnerProfessionalId).ToList();
            var newfinalorder = new List<Order>();
            var serviceprovided = db.ServiceProvideds.ToList().Where(x => x.PartnerProfessional.PartnerProfessionalId == orderPartner.PartnerProfessionalId).ToList();
            foreach (var orderdata in neworderids)
            {
                var currorder = orders.Where(x => x.OrderId == orderdata).First();
                
                
                int cntdata = serviceprovided.Where(x => x.CategoryId.Value == currorder.SubCategoryId).Count();
                if (cntdata == 0)
                    continue;
                Pincode orderpincode = pincodes.Where(x => x.PincodeId == currorder.Customer.Pincode.PincodeId).First();
                Pincode profpincode = pincodes.Where(x => x.PincodeId == orderPartner.Pincode.PincodeId).First();
                var distance = GetDistanceBetweenPoints(orderpincode.Latitude.Value, orderpincode.Longitude.Value, profpincode.Latitude.Value, profpincode.Longitude.Value);
                if(Math.Round(distance) <= 5)
                {
                    newfinalorder.Add(currorder);
                }
            
            }

            //List<OrderViewModel> data = GetOrderViewModels(orders.ToList(), "Ongoing");
            List<OrderViewModel> data = GetOrderViewModels(newfinalorder.ToList(), "");



            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AcceptedRequestAppointmentJson()
        {

            var orders = db.Orders.ToList();
            var subcategories = db.Categories.ToList();
            var userobj = db.Users.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).First();
            var orderPartner = db.PartnerProfessionals.ToList().Where(x => x.UserId == userobj.UserId).First();
            var orderrequest = db.OrderRequests.ToList().Where(x => x.Status == "Accepted" && x.PartnerProfessional.PartnerProfessionalId == orderPartner.PartnerProfessionalId).ToList();
            //var completedorCancel = orders.Where(x => (x.IsCancelled.HasValue && x.IsCancelled.Value) || (x.IsDelivered.HasValue && x.IsDelivered.Value)).Select(x => x.OrderId).ToList();
            var customer = db.Customers.ToList();
            int ij = 1;
            List<OrderViewModel> data = new List<OrderViewModel>();
            foreach (var orderdata in orderrequest)
            {

                var order = orders.Where(x => x.OrderId == orderdata.OrderId).First();
                if ((order.IsDelivered.HasValue && order.IsDelivered.Value) || (order.IsCancelled.HasValue && order.IsCancelled.Value))
                    continue;
                var subcat = subcategories.Where(x => x.CategoryId == order.SubCategoryId).First();
                var customerobj = customer.Where(x => x.CustomerId == order.Customer.CustomerId).First();
                OrderViewModel orderView = new OrderViewModel();
                orderView.SNo = ij;
                orderView.OrderId = orderdata.OrderId;
                orderView.Service = subcat.CategoryName;
                orderView.CustomerAddress = customerobj.Address;
                orderView.CustomerPhoneNumber = customerobj.MobileNo;
                orderView.CustomerName = customerobj.CustomerName;
                orderView.AppointmentDate = orderdata.SelectedDate.Value.ToString("dd-MMM-yyyy") + " " + orderdata.SelectedTime;
                
                ij++;
                data.Add(orderView);
            }

            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PendingRequestAppointmentJson()
        {

            var orders = db.Orders.ToList();
            var subcategories = db.Categories.ToList();
            var userobj = db.Users.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).First();
            var orderPartner = db.PartnerProfessionals.ToList().Where(x => x.UserId == userobj.UserId).First();
            var orderrequest = db.OrderRequests.ToList().Where(x => x.Status != "Accepted" && x.PartnerProfessional.PartnerProfessionalId == orderPartner.PartnerProfessionalId).ToList();
            var customer = db.Customers.ToList();
            int ij = 1;
            List<OrderViewModel> data = new List<OrderViewModel>();
            foreach (var orderdata in orderrequest)
            {
                var order = orders.Where(x => x.OrderId == orderdata.OrderId).First();
                var subcat = subcategories.Where(x => x.CategoryId == order.SubCategoryId).First();
                var customerobj = customer.Where(x => x.CustomerId == order.Customer.CustomerId).First();
                OrderViewModel orderView = new OrderViewModel();
                orderView.SNo = ij;
                orderView.OrderId = orderdata.OrderId;
                orderView.Service = subcat.CategoryName;
                orderView.CustomerName = customerobj.CustomerName;
                orderView.AppointmentDate = orderdata.SelectedDate.Value.ToString("dd-MMM-yyyy") + " " + orderdata.SelectedTime;
                if (orderdata.IsApproved.HasValue && orderdata.IsApproved.Value)
                    orderView.StatusValue = "~" + orderdata.OrderId.ToString();
                else
                    orderView.StatusValue = "Pending";
                ij++;
                data.Add(orderView);
            }

            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public List<OrderViewModel> GetOrderViewModels(List<Order> orders, string Status = "New")
        {
            int ij = 1;
            List<OrderViewModel> data = new List<OrderViewModel>();
            foreach (var tempdata in orders)
            {
                OrderViewModel model = new OrderViewModel();
                model.SNo = ij;
                var subcats = db.Categories.ToList().Where(x => x.CategoryId == tempdata.SubCategoryId).First();
                model.Service = subcats.CategoryName;
                model.Amount = "";// subcats.Price.Value.ToString();
                model.CustomerName = tempdata.Customer.CustomerName;
                model.AppointmentDate = "";
                model.OrderId = tempdata.OrderId;
                model.Status = Status;
                model.Area = tempdata.Customer.Pincode.Pincode1;
                ij++;
                data.Add(model);
            }
            return data;
        }



        public ActionResult CompletedAppointmentJson()
        {
            var orders = db.Orders.Include(o => o.Customer).Include(o => o.PaymentMethod).Include(o => o.Pincode).ToList();
            var userobj = db.Users.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).First();
            var partnerprof = db.PartnerProfessionals.ToList().Where(x => x.UserId == userobj.UserId).First();
            var orderPartner = db.OrderWithProfessionals.ToList().Where(x => x.PartnerProfessional.PartnerProfessionalId == partnerprof.PartnerProfessionalId).ToList();
            var orderreq = db.OrderRequests.ToList();
            
            List<OrderViewModel> data = new List<OrderViewModel>();
            var resultdata = (from mainorder in orders
                              join pat in orderPartner on mainorder.OrderId equals pat.OrderId
                              select mainorder).ToList();
            int ij = 1;
            foreach (var tempdata in resultdata)
            {
                if (tempdata.IsDelivered.HasValue && tempdata.IsDelivered.Value == true)
                {
                    OrderRequest request = orderreq.Where(x => x.OrderId == tempdata.OrderId && x.PartnerProfessional.PartnerProfessionalId == partnerprof.PartnerProfessionalId).First();
                    OrderViewModel model = new OrderViewModel();
                    model.SNo = ij;
                    var subcats = db.Categories.ToList().Where(x => x.CategoryId == tempdata.SubCategoryId).First();
                    model.Service = subcats.CategoryName;
                    model.Amount = ""; // subcats.Price.Value.ToString();
                    model.CustomerName = tempdata.Customer.CustomerName;
                    model.AppointmentDate = request.SelectedDate.Value.ToString("dd-MMM-yyyy") + " " + request.SelectedTime;
                    model.OrderId = tempdata.OrderId;
                    model.Status = "Ongoing";
                    model.Area = tempdata.Customer.Pincode.Pincode1;
                    data.Add(model);
                }
            }


            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CancelledAppointmentJson()
        {
            var orders = db.Orders.Include(o => o.Customer).Include(o => o.PaymentMethod).Include(o => o.Pincode).ToList();
            var userobj = db.Users.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).First();
            var partnerprof = db.PartnerProfessionals.ToList().Where(x => x.UserId == userobj.UserId).First();
            var orderPartner = db.OrderWithProfessionals.ToList().Where(x => x.PartnerProfessional.PartnerProfessionalId == partnerprof.PartnerProfessionalId).ToList();
            var orderreq = db.OrderRequests.ToList();

            List<OrderViewModel> data = new List<OrderViewModel>();
            var resultdata = (from mainorder in orders
                              join pat in orderPartner on mainorder.OrderId equals pat.OrderId
                              select mainorder).ToList();
            int ij = 1;
            foreach (var tempdata in resultdata)
            {
                if (tempdata.IsCancelled.HasValue && tempdata.IsCancelled.Value == true)
                {
                    OrderRequest request = orderreq.Where(x => x.OrderId == tempdata.OrderId && x.PartnerProfessional.PartnerProfessionalId == partnerprof.PartnerProfessionalId).First();
                    OrderViewModel model = new OrderViewModel();
                    model.SNo = ij;
                    var subcats = db.Categories.ToList().Where(x => x.CategoryId == tempdata.SubCategoryId).First();
                    model.Service = subcats.CategoryName;
                    model.Amount = ""; // subcats.Price.Value.ToString();
                    model.CustomerName = tempdata.Customer.CustomerName;
                    model.AppointmentDate = request.SelectedDate.Value.ToString("dd-MMM-yyyy") + " " + request.SelectedTime.ToString();
                    model.OrderId = tempdata.OrderId;
                    model.Status = "Ongoing";
                    model.Area = tempdata.Pincode.Pincode1;
                    data.Add(model);
                }
            }


            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AcceptRequest(string id)
        {
            ViewBag.OrderId = Convert.ToInt32(id);
            var userobj = db.Users.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).First();
            var partnerprof = db.PartnerProfessionals.ToList().Where(x => x.UserId == userobj.UserId).First();
            ViewBag.ParnerId = partnerprof.PartnerProfessionalId;
            return View();
        }

        [HttpPost]
        public ActionResult AcceptRequest(int OrderId, int PartnerId)
        {
            var orderFull = db.OrderRequests.Where(x => x.Order.OrderId == OrderId).ToList();
            var orderRequest = orderFull.Where(x => x.PartnerProfessional.PartnerProfessionalId == PartnerId).First();
            //var exceptRequest = orderFull.Where(x => x.OrderRequestId != orderRequest.OrderRequestId).ToList();
            orderRequest.Status = "Accepted"; orderRequest.IsApproved = true; orderRequest.IsCustomerNotify = 0;
            db.Entry(orderRequest).State = EntityState.Modified;
            db.SaveChanges();
            var orderobj = db.Orders.ToList().Where(x => x.OrderId == OrderId).First();
            var partner = db.PartnerProfessionals.ToList().Where(x => x.PartnerProfessionalId == PartnerId).First();

            orderobj.IsGoingOn = true;
            db.Entry(orderobj).State = EntityState.Modified;
            db.SaveChanges();


            OrderWithProfessional order = new OrderWithProfessional();
            order.Order = orderobj;
            order.PartnerProfessional = partner;
            db.OrderWithProfessionals.Add(order);
            db.SaveChanges();

            //foreach(var objectreq in exceptRequest)
            //{
            //    db.OrderRequests.Remove(objectreq);
            //}
            //db.SaveChanges();

            //var order = db.Orders.Where(x => x.OrderId == OrderId).First();
            

            return View("Accepted");
        }

        [HttpGet]
        public ActionResult ChangeStatus(int id)
        {
            ViewBag.OrderId = id;
            return View();
            //return (RedirectToAction("ChangeStatus", new { OrderId = id }));
        }

        [HttpGet]
        public ActionResult Approve(int id)
        {
            ViewBag.OrderId = id;
            return View();
            //return (RedirectToAction("ChangeStatus", new { OrderId = id }));
        }

        public ActionResult ChangeStatusMain(int id)
        {
            ViewBag.OrderId = id;
            //return View("ChangeStatus");
            return (RedirectToAction("ChangeStatus", new { id = id.ToString() }));
        }

        private void InsertOrderWithProf(int OrderId, int ProfessionalId)
        {
            int returnvalue = 0;
            string connstr = ConfigurationManager.ConnectionStrings["sqlConString"].ConnectionString;
            using(SqlConnection conn = new SqlConnection(connstr))
            {
                SqlCommand comm = new SqlCommand("InsertOrderWithProfessional", conn);
                comm.CommandText = "InsertOrderWithProfessional";
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.AddWithValue("@OrderId", OrderId);
                comm.Parameters.AddWithValue("@ProfessionalId", ProfessionalId);
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                comm.ExecuteNonQuery();
                

            }
            
        }
        //SendRequest with time for next Client approval
        public ActionResult ApproveAppointment(string OrderId,string selecteddatetime)
        {
            var order = db.Orders.ToList().Where(x => x.OrderId == Convert.ToInt32(OrderId)).FirstOrDefault();
            var customer = db.Customers.ToList().Where(x => x.CustomerId == order.Customer.CustomerId).First();
            var userobj = db.Users.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).First();
            var professional = db.PartnerProfessionals.ToList().Where(x => x.UserId == userobj.UserId).First();

            DateTime selected = Convert.ToDateTime(selecteddatetime);
            OrderRequest orderRequest = new OrderRequest();
            orderRequest.Customer = customer;
            orderRequest.PartnerProfessional = professional;
            orderRequest.SelectedDate = Convert.ToDateTime(selected.ToString("dd-MMM-yyyy"));
            orderRequest.SelectedTime = selected.ToString("HH:mm");
            orderRequest.Status = "";
            orderRequest.Order = order;
            db.OrderRequests.Add(orderRequest);
            db.SaveChanges();

            return View("ApprovedAppointments");


            //var countorder = db.OrderWithProfessionals.ToList().Where(x => x.Order.OrderId == Convert.ToInt32(OrderId)).Count();
            //if (countorder > 0)
            //{
            //    ViewBag.Message = "Appointment declined";
            //    return View("NewAppointments");
            //}
            //else
            //{
            //    var userobj = db.Users.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).First();
            //    var professional = db.PartnerProfessionals.ToList().Where(x => x.UserId == userobj.UserId).First();
            //    order.IsGoingOn = true;
            //    order.IsActive = true;
            //    order.IsDelivered = false;
            //    order.IsCancelled = false;

            //    OrderWithProfessional orderProf = new OrderWithProfessional();
            //    orderProf.IsCancelled = false; orderProf.IsCompleted = false; orderProf.Order = order; orderProf.PartnerProfessional = professional;
            //    //orderProf.ProfessionalId = professional.PartnerProfessionalId; orderProf.OrderId = order.OrderId;



            //    //db.OrderWithProfessionals.Add(orderProf);
            //    //db.SaveChanges();
            //    InsertOrderWithProf(order.OrderId, professional.PartnerProfessionalId);

            //    var listowp = db.OrderWithProfessionals.ToList().Where(x => x.OrderId == order.OrderId).ToList();
            //    //List<OrderWithProfessional> listowp = new List<OrderWithProfessional>();
            //    //listowp.Add(orderProf);
            //    order.OrderWithProfessionals = listowp;
            //    db.Entry(order).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return View("Index");
            //}

        }

        public ActionResult ApproveAppointmentOld(string OrderId)
        {
            var order = db.Orders.ToList().Where(x => x.OrderId == Convert.ToInt32(OrderId)).FirstOrDefault();
            var countorder = db.OrderWithProfessionals.ToList().Where(x => x.Order.OrderId == Convert.ToInt32(OrderId)).Count();
            if(countorder > 0)
            {
                ViewBag.Message = "Appointment declined";
                return View("NewAppointments");
            }
            else
            {
                var userobj = db.Users.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).First();
                var professional = db.PartnerProfessionals.ToList().Where(x => x.UserId == userobj.UserId).First();
                order.IsGoingOn = true;
                order.IsActive = true;
                order.IsDelivered = false;
                order.IsCancelled = false;

                OrderWithProfessional orderProf = new OrderWithProfessional();
                orderProf.IsCancelled = false; orderProf.IsCompleted = false; orderProf.Order = order; orderProf.PartnerProfessional = professional;
                //orderProf.ProfessionalId = professional.PartnerProfessionalId; orderProf.OrderId = order.OrderId;



                //db.OrderWithProfessionals.Add(orderProf);
                //db.SaveChanges();
                InsertOrderWithProf(order.OrderId, professional.PartnerProfessionalId);

                var listowp = db.OrderWithProfessionals.ToList().Where(x => x.OrderId == order.OrderId).ToList();
                //List<OrderWithProfessional> listowp = new List<OrderWithProfessional>();
                //listowp.Add(orderProf);
                order.OrderWithProfessionals = listowp;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return View("Index");
            }
            
        }

       
        public JsonResult GetNotification()
        {
            if (User.Identity.Name == "")
            {
                NotificationModel notificationtemp = new NotificationModel();
                notificationtemp.Notification = "";
                notificationtemp.count = 0;

                return Json(notificationtemp, JsonRequestBehavior.AllowGet);
            }
            var userobj = db.Users.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).First();
            var professional = db.PartnerProfessionals.Include(o => o.Pincode).ToList().Where(x => x.UserId == userobj.UserId).First();
            var pincodes = db.Pincodes.ToList();
            var subcategories = db.Categories.ToList();
            var lstOrderReqs = db.OrderRequests.Include(o => o.Customer).Include(o => o.Order).Where( x => x.PartnerProfessional.PartnerProfessionalId == professional.PartnerProfessionalId &&
                (!x.NotifyStatus.HasValue || x.NotifyStatus.Value == 0)).ToList();
            
            var isapprovedButNotAccepted = lstOrderReqs.Where(x => x.IsApproved.HasValue && x.IsApproved.Value && x.Status != "Accepted").ToList();
            var newOrderslst = db.Orders.Where(x => x.IsLocked.HasValue == false || x.IsLocked.Value == 0).ToList();
            var ids = newOrderslst.Select(x => x.OrderId).Except(lstOrderReqs.Select(x => x.Order.OrderId)).ToList();
            var serviceprovided = db.ServiceProvideds.ToList().Where(x => x.PartnerProfessional.PartnerProfessionalId == professional.PartnerProfessionalId).ToList();
            List<Order> tempnewOrder = new List<Order>();
            foreach(var neworder in newOrderslst)
            {
                int cntdata = serviceprovided.Where(x => x.CategoryId.Value == neworder.SubCategoryId).Count();
                if (cntdata > 0)
                    tempnewOrder.Add(neworder);
            }
            newOrderslst = tempnewOrder;

            var newOrdersAll = (from ord in newOrderslst
                                join id in ids on ord.OrderId equals id
                                select ord).ToList();

            var dbcustomers = db.Customers.ToList();
            var notificatonshowns = db.IsNotificationShowns
                .Where(x => x.PartnerProfessional.PartnerProfessionalId == professional.PartnerProfessionalId).Select(x => x.Order.OrderId).ToList();
            List<Order> newOrders = new List<Order>();
            foreach(var ordd in newOrdersAll)
            {
                if (notificatonshowns.Contains(ordd.OrderId))
                    continue;
                var customer = dbcustomers.Where(x => x.CustomerId == ordd.Customer.CustomerId).First();
                Pincode orderpincode = pincodes.Where(x => x.PincodeId == customer.Pincode.PincodeId).First();
                Pincode profpincode = pincodes.Where(x => x.PincodeId == professional.Pincode.PincodeId).First();
                var distance = GetDistanceBetweenPoints(orderpincode.Latitude.Value, orderpincode.Longitude.Value, profpincode.Latitude.Value, profpincode.Longitude.Value);
                if (Math.Round(distance) <= 5)
                {
                    newOrders.Add(ordd);
                }
            }
            
            List<OrderViewModel> orderViews = new List<OrderViewModel>();
            
            foreach(var objs in isapprovedButNotAccepted)
            {
                OrderViewModel order = new OrderViewModel();
                order.OrderId = objs.OrderId;
                order.CustomerName = objs.Customer.CustomerName;
                order.CustomerPhoneNumber = objs.Customer.MobileNo;
                order.CustomerAddress = objs.Customer.Address;
                order.Service = subcategories.Where(x => x.CategoryId == objs.Order.SubCategoryId).First().CategoryName;
                order.AppointmentDate = objs.SelectedDate.Value.ToString("dd-MMM-yyyy") + " " + objs.SelectedTime;
                orderViews.Add(order);
                
            }
            foreach (var objs in newOrders)
            {
                OrderViewModel order = new OrderViewModel();
                order.OrderId = objs.OrderId;
                order.CustomerName = objs.Customer.CustomerName;
                order.Service = subcategories.Where(x => x.CategoryId == objs.SubCategoryId).First().CategoryName;
                orderViews.Add(order);

            }

            orderViews = orderViews.OrderByDescending(x => x.OrderId).ToList();
            string outputdata = string.Empty;
            foreach(var objs in orderViews)
            {
                if (string.IsNullOrEmpty(objs.CustomerPhoneNumber))
                {
                    outputdata = outputdata + " <li><a href=\"../../Professional/Orders/SelectNotify/"+ objs.OrderId.ToString() +"\">" +
                        "<small>"+ objs.Service +"</small>" +
                        "<strong><em>"+ objs.CustomerName +"</em></strong>"+
                        "</a></li> ";
                }
                else
                {
                    outputdata = outputdata + " <li><a href=\"../../Professional/Orders/SelectNotify/" + objs.OrderId.ToString() + "\">" +
                        "<small>" + objs.Service + "</small>" +
                        "<strong><em>" + objs.CustomerName + "-" + objs.CustomerPhoneNumber + "</em></strong><br/>" +
                        "<small>"+ objs.CustomerAddress +"</small>" +
                        "</a></li> ";
                }
            }

            if(orderViews.Count == 0)
            {
                outputdata = " <li><a href=\"#\">" +
                        "<strong>No Notification Found</strong>" +
                        "</a></li> ";
            }

            NotificationModel notification = new NotificationModel();
            notification.Notification = outputdata;
            notification.count = orderViews.Count;

            return Json(notification, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SelectNotify(string id)
        {
            int orderid = Convert.ToInt32(id);
            var userobj = db.Users.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).First();
            var professional = db.PartnerProfessionals.ToList().Where(x => x.UserId == userobj.UserId).First();
            var orderrequest = db.OrderRequests.Where(x => x.Order.OrderId == orderid && x.PartnerProfessional.PartnerProfessionalId == professional.PartnerProfessionalId).FirstOrDefault();

            if (orderrequest != null)
            {
                orderrequest.NotifyStatus = 1;
                db.Entry(orderrequest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ApprovedAppointments", "Orders", new { Area = "Professional" });
                //return View("ApprovedAppointments");
                
            }
            else
            {
                IsNotificationShown shown = new IsNotificationShown();
                shown.PartnerProfessional = professional;
                shown.OrderId = orderid;
                db.IsNotificationShowns.Add(shown);
                db.SaveChanges();
                return RedirectToAction("NewAppointments", "Orders", new {  Area = "Professional" });
            }
            
        }



        public List<Order> GetOrders()
        {
            using (HomeServicesEntities db = new HomeServicesEntities())
            {
                var ordersids = db.Orders.Include(o => o.Customer).Include(o => o.PaymentMethod).Include(o => o.Pincode).ToList();
                var orderprof = db.OrderWithProfessionals.ToList();
                var neworders = ordersids.Select(x => x.OrderId).Except(orderprof.Select(x => x.OrderId)).ToList();
                var orders = (from ord in ordersids
                              join nod in neworders on ord.OrderId equals nod
                              select ord).ToList();
                return orders;
            }
        }


        public ActionResult SaveChangeStatus(string ChangeStatus, string OrderId)
        {
            var order = db.Orders.ToList().Where(x => x.OrderId == Convert.ToInt32(OrderId)).FirstOrDefault();
            if(order != null)
            {
                if(ChangeStatus == "1")
                {
                    order.IsGoingOn = true;
                    order.IsCancelled = false;
                    order.IsDelivered = false;
                }
                else if(ChangeStatus == "2")
                {
                    order.IsGoingOn = false;
                    order.IsDelivered = true;
                    order.IsCancelled = false;
                    db.Entry(order).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Download", new { id = Convert.ToInt32(OrderId) });
                }
                else if(ChangeStatus == "3")
                {
                    order.IsGoingOn = false;
                    order.IsDelivered = false;
                    order.IsCancelled = true;
                }
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
            }
            return View("Index");
        }

        public ActionResult NewAppointments()
        {
            //var orders = db.Orders.Where(x => !x.IsGoingOn.Value && !x.IsDelivered.Value && !x.IsCancelled.Value && x.IsActive.Value).Include(o => o.Customer).Include(o => o.PaymentMethod).Include(o => o.Pincode);
            return View();
        }

        public ActionResult Accepted()
        {
            //var orders = db.Orders.Where(x => !x.IsGoingOn.Value && !x.IsDelivered.Value && !x.IsCancelled.Value && x.IsActive.Value).Include(o => o.Customer).Include(o => o.PaymentMethod).Include(o => o.Pincode);
            return View();
        }

        public ActionResult ApprovedAppointments()
        {
            return View();
        }

        public ActionResult CancelledAppointments()
        {
            //var orders = db.Orders.Where(x => !x.IsGoingOn.Value && !x.IsDelivered.Value && !x.IsCancelled.Value && x.IsActive.Value).Include(o => o.Customer).Include(o => o.PaymentMethod).Include(o => o.Pincode);
            return View();
        }

        public ActionResult CompletedAppointments()
        {
            //var orders = db.Orders.Where(x => !x.IsGoingOn.Value && x.IsDelivered.Value && !x.IsCancelled.Value).Include(o => o.Customer).Include(o => o.PaymentMethod).Include(o => o.Pincode);
            return View();
        }

        // GET: Professional/Orders/Details/5
        public ActionResult Details(int id)
        {
            var orders = db.Orders.Include(o => o.Customer).Include(o => o.PaymentMethod).Include(o => o.Pincode).ToList().Where(x => x.OrderId == id).First();
            var userobj = db.Users.ToList().Where(x => x.UserId == Convert.ToInt32(User.Identity.Name)).First();
            var partnerprof = db.PartnerProfessionals.ToList().Where(x => x.UserId == userobj.UserId).First();
            OrderRequest request = db.OrderRequests.Where(x => x.OrderId == orders.OrderId && x.PartnerProfessional.PartnerProfessionalId == partnerprof.PartnerProfessionalId).First();

            OrderViewModel model = new OrderViewModel();
                
            var subcats = db.Categories.ToList().Where(x => x.CategoryId == orders.SubCategoryId).First();
            model.Service = subcats.CategoryName;
            model.Amount = ""; // subcats.Price.Value.ToString();
            model.CustomerName = orders.Customer.CustomerName;
            model.AppointmentDate = request.SelectedDate.Value.ToString("dd-MMM-yyyy") + " " + request.SelectedTime;

            if (orders.IsGoingOn.HasValue && orders.IsGoingOn.Value == true)
            {
                model.Status = "Ongoing";
            }
            else if (orders.IsCancelled.HasValue && orders.IsCancelled.Value == true)
                model.Status = "Cancelled";
            else if (orders.IsDelivered.HasValue && orders.IsDelivered.Value == true)
                model.Status = "Completed";
            else
                model.Status = "New";
            
            model.Area = orders.Customer.Pincode.Pincode1;
                
            

            return View(model);
        }

        // GET: Professional/Orders/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustomerName");
            ViewBag.PaymentId = new SelectList(db.PaymentMethods, "Paymentid", "PaymentType");
            ViewBag.GuestPincodeId = new SelectList(db.Pincodes, "PincodeId", "Pincode1");
            return View();
        }

        // POST: Professional/Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderId,SubCategoryId,OrderDate,OrderTime,CustomerId,GuestName,GuestPhone,GuestEmail,GuestPincodeId,GuestCityId,IsStarted,IsDelivered,IsPaymentDone,DeliveredDateTime,IsCancelled,Rating,Description,OrderPlacedOn,IsActive,IsGoingOn,PaymentId")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustomerName", order.CustomerId);
            ViewBag.PaymentId = new SelectList(db.PaymentMethods, "Paymentid", "PaymentType", order.PaymentId);
            ViewBag.GuestPincodeId = new SelectList(db.Pincodes, "PincodeId", "Pincode1", order.GuestPincodeId);
            return View(order);
        }

        // GET: Professional/Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustomerName", order.CustomerId);
            ViewBag.PaymentId = new SelectList(db.PaymentMethods, "Paymentid", "PaymentType", order.PaymentId);
            ViewBag.GuestPincodeId = new SelectList(db.Pincodes, "PincodeId", "Pincode1", order.GuestPincodeId);
            return View(order);
        }

        // POST: Professional/Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderId,SubCategoryId,OrderDate,OrderTime,CustomerId,GuestName,GuestPhone,GuestEmail,GuestPincodeId,GuestCityId,IsStarted,IsDelivered,IsPaymentDone,DeliveredDateTime,IsCancelled,Rating,Description,OrderPlacedOn,IsActive,IsGoingOn,PaymentId")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "CustomerName", order.CustomerId);
            ViewBag.PaymentId = new SelectList(db.PaymentMethods, "Paymentid", "PaymentType", order.PaymentId);
            ViewBag.GuestPincodeId = new SelectList(db.Pincodes, "PincodeId", "Pincode1", order.GuestPincodeId);
            return View(order);
        }

        public ActionResult ViewRating(int id)
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
            return View(order);
        }

        // GET: Professional/Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Professional/Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public double GetDistanceBetweenPoints(decimal lat1param, decimal long1param, decimal lat2param, decimal long2param)
        {
            double lat1 = Convert.ToDouble(lat1param);
            double long1 = Convert.ToDouble(long1param);
            double lat2 = Convert.ToDouble(lat2param);
            double long2 = Convert.ToDouble(long2param);

            double distance = 0;

            double dLat = (lat2 - lat1) / 180 * Math.PI;
            double dLong = (long2 - long1) / 180 * Math.PI;

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2)
                        + Math.Cos(lat2) * Math.Sin(dLong / 2) * Math.Sin(dLong / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            //Calculate radius of earth
            // For this you can assume any of the two points.
            double radiusE = 6378135; // Equatorial radius, in metres
            double radiusP = 6356750; // Polar Radius

            //Numerator part of function
            double nr = Math.Pow(radiusE * radiusP * Math.Cos(lat1 / 180 * Math.PI), 2);
            //Denominator part of the function
            double dr = Math.Pow(radiusE * Math.Cos(lat1 / 180 * Math.PI), 2)
                            + Math.Pow(radiusP * Math.Sin(lat1 / 180 * Math.PI), 2);
            double radius = Math.Sqrt(nr / dr);

            //Calaculate distance in metres.
            distance = radius * c;
            return distance;
        }

        public ActionResult Download(int id)
        {
            OrderViewModel model = new OrderViewModel();
            Order order = db.Orders.ToList().Where(x => x.OrderId == id).First();
            Customer customer = db.Customers.ToList().Where(x => x.CustomerId == order.CustomerId).First();
            Category sub = db.Categories.ToList().Where(x => x.CategoryId == order.SubCategoryId).First();
            OrderRequest request = db.OrderRequests.ToList().Where(x => x.OrderId == order.OrderId && x.IsApproved.Value && x.NotifyStatus.Value == 1).First();

            model.OrderId = order.OrderId;
            model.CustomerName = customer.CustomerName;
            model.CustomerPhoneNumber = customer.MobileNo;
            model.CustomerAddress = customer.Address;
            model.Amount = ""; // sub.Price.Value.ToString();
            model.AppointmentDate = request.SelectedDate.Value.ToString() + " " + request.SelectedTime;
            model.PlacedOn = order.OrderPlacedOn.Value.ToString("dd-MMM-yyyy HH:mm");
            model.Service = sub.CategoryName;
            

            var html = ToHtml("PrintableInvoices", new ViewDataDictionary(model), ControllerContext);
            var fileName = BuildInvoiceFileName(model, "pdf");

            return BuildFileStream(html, fileName, MediaTypeNames.Application.Pdf, false);

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
            return string.Format("Invoice_{0}_{1}.{2}", "", orderViewModel.OrderId, fileExtension);
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
