using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using MYProj.Models;

namespace MYProj.Controllers
{
    [Authorize(Roles = "Depart_Director,Worker")]
    public class Workers_TasksController : Controller
    {
        private Entities db = new Entities();
        private ApplicationContext db1 = new ApplicationContext();

        // GET: Workers_Tasks

        public ActionResult Index()
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
            
            var workers_Tasks = db.Workers_Tasks.Include(w => w.Project).Include(w => w.Status).Include(w => w.Worker).Where(w=>w.Worker.Отдел == worker.Отдел);
            return View(workers_Tasks.ToList());
        }
        public ActionResult Otchet()
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
                if (item1.Почта.Trim() == req.Email.Trim())
                {
                    worker = item1;
                }
            }

            var workers_Tasks = db.Workers_Tasks.Include(w => w.Project).Include(w => w.Status).Include(w => w.Worker).Where(w => w.Worker.Код_сотрудника == worker.Код_сотрудника);
            return View(workers_Tasks.ToList());
        }

        public ActionResult Export()
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
                if (item1.Почта.Trim() == req.Email.Trim())
                {
                    worker = item1;
                }
            }
            List<Workers_Tasks> workers = new List<Workers_Tasks>();
            workers =  db.Workers_Tasks.Include(w => w.Project).Include(w => w.Status).Include(w => w.Worker).Where(w => w.Worker.Код_сотрудника == worker.Код_сотрудника).ToList();
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var worksheet = workbook.Worksheets.Add("Worker");

                worksheet.Cell("A1").Value = "Отчет на " + DateTime.Now.ToShortDateString();
                worksheet.Cell("B1").Value = "Проект";
                worksheet.Cell("C1").Value = "Задача";
                worksheet.Cell("D1").Value = "Срок выполнения";
                worksheet.Cell("E1").Value = "Статус";
                worksheet.Row(1).Style.Font.Bold = true;

                //нумерация строк/столбцов начинается с индекса 1 (не 0)
                for (int i = 0; i < workers.Count; i++)
                {
                    worksheet.Cell(i + 2, 2).Value = workers[i].Project.Название;
                    worksheet.Cell(i + 2, 3).Value = workers[i].Задача;
                    worksheet.Cell(i + 2, 4).Value = workers[i].Срок_выполнения.ToShortDateString();
                    worksheet.Cell(i + 2, 5).Value = workers[i].Status.Статус;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"worker_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }

        // GET: Workers_Tasks/Create
        public ActionResult Create()
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
            ViewBag.Проект = new SelectList(db.Projects, "Код_проекта", "Название");
            ViewBag.Статус = new SelectList(db.Statuses, "Код_статуса", "Статус");
            ViewBag.Сотрудник = new SelectList(db.Workers.Where(e => e.Отдел == worker.Отдел && e.Роль == 2), "Код_сотрудника", "Фамилия");
            return View();
        }

        // POST: Workers_Tasks/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Проект,Сотрудник,Задача,Срок_выполнения,Статус")] Workers_Tasks workers_Tasks)
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
            if (ModelState.IsValid)
            {
                db.Workers_Tasks.Add(workers_Tasks);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Проект = new SelectList(db.Projects, "Код_проекта", "Название", workers_Tasks.Проект);
            ViewBag.Статус = new SelectList(db.Statuses, "Код_статуса", "Статус", workers_Tasks.Статус);
            ViewBag.Сотрудник = new SelectList(db.Workers.Where(e => e.Отдел == worker.Отдел && e.Роль == 2), "Код_сотрудника", "Фамилия", workers_Tasks.Сотрудник);
            return View(workers_Tasks);
        }

        // GET: Workers_Tasks/Edit/5
        public ActionResult Edit(int pr, int wor)
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
            var  workers_Tasks= db.Workers_Tasks.Where(e => e.Проект == pr && e.Сотрудник == wor).FirstOrDefault();
            if (workers_Tasks == null)
            {
                return HttpNotFound();
            }
            ViewBag.Проект = new SelectList(db.Projects, "Код_проекта", "Название", workers_Tasks.Проект);
            ViewBag.Статус = new SelectList(db.Statuses, "Код_статуса", "Статус", workers_Tasks.Статус);
            ViewBag.Сотрудник = new SelectList(db.Workers.Where(e=>e.Отдел==worker.Отдел&&e.Роль==2), "Код_сотрудника", "Фамилия", workers_Tasks.Сотрудник);
            return View(workers_Tasks);
        }

        // POST: Workers_Tasks/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Проект,Сотрудник,Задача,Срок_выполнения,Статус")] Workers_Tasks workers_Tasks)
        {
            if (ModelState.IsValid)
            {
                db.Entry(workers_Tasks).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Проект = new SelectList(db.Projects, "Код_проекта", "Название", workers_Tasks.Проект);
            ViewBag.Статус = new SelectList(db.Statuses, "Код_статуса", "Статус", workers_Tasks.Статус);
            ViewBag.Сотрудник = new SelectList(db.Workers, "Код_сотрудника", "Фамилия", workers_Tasks.Сотрудник);
            return View(workers_Tasks);
        }

        // GET: Workers_Tasks/Delete/5
        public ActionResult Delete(int pr, int wor)
        {
            var workers_Tasks = db.Workers_Tasks.Where(e => e.Проект == pr && e.Сотрудник == wor).FirstOrDefault();
            if (workers_Tasks == null)
            {
                return HttpNotFound();
            }
            return View(workers_Tasks);
        }

        // POST: Workers_Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int pr, int wor)
        {
            var workers_Tasks = db.Workers_Tasks.Where(e => e.Проект == pr && e.Сотрудник == wor).FirstOrDefault();
            db.Workers_Tasks.Remove(workers_Tasks);
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
