using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MYProj.Models;

namespace MYProj.Controllers
{
    public class DepartsController : Controller
    {
        private Entities db = new Entities();

        // GET: Departs
        public ActionResult Index()
        {
            return View(db.Departs.ToList());
        }


        // GET: Departs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Departs/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Код_отдела,Название,Краткое_описание")] Depart depart)
        {
            if (ModelState.IsValid)
            {
                db.Departs.Add(depart);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(depart);
        }

        // GET: Departs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Depart depart = db.Departs.Find(id);
            if (depart == null)
            {
                return HttpNotFound();
            }
            return View(depart);
        }

        // POST: Departs/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Код_отдела,Название,Краткое_описание")] Depart depart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(depart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(depart);
        }

        // GET: Departs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Depart depart = db.Departs.Find(id);
            if (depart == null)
            {
                return HttpNotFound();
            }
            return View(depart);
        }

        // POST: Departs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Depart depart = db.Departs.Find(id);
            db.Departs.Remove(depart);
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
