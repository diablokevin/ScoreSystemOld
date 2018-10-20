using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CompetitorsController : Controller
    {
        private CompetitionEntities db = new CompetitionEntities();

        // GET: Competitors
        public ActionResult Index()
        {
            var competitors = db.Competitors.Include(c => c.Company);
           
            return View(competitors.ToList());
        }

        public ActionResult GetCompetorByCompanyId(int id)
        {
            var competitors = db.Competitors.Where(x => x.CompanyId == id).Select(x=>new { x.Id, x.Name } );
           // return View(competitors.ToList());
            return Json(competitors, JsonRequestBehavior.AllowGet);

        }
        // GET: Competitors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Competitor competitor = db.Competitors.Find(id);
            if (competitor == null)
            {
                return HttpNotFound();
            }
            return View(competitor);
        }

        // GET: Competitors/Create
        public ActionResult Create()
        {
            ViewBag.CompanyId = new SelectList(db.Companys, "Id", "Name");
            return View();
        }

        // POST: Competitors/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Pro,CompanyId")] Competitor competitor)
        {
            if (ModelState.IsValid)
            {
                db.Competitors.Add(competitor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CompanyId = new SelectList(db.Companys, "Id", "Name", competitor.CompanyId);
            return View(competitor);
        }

        // GET: Competitors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Competitor competitor = db.Competitors.Find(id);
            if (competitor == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyId = new SelectList(db.Companys, "Id", "Name", competitor.CompanyId);
            return View(competitor);
        }

        // POST: Competitors/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Pro,CompanyId")] Competitor competitor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(competitor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CompanyId = new SelectList(db.Companys, "Id", "Name", competitor.CompanyId);
            return View(competitor);
        }

        // GET: Competitors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Competitor competitor = db.Competitors.Find(id);
            if (competitor == null)
            {
                return HttpNotFound();
            }
            return View(competitor);
        }

        // POST: Competitors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Competitor competitor = db.Competitors.Find(id);
            db.Competitors.Remove(competitor);
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

        public ActionResult Multi(int? id)
        { return View(); }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Multi()
        {
            if (ModelState.IsValid)
            {
                string content = Request["List"];
                

                List<string> t = content.Split('\r', '\n').ToList();
                ViewBag.Content = t;
                ViewBag.Count = t.Count;

                foreach (string item in t)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        Competitor competitor = new Competitor();
                        string company = item.Split('\t')[0];
                        int? companyId = null;
                        IQueryable<Company> companys = db.Companys.Where(x => x.Name.Contains(company));
                        if (companys.Count() > 0)
                        {
                            companyId = companys.First().Id;
                            competitor.CompanyId = companyId.Value;
                        }
                       
                        competitor.Name = item.Split('\t')[1];
                        competitor.Pro = item.Split('\t')[2];
                        db.Competitors.Add(competitor);

                        db.SaveChanges();
                    }
                }
                return View();
            }


            return View();
        }
    }
}
