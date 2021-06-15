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
    [Authorize(Roles = "Director,Depart_Director")]
    public class Departs_TasksController : Controller
    {
        private Entities db = new Entities();
        private ApplicationContext db1 = new ApplicationContext();
        // GET: Departs_Tasks
        public ActionResult Index()
        {
            var departs_Tasks = db.Departs_Tasks.Include(d => d.Depart).Include(d => d.Project);
            return View(departs_Tasks.ToList());
        }
        public ActionResult Real()
        {
            var req = new MYProj.Models.ApplicationUser();
            foreach (var item in db1.Users)
            {
                if (item.UserName == User.Identity.Name)
                {
                    req = item;
                }
            }
            var worker = new MYProj.Models.Worker();
            foreach (var item1 in db.Workers)
            {
                if (item1.Почта.Trim() == req.Email)
                {
                    worker = item1;
                }
            }
            var departs_Tasks = db.Departs_Tasks.Include(d => d.Depart).Include(d => d.Project).Where(e=>e.Отдел == worker.Отдел);
            return View(departs_Tasks.ToList());
        }

        // GET: Departs_Tasks/Create
        public ActionResult Create()
        {
            ViewBag.Отдел = new SelectList(db.Departs, "Код_отдела", "Название");
            ViewBag.Проект = new SelectList(db.Projects, "Код_проекта", "Название");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Проект,Отдел,Задача_отделу,Срок_выполнения_задачи,Реальный_срок")] Departs_Tasks departs_Tasks)
        {
            if (ModelState.IsValid)
            {
                db.Departs_Tasks.Add(departs_Tasks);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Отдел = new SelectList(db.Departs, "Код_отдела", "Название", departs_Tasks.Отдел);
            ViewBag.Проект = new SelectList(db.Projects, "Код_проекта", "Название", departs_Tasks.Проект);
            return View(departs_Tasks);
        }

        // GET: Departs_Tasks/Edit/5
        public ActionResult Edit(int pr, int dep)
        {
            var departs_Tasks = db.Departs_Tasks.Where(e => e.Проект == pr && e.Отдел == dep).FirstOrDefault();
            if (departs_Tasks == null)
            {
                return HttpNotFound();
            }
            ViewBag.Отдел = new SelectList(db.Departs, "Код_отдела", "Название", departs_Tasks.Отдел);
            ViewBag.Проект = new SelectList(db.Projects, "Код_проекта", "Название", departs_Tasks.Проект);
            return View(departs_Tasks);
        }

        // POST: Departs_Tasks/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Проект,Отдел,Задача_отделу,Срок_выполнения_задачи,Реальный_срок")] Departs_Tasks departs_Tasks)
        {
            if (ModelState.IsValid)
            {
                db.Entry(departs_Tasks).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Отдел = new SelectList(db.Departs, "Код_отдела", "Название", departs_Tasks.Отдел);
            ViewBag.Проект = new SelectList(db.Projects, "Код_проекта", "Название", departs_Tasks.Проект);
            return View(departs_Tasks);
        }

        // GET: Departs_Tasks/Delete/5
        public ActionResult Delete(int pr, int dep)
        {
            var departs_Tasks = db.Departs_Tasks.Where(e => e.Проект == pr && e.Отдел == dep).FirstOrDefault();
            if (departs_Tasks == null)
            {
                return HttpNotFound();
            }
            return View(departs_Tasks);
        }

        // POST: Departs_Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int pr, int dep)
        {
            var departs_Tasks = db.Departs_Tasks.Where(e => e.Проект == pr && e.Отдел == dep).FirstOrDefault();
            db.Departs_Tasks.Remove(departs_Tasks);
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
