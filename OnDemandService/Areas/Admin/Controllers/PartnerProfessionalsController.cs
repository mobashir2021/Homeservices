using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnDemandService.Models;

namespace OnDemandService.Areas.Admin.Controllers
{
    public class PartnerProfessionalsController : Controller
    {
        private HomeServicesEntities db = new HomeServicesEntities();

        // GET: PartnerProfessionals
        public ActionResult Index()
        {
            var partnerProfessionals = db.PartnerProfessionals.Include(p => p.Pincode).Include(p => p.City).Include(p => p.SubCategory);
            return View(partnerProfessionals.ToList());
        }

        // GET: PartnerProfessionals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PartnerProfessional partnerProfessional = db.PartnerProfessionals.Find(id);
            if (partnerProfessional == null)
            {
                return HttpNotFound();
            }
            return View(partnerProfessional);
        }

        // GET: PartnerProfessionals/Create
        public ActionResult Create()
        {
            ViewBag.PartnerProfessionalId = new SelectList(db.OrderWithProfessionals, "OrderProfessionalId", "Remarks");
            ViewBag.PincodeId = new SelectList(db.Pincodes, "PincodeId", "Pincode1");
            ViewBag.Cityid = new SelectList(db.Cities, "CityId", "CityName");
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "SubCategoryName");
            return View();
        }

        // POST: PartnerProfessionals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PartnerProfessionalId,PartnerName,UserId,MobileNo,Gender,Address,PincodeId,Cityid,Ratings,TotalIncome,SubCategoryId")] PartnerProfessional partnerProfessional)
        {
            if (ModelState.IsValid)
            {
                db.PartnerProfessionals.Add(partnerProfessional);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PartnerProfessionalId = new SelectList(db.OrderWithProfessionals, "OrderProfessionalId", "Remarks", partnerProfessional.PartnerProfessionalId);
            ViewBag.PincodeId = new SelectList(db.Pincodes, "PincodeId", "Pincode1", partnerProfessional.PincodeId);
            ViewBag.Cityid = new SelectList(db.Cities, "CityId", "CityName", partnerProfessional.Cityid);
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "SubCategoryName", partnerProfessional.SubCategoryId);
            return View(partnerProfessional);
        }

        // GET: PartnerProfessionals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PartnerProfessional partnerProfessional = db.PartnerProfessionals.Find(id);
            if (partnerProfessional == null)
            {
                return HttpNotFound();
            }
            ViewBag.PartnerProfessionalId = new SelectList(db.OrderWithProfessionals, "OrderProfessionalId", "Remarks", partnerProfessional.PartnerProfessionalId);
            ViewBag.PincodeId = new SelectList(db.Pincodes, "PincodeId", "Pincode1", partnerProfessional.PincodeId);
            ViewBag.Cityid = new SelectList(db.Cities, "CityId", "CityName", partnerProfessional.Cityid);
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "SubCategoryName", partnerProfessional.SubCategoryId);
            return View(partnerProfessional);
        }

        // POST: PartnerProfessionals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PartnerProfessionalId,PartnerName,UserId,MobileNo,Gender,Address,PincodeId,Cityid,Ratings,TotalIncome,SubCategoryId")] PartnerProfessional partnerProfessional)
        {
            if (ModelState.IsValid)
            {
                db.Entry(partnerProfessional).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PartnerProfessionalId = new SelectList(db.OrderWithProfessionals, "OrderProfessionalId", "Remarks", partnerProfessional.PartnerProfessionalId);
            ViewBag.PincodeId = new SelectList(db.Pincodes, "PincodeId", "Pincode1", partnerProfessional.PincodeId);
            ViewBag.Cityid = new SelectList(db.Cities, "CityId", "CityName", partnerProfessional.Cityid);
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "SubCategoryName", partnerProfessional.SubCategoryId);
            return View(partnerProfessional);
        }

        // GET: PartnerProfessionals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PartnerProfessional partnerProfessional = db.PartnerProfessionals.Find(id);
            if (partnerProfessional == null)
            {
                return HttpNotFound();
            }
            return View(partnerProfessional);
        }

        // POST: PartnerProfessionals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PartnerProfessional partnerProfessional = db.PartnerProfessionals.Find(id);
            db.PartnerProfessionals.Remove(partnerProfessional);
            db.SaveChanges();
            return RedirectToAction("Index");
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
