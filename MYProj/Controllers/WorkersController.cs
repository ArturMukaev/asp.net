using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MYProj.Models;

namespace MYProj.Controllers
{
    [Authorize(Roles = "HR")]
    public class WorkersController : Controller
    {
        private Entities db = new Entities();

        // GET: Workers
        public async Task<ActionResult> Index(string searchString)
        {
            var workers = db.Workers.Include(w => w.Depart).Include(w => w.Gender).Include(w => w.Role);
            if (!String.IsNullOrEmpty(searchString))
            {
                workers = workers.Where(s => s.Фамилия.Contains(searchString));
            }
            return View( await workers.ToListAsync());
        }

        // GET: Workers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Worker worker = db.Workers.Find(id);
            if (worker == null)
            {
                return HttpNotFound();
            }
            return View(worker);
        }

        // GET: Workers/Create
        public ActionResult Create()
        {
            ViewBag.Отдел = new SelectList(db.Departs, "Код_отдела", "Название");
            ViewBag.Пол = new SelectList(db.Genders, "Код_пола", "Пол");
            ViewBag.Роль = new SelectList(db.Roles, "Код_роли", "Должность");
            return View();
        }

        // POST: Workers/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Код_сотрудника,Отдел,Фамилия,Имя,Отчество,Дата_рождения,Телефон,Почта,Пол,Роль")] Worker worker)
        {
            if (ModelState.IsValid)
            {
                db.Workers.Add(worker);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Отдел = new SelectList(db.Departs, "Код_отдела", "Название", worker.Отдел);
            ViewBag.Пол = new SelectList(db.Genders, "Код_пола", "Пол", worker.Пол);
            ViewBag.Роль = new SelectList(db.Roles, "Код_роли", "Должность", worker.Роль);
            return View(worker);
        }

        // GET: Workers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Worker worker = db.Workers.Find(id);
            if (worker == null)
            {
                return HttpNotFound();
            }
            ViewBag.Отдел = new SelectList(db.Departs, "Код_отдела", "Название", worker.Отдел);
            ViewBag.Пол = new SelectList(db.Genders, "Код_пола", "Пол", worker.Пол);
            ViewBag.Роль = new SelectList(db.Roles, "Код_роли", "Должность", worker.Роль);
            return View(worker);
        }

        // POST: Workers/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Код_сотрудника,Отдел,Фамилия,Имя,Отчество,Дата_рождения,Телефон,Почта,Пол,Роль")] Worker worker)
        {
            if (ModelState.IsValid)
            {
                db.Entry(worker).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Отдел = new SelectList(db.Departs, "Код_отдела", "Название", worker.Отдел);
            ViewBag.Пол = new SelectList(db.Genders, "Код_пола", "Пол", worker.Пол);
            ViewBag.Роль = new SelectList(db.Roles, "Код_роли", "Должность", worker.Роль);
            return View(worker);
        }

        // GET: Workers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Worker worker = db.Workers.Find(id);
            if (worker == null)
            {
                return HttpNotFound();
            }
            return View(worker);
        }

        // POST: Workers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Worker worker = db.Workers.Find(id);
            db.Workers.Remove(worker);
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
