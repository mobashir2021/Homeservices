using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnDemandService.Models
{
    public class DatabaseOperations
    {
        HomeServicesEntities db = new HomeServicesEntities();

        //public IEnumerable<Order> GetLeads(string Status)
        //{
        //    List<Order> lst = db.Orders.ToList();
        //    if()

        //    return lst;
        //}

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

        public List<LeadGenerationViewModel> GetLeadsViewModel(string Status)
        {
            List<Order> lst = db.Orders.ToList().ToList();
            
            List<Order> lstFinal = new List<Order>();
            Dictionary<int, List<OrderRequest>> dctOrderReq = new Dictionary<int, List<OrderRequest>>();
            Dictionary<int, string> tempPincode = db.Pincodes.ToDictionary(x => x.PincodeId, x => x.Pincode1);
            
            
            var orderrequest = db.OrderRequests.ToList();
            if (Status == "New")
            {
                foreach (var data in lst)
                {
                    var newresult = orderrequest.Where(x => x.OrderId == data.OrderId && x.IsApproved.HasValue && x.IsApproved.Value).ToList();
                    if (newresult.Count == 0)
                    {
                        lstFinal.Add(data);
                        dctOrderReq.Add(data.OrderId, newresult);
                    }

                }
            }
            if (Status == "Ongoing")
            {
                foreach (var data in lst)
                {
                    var newresult = orderrequest.Where(x => x.OrderId == data.OrderId && x.IsApproved.HasValue && x.IsApproved.Value && x.Status == "Accepted").ToList();
                    if (newresult.Count == 1 && data.IsGoingOn.HasValue && data.IsGoingOn.Value)
                    {
                        lstFinal.Add(data);
                        dctOrderReq.Add(data.OrderId, newresult);
                    }

                }
            }
            if (Status == "Completed")
            {
                foreach (var data in lst)
                {
                    var newresult = orderrequest.Where(x => x.OrderId == data.OrderId && x.IsApproved.HasValue && x.IsApproved.Value && x.Status == "Accepted").ToList();
                    if (newresult.Count == 1 && data.IsDelivered.HasValue == false && data.IsDelivered.Value)
                    {
                        lstFinal.Add(data);
                        dctOrderReq.Add(data.OrderId, newresult);
                    }

                }
            }
            if (Status == "Cancelled")
            {
                foreach (var data in lst)
                {
                    var newresult = orderrequest.Where(x => x.OrderId == data.OrderId && x.IsApproved.HasValue && x.IsApproved.Value && x.Status == "Accepted").ToList();
                    if (newresult.Count == 1 && data.IsCancelled.HasValue == false && data.IsCancelled.Value)
                    {
                        lstFinal.Add(data);
                        dctOrderReq.Add(data.OrderId, newresult);
                    }

                }
            }
            List<LeadGenerationViewModel> viewModels = new List<LeadGenerationViewModel>();
            Dictionary<int, Customer> dctcust = db.Customers.ToDictionary(x => x.CustomerId, x => x);
            foreach (var value in lstFinal)
            {
                LeadGenerationViewModel lead = new LeadGenerationViewModel();
                
                lead.Leadgenerationid = value.OrderId;
                Customer cust = dctcust[value.CustomerId.Value];
                lead.CustomerId = cust.CustomerId;
                lead.Customername = cust.CustomerName;
                lead.Customermobileno = cust.MobileNo;
                lead.Customeraddress = cust.Address;
                lead.City = db.Cities.Where(x => x.CityId == cust.Cityid).First().CityName;
                lead.Orderdatetime = Convert.ToDateTime(value.OrderDate.ToString("dd-MMM-yyyy") + " " + value.OrderTime);
                lead.ServiceName = db.Categories.Where(x => x.CategoryId == value.SubCategoryId).First().CategoryName;
                lead.Serviceprovided = value.SubCategoryId;
                if(Status != "New")
                {
                    List<OrderRequest> temp = dctOrderReq[value.OrderId];
                    OrderRequest orderRequest = temp.Where(x => x.IsApproved.HasValue && x.IsApproved.Value).First();
                    lead.AppointedDate = orderRequest.SelectedDate.Value.ToString("dd-MMM-yyyy") + " " + orderRequest.SelectedTime;
                }
                    
                lead.Pincode = Convert.ToInt32(tempPincode[cust.PincodeId.Value]);
                lead.LeadStatus = Status;
                lead.Orderdesc = value.Description;
                
                viewModels.Add(lead);
            }
            return viewModels;
        }

        public List<LeadGenerationViewModel> GetLeadsViewModelPartner(string Status, int Partnerid, List<Order> orderlist = null)
        {
            List<Order> lst = new List<Order>();
            if (orderlist != null)
            {
                lst = orderlist;
            }
            else
                lst = db.Orders.ToList();

            List<Order> lstFinal = new List<Order>();
            Dictionary<int, List<OrderRequest>> dctOrderReq = new Dictionary<int, List<OrderRequest>>();
            Dictionary<int, string> tempPincode = db.Pincodes.ToDictionary(x => x.PincodeId, x => x.Pincode1);


            var orderrequest = db.OrderRequests.ToList();
            if (Status == "New")
            {
                foreach (var data in lst)
                {
                    //var newresult = orderrequest.Where(x => x.OrderId == data.OrderId && x.IsApproved.HasValue && x.IsApproved.Value).ToList();
                    var newresult = orderrequest.Where(x => x.OrderId == data.OrderId).ToList();
                    //if (newresult.Count == 0)
                    //{
                        lstFinal.Add(data);
                        dctOrderReq.Add(data.OrderId, newresult);
                    //}

                }
            }
            if (Status == "Ongoing")
            {
                foreach (var data in lst)
                {
                    var newresult = orderrequest.Where(x => x.OrderId == data.OrderId && x.IsApproved.HasValue && x.IsApproved.Value && x.Status == "Accepted" && x.Partnerid.Value == Partnerid).ToList();
                    if (newresult.Count == 1 && data.IsGoingOn.HasValue && data.IsGoingOn.Value)
                    {
                        lstFinal.Add(data);
                        dctOrderReq.Add(data.OrderId, newresult);
                    }

                }
            }
            if (Status == "Completed")
            {
                foreach (var data in lst)
                {
                    var newresult = orderrequest.Where(x => x.OrderId == data.OrderId && x.IsApproved.HasValue && x.IsApproved.Value && x.Status == "Accepted" && x.Partnerid.Value == Partnerid).ToList();
                    if (newresult.Count == 1 && data.IsDelivered.HasValue && data.IsDelivered.Value)
                    {
                        lstFinal.Add(data);
                        dctOrderReq.Add(data.OrderId, newresult);
                    }

                }
            }
            if (Status == "Cancelled")
            {
                foreach (var data in lst)
                {
                    var newresult = orderrequest.Where(x => x.OrderId == data.OrderId && x.IsApproved.HasValue && x.IsApproved.Value && x.Status == "Accepted" && x.Partnerid.Value == Partnerid).ToList();
                    if (newresult.Count == 1 && data.IsCancelled.HasValue == false && data.IsCancelled.Value)
                    {
                        lstFinal.Add(data);
                        dctOrderReq.Add(data.OrderId, newresult);
                    }

                }
            }
            List<LeadGenerationViewModel> viewModels = new List<LeadGenerationViewModel>();
            Dictionary<int, Customer> dctcust = db.Customers.ToDictionary(x => x.CustomerId, x => x);
            foreach (var value in lstFinal)
            {
                LeadGenerationViewModel lead = new LeadGenerationViewModel();

                lead.Leadgenerationid = value.OrderId;
                if (value.CustomerId.HasValue && dctcust.ContainsKey(value.CustomerId.Value))
                {
                    Customer cust = dctcust[value.CustomerId.Value];
                    lead.CustomerId = cust.CustomerId;
                    lead.Customername = cust.CustomerName;
                    lead.Customermobileno = cust.MobileNo;
                    lead.Customeraddress = cust.Address;
                    City city = db.Cities.Where(x => x.CityId == cust.Cityid).FirstOrDefault();
                    if (city != null)
                    {
                        lead.City = city.CityName;
                    }
                    lead.Orderdatetime = Convert.ToDateTime(value.OrderDate.ToString("dd-MMM-yyyy") + " " + value.OrderTime);
                    lead.ServiceName = db.Categories.Where(x => x.CategoryId == value.SubCategoryId).First().CategoryName;
                    lead.Serviceprovided = value.SubCategoryId;
                    if (Status != "New")
                    {
                        List<OrderRequest> temp = dctOrderReq[value.OrderId];
                        OrderRequest orderRequest = temp.Where(x => x.IsApproved.HasValue && x.IsApproved.Value).First();
                        lead.AppointedDate = orderRequest.SelectedDate.Value.ToString("dd-MMM-yyyy") + " " + orderRequest.SelectedTime;
                    }

                    lead.Pincode = Convert.ToInt32(tempPincode[cust.PincodeId.Value]);
                    lead.LeadStatus = Status;
                    lead.Orderdesc = value.Description;

                    viewModels.Add(lead);
                }
            }
            return viewModels;
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

        public List<LeadGenerationViewModel> GetLeadsViewModelCustomer(string Status, int Customerid)
        {
            List<Order> lst = db.Orders.Where(x => x.CustomerId == Customerid).ToList();

            List<Order> lstFinal = new List<Order>();
            Dictionary<int, List<OrderRequest>> dctOrderReq = new Dictionary<int, List<OrderRequest>>();
            Dictionary<int, string> tempPincode = db.Pincodes.ToDictionary(x => x.PincodeId, x => x.Pincode1);


            var orderrequest = db.OrderRequests.ToList();
            if (Status == "New")
            {
                foreach (var data in lst)
                {
                    var newresult = orderrequest.Where(x => x.OrderId == data.OrderId && x.IsApproved.HasValue && x.IsApproved.Value).ToList();
                    if (newresult.Count == 0)
                    {
                        lstFinal.Add(data);
                        dctOrderReq.Add(data.OrderId, newresult);
                    }

                }
            }
            if (Status == "Ongoing")
            {
                foreach (var data in lst)
                {
                    var newresult = orderrequest.Where(x => x.OrderId == data.OrderId && x.IsApproved.HasValue && x.IsApproved.Value && x.Status == "Accepted" ).ToList();
                    if (newresult.Count == 1 && data.IsGoingOn.HasValue && data.IsGoingOn.Value)
                    {
                        lstFinal.Add(data);
                        dctOrderReq.Add(data.OrderId, newresult);
                    }

                }
            }
            if (Status == "Completed")
            {
                foreach (var data in lst)
                {
                    var newresult = orderrequest.Where(x => x.OrderId == data.OrderId && x.IsApproved.HasValue && x.IsApproved.Value && x.Status == "Accepted" ).ToList();
                    if (newresult.Count == 1 && data.IsDelivered.HasValue && data.IsDelivered.Value)
                    {
                        lstFinal.Add(data);
                        dctOrderReq.Add(data.OrderId, newresult);
                    }

                }
            }
            if (Status == "Cancelled")
            {
                foreach (var data in lst)
                {
                    var newresult = orderrequest.Where(x => x.OrderId == data.OrderId && x.IsApproved.HasValue && x.IsApproved.Value && x.Status == "Accepted").ToList();
                    if (newresult.Count == 1 && data.IsCancelled.HasValue == false && data.IsCancelled.Value)
                    {
                        lstFinal.Add(data);
                        dctOrderReq.Add(data.OrderId, newresult);
                    }

                }
            }
            List<LeadGenerationViewModel> viewModels = new List<LeadGenerationViewModel>();
            Dictionary<int, PartnerProfessional> dctcust = db.PartnerProfessionals.ToDictionary(x => x.PartnerProfessionalId, x => x);
            Dictionary<int, OrderWithProfessional> dctOrderprof = db.OrderWithProfessionals.ToDictionary(x => x.OrderId, x => x);
            foreach (var value in lstFinal)
            {
                LeadGenerationViewModel lead = new LeadGenerationViewModel();

                lead.Leadgenerationid = value.OrderId;
                if (dctOrderprof.ContainsKey(value.OrderId))
                {
                    int professonalid = dctOrderprof[value.OrderId].ProfessionalId;
                    PartnerProfessional cust = dctcust[professonalid];
                    lead.PartnerId = cust.PartnerProfessionalId;
                    lead.Customername = cust.PartnerName;
                    lead.Customermobileno = cust.MobileNo;
                    lead.Customeraddress = cust.Address;
                    City city = db.Cities.Where(x => x.CityId == cust.Cityid).FirstOrDefault();
                    if(city != null)
                    {
                        lead.City = city.CityName;
                    }
                    
                    lead.Orderdatetime = Convert.ToDateTime(value.OrderDate.ToString("dd-MMM-yyyy") + " " + value.OrderTime);
                    lead.ServiceName = db.Categories.Where(x => x.CategoryId == value.SubCategoryId).First().CategoryName;
                    lead.Serviceprovided = value.SubCategoryId;
                    if (Status != "New")
                    {
                        List<OrderRequest> temp = dctOrderReq[value.OrderId];
                        OrderRequest orderRequest = temp.Where(x => x.IsApproved.HasValue && x.IsApproved.Value).First();
                        lead.AppointedDate = orderRequest.SelectedDate.Value.ToString("dd-MMM-yyyy") + " " + orderRequest.SelectedTime;
                    }

                    lead.Pincode = Convert.ToInt32(tempPincode[cust.PincodeId.Value]);
                    lead.LeadStatus = Status;
                    lead.Orderdesc = value.Description;

                    viewModels.Add(lead);
                }
            }
            return viewModels;
        }

        public UserData GetLoginData(string username, string password)
        {
            User user = db.Users.Where(x => x.UserName.ToLower() == username.ToLower() && x.Password == password).FirstOrDefault();

            Role role = db.Roles.Where(x => x.User.UserId == user.UserId).FirstOrDefault();
            Dictionary<int, string> dctPincode = db.Pincodes.ToDictionary(x => x.PincodeId, x => x.Pincode1);
            UserData userData = new UserData();
            if (role.Roles == "Professional")
            {
                PartnerProfessional partner = db.PartnerProfessionals.Where(x => x.UserId == user.UserId).First();
                //SubCategory sub = db.Categories.Where(x => x.SubCategoryId == partner.SubCategoryId).First();
                City city = db.Cities.Where(x => x.CityId == partner.Cityid).FirstOrDefault();
                
                userData.PartnerId = partner.PartnerProfessionalId; userData.Userid = user.UserId; userData.Username = user.UserName; userData.Password = user.Password;
                userData.Pincode = dctPincode[partner.PincodeId.Value]; userData.Address = partner.Address;
                userData.Mobileno = partner.MobileNo;
                if (city != null)
                    userData.City = city.CityName;
                else
                    userData.City = "";
                userData.Service = "";
                userData.Balance = "";// GetBalanceRecharge(userData.PartnerId).ToString();
                userData.TypeLogin = "Professional";
                
            }else if(role.Roles == "Customer")
            {
                Customer partner = db.Customers.Where(x => x.UserId == user.UserId).First();
                //SubCategory sub = db.SubCategories.Where(x => x.SubCategoryId == partner.SubCategoryId).First();
                City city = db.Cities.Where(x => x.CityId == partner.Cityid).FirstOrDefault();
                
                userData.PartnerId = partner.CustomerId; userData.Userid = user.UserId; userData.Username = user.UserName; userData.Password = user.Password;
                userData.Pincode = dctPincode[partner.PincodeId.Value]; userData.Address = partner.Address;
                userData.Mobileno = partner.MobileNo;
                if (city != null)
                    userData.City = city.CityName;
                else
                    userData.City = "";
                userData.Service = "";
                userData.Balance = "";// GetBalanceRecharge(userData.PartnerId).ToString();
                userData.TypeLogin = "Customer";

            }
            return userData;
        }


    }
}