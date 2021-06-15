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
    [Authorize(Roles = "Director")]
    public class ProjectsController : Controller
    {
        private Entities db = new Entities();

        // GET: Projects
        public ActionResult Index()
        {
            return View(db.Projects.ToList());
        }


        public ActionResult Otchet()
        {
            var model = new Otchet();
            model.Projects = db.Projects.ToList();
            model.Tasks = db.Departs_Tasks.ToList();
            return View(model);
        }
        public ActionResult Export()
        {
            List<Project> projects = new List<Project>();
            projects = db.Projects.ToList();
            List<Departs_Tasks> tasks = new List<Departs_Tasks>();
            tasks = db.Departs_Tasks.ToList();
            List<DateTime> array = new List<DateTime>();
DateTime MyDateTime = DateTime.ParseExact("01.01.1800", "dd.MM.yyyy", null);
            foreach (var item in projects)
            { 
            List<DateTime> dates = new List<DateTime>();
            foreach (var thing in tasks)
            {
                if (thing.Проект == item.Код_проекта)
                {
                    if (thing.Реальный_срок != null)
                    {
                        dates.Add(Convert.ToDateTime(thing.Реальный_срок));
                    }
                }
            }
            DateTime date = new DateTime();
            if (dates.Count != 0)
            {
                date = dates.Max<DateTime>();
                    array.Add(date);
            }
                else
                {
                    array.Add(MyDateTime);
                }
}
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var worksheet = workbook.Worksheets.Add("Projects");

                worksheet.Cell("A1").Value = "Отчет на "+ DateTime.Now.ToShortDateString();
                worksheet.Cell("B1").Value = "Проект";
                worksheet.Cell("C1").Value = "Срок выполнения";
                worksheet.Cell("D1").Value = "Реальный срок выполнения";
                worksheet.Row(1).Style.Font.Bold = true;

                //нумерация строк/столбцов начинается с индекса 1 (не 0)
                for (int i = 0; i < projects.Count; i++)
                {
                    worksheet.Cell(i + 2, 2).Value = projects[i].Название;
                    worksheet.Cell(i + 2, 3).Value = projects[i].Срок_выполнения;
                    if (array[i] ==MyDateTime)
                    {
                        worksheet.Cell(i + 2, 4).Value = "Нет данных";
                    }
                    else
                    {
                    worksheet.Cell(i + 2, 4).Value = array[i];
                    }

                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"projects_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }


        // GET: Projects/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Код_проекта,Название,Срок_выполнения")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Код_проекта,Название,Срок_выполнения")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
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
