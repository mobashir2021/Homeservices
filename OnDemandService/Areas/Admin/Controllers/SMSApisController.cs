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
    public class SMSApisController : Controller
    {
        private HomeServicesEntities db = new HomeServicesEntities();

        // GET: Admin/SMSApis
        public ActionResult Index()
        {
            return View(db.SMSApis.ToList());
        }

        // GET: Admin/SMSApis/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SMSApi sMSApi = db.SMSApis.Find(id);
            if (sMSApi == null)
            {
                return HttpNotFound();
            }
            return View(sMSApi);
        }

        // GET: Admin/SMSApis/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/SMSApis/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SMSApiId,SMSUrl,SMSApiKey,Sender")] SMSApi sMSApi)
        {
            var result = db.SMSApis;
            foreach (var data in result)
            {
                db.SMSApis.Remove(data);
                db.SaveChanges();
            }
            if (ModelState.IsValid)
            {
                db.SMSApis.Add(sMSApi);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sMSApi);
        }

        // GET: Admin/SMSApis/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SMSApi sMSApi = db.SMSApis.Find(id);
            if (sMSApi == null)
            {
                return HttpNotFound();
            }
            return View(sMSApi);
        }

        // POST: Admin/SMSApis/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SMSApiId,SMSUrl,SMSApiKey,Sender")] SMSApi sMSApi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sMSApi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sMSApi);
        }

        // GET: Admin/SMSApis/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SMSApi sMSApi = db.SMSApis.Find(id);
            if (sMSApi == null)
            {
                return HttpNotFound();
            }
            return View(sMSApi);
        }

        // POST: Admin/SMSApis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SMSApi sMSApi = db.SMSApis.Find(id);
            db.SMSApis.Remove(sMSApi);
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
