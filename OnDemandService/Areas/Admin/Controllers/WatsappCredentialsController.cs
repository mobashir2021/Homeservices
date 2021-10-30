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
    public class WatsappCredentialsController : Controller
    {
        private HomeServicesEntities db = new HomeServicesEntities();

        // GET: Admin/WatsappCredentials
        public ActionResult Index()
        {
            return View(db.WatsappCredentials.ToList());
        }

        // GET: Admin/WatsappCredentials/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WatsappCredential watsappCredential = db.WatsappCredentials.Find(id);
            if (watsappCredential == null)
            {
                return HttpNotFound();
            }
            return View(watsappCredential);
        }

        // GET: Admin/WatsappCredentials/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/WatsappCredentials/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WatsappCredentialsId,WatsappUsername,WatsappPassword")] WatsappCredential watsappCredential)
        {
            var result = db.WatsappCredentials;
            foreach (var data in result)
            {
                db.WatsappCredentials.Remove(data);
                db.SaveChanges();
            }
            if (ModelState.IsValid)
            {
                db.WatsappCredentials.Add(watsappCredential);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(watsappCredential);
        }

        // GET: Admin/WatsappCredentials/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WatsappCredential watsappCredential = db.WatsappCredentials.Find(id);
            if (watsappCredential == null)
            {
                return HttpNotFound();
            }
            return View(watsappCredential);
        }

        // POST: Admin/WatsappCredentials/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WatsappCredentialsId,WatsappUsername,WatsappPassword")] WatsappCredential watsappCredential)
        {
            if (ModelState.IsValid)
            {
                db.Entry(watsappCredential).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(watsappCredential);
        }

        // GET: Admin/WatsappCredentials/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WatsappCredential watsappCredential = db.WatsappCredentials.Find(id);
            if (watsappCredential == null)
            {
                return HttpNotFound();
            }
            return View(watsappCredential);
        }

        // POST: Admin/WatsappCredentials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WatsappCredential watsappCredential = db.WatsappCredentials.Find(id);
            db.WatsappCredentials.Remove(watsappCredential);
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
