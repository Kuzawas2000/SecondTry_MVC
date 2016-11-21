using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SecondTry.Models;
using System.IO;


namespace SecondTry.Controllers
{
    public class PeopleController : Controller
    {
        private PeopleDBContext db = new PeopleDBContext();

        // GET: People
        public ActionResult Index(string searchString)
        {
            var ppl = from m in db.People
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {               
                ppl = ppl.Where(s => s.name.Contains(searchString));
            }           
            return View(ppl);           
        }
        //public ActionResult Index()
      //  {
           // return View(db.People.ToList());
     //   }
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                if (fileName.Contains("csv"))
                {
                    var path = Path.Combine(Server.MapPath("~/App_Data"), "zmones.csv");
                    file.SaveAs(path);
                }
                return RedirectToAction("AddingFileData");
            } 
            return RedirectToAction("Index");

        }

        // GET: People/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            People people = db.People.Find(id);
            if (people == null)
            {
                return HttpNotFound();
                
            }
            return View(people);
        }

        // GET: People/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,age,gender")] People people)
        {
            if (ModelState.IsValid)
            {
                db.People.Add(people);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(people);
        }

        // GET: People/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            People people = db.People.Find(id);
            if (people == null)
            {
                return HttpNotFound();
            }
            return View(people);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,age,gender")] People people)
        {
            if (ModelState.IsValid)
            {
                db.Entry(people).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(people);
        }

        // GET: People/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            People people = db.People.Find(id);
            if (people == null)
            {
                return HttpNotFound();
            }
            return View(people);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            People people = db.People.Find(id);
            db.People.Remove(people);
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

        public ActionResult AddingFileData()
        {

            People people = new People();
            string[] duomenys = System.IO.File.ReadAllLines("C:\\Users\\z.alkovikas\\Desktop\\C#\\zmones.csv");
            int ilgis = duomenys.Length;
            string[] Vardas = new string[duomenys.Length];
            int[] Metai = new int[duomenys.Length];
            string[] Lytis = new string[duomenys.Length];
            for (int i = 0; i < ilgis; i++)
            {
                string[] tarpinis = duomenys[i].Split(';');
                Vardas[i] = tarpinis[0];
                Metai[i] = Convert.ToInt32(tarpinis[1]);
                Lytis[i] = tarpinis[2];
                people.name = Vardas[i];
                people.age = Metai[i];
                people.gender = Lytis[i];
                db.People.Add(people);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        
    }
}
