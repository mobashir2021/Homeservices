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
    public class PreferOTPsController : Controller
    {
        private HomeServicesEntities db = new HomeServicesEntities();

        // GET: Admin/PreferOTPs
        public ActionResult Index()
        {
            return View(db.PreferOTPs.ToList());
        }

        // GET: Admin/PreferOTPs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PreferOTP preferOTP = db.PreferOTPs.Find(id);
            if (preferOTP == null)
            {
                return HttpNotFound();
            }
            return View(preferOTP);
        }

        // GET: Admin/PreferOTPs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/PreferOTPs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PreferOTPId,Watsapp,SMS")] PreferOTP preferOTP, object Watsapp)
        {
            var otps = db.PreferOTPs;
            foreach(PreferOTP temp in otps)
            {
                db.PreferOTPs.Remove(temp);
                db.SaveChanges();
            }
            string[] data = (string[])Watsapp;
            if (preferOTP == null)
                preferOTP = new PreferOTP();
            if(data[0] == "Watsapp")
            {
                preferOTP.Watsapp = true;
                preferOTP.SMS = false;
            }else if (data[0] == "SMS")
            {
                preferOTP.Watsapp = false;
                preferOTP.SMS = true;
            }
            
                db.PreferOTPs.Add(preferOTP);
                db.SaveChanges();
                return RedirectToAction("Index");
            
        }

        // GET: Admin/PreferOTPs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PreferOTP preferOTP = db.PreferOTPs.Find(id);
            if (preferOTP == null)
            {
                return HttpNotFound();
            }
            return View(preferOTP);
        }

        // POST: Admin/PreferOTPs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PreferOTPId,Watsapp,SMS")] PreferOTP preferOTP, object Watsapp)
        {
            string[] data = (string[])Watsapp;
            if (data[0] == "Watsapp")
            {
                preferOTP.Watsapp = true;
                preferOTP.SMS = false;
            }
            else if (data[0] == "SMS")
            {
                preferOTP.Watsapp = false;
                preferOTP.SMS = true;
            }
            if (ModelState.IsValid)
            {
                db.Entry(preferOTP).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(preferOTP);
        }

        // GET: Admin/PreferOTPs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PreferOTP preferOTP = db.PreferOTPs.Find(id);
            if (preferOTP == null)
            {
                return HttpNotFound();
            }
            return View(preferOTP);
        }

        // POST: Admin/PreferOTPs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PreferOTP preferOTP = db.PreferOTPs.Find(id);
            db.PreferOTPs.Remove(preferOTP);
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
