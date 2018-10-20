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
    public class ScoresController : Controller
    {
        private CompetitionEntities db = new CompetitionEntities();

        // GET: Scores
        public ActionResult Index(int? eventID)
        {
           
            if (eventID == null)
            {var  scores = db.Scores.Include(s => s.Event).Include(s => s.Competitor);
                return View(scores.ToList().OrderByDescending(x => x.JudgeTime));
            }
            else
            { 


             var scores = from score in db.Scores
                         where score.EventId == eventID
                         select score;
                ViewBag.eventID = eventID;

                if (db.Events.Find(eventID).Pro == "FIN")
                {
                    ViewBag.IsFin = true;
                }
                else
                {
                    ViewBag.IsFin = false;
                }
                ViewBag.IsTs = db.Events.Find(eventID).Name.Contains("综合");
                ViewBag.eventTitle = db.Events.Find(eventID).Name+"|" + db.Events.Find(eventID).Pro;
                return View(scores.ToList().OrderByDescending(x=>x.JudgeTime));
            }
            
        }
        /*
        public ActionResult Index(int eventID)
        {
            var scores = from score in db.Scores
                         where score.EventId == eventID
                         select score;
            return View(scores.ToList());
        }
        */
        // GET: Scores/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Score score = db.Scores.Find(id);
            if (score == null)
            {
                return HttpNotFound();
            }
            return View(score);
        }

        // GET: Scores/Create
        public ActionResult Create(int? eventID,string error)
        {
            if(eventID==null)
            {
                ViewBag.Events = new SelectList(db.Events, "Id", "Name");
            }
            else
            {
                ViewBag.EventId =eventID;
                //判断是不是综合题
                ViewBag.IsTs= db.Events.Find(eventID).Name.Contains("综合");
                List<SelectListItem> items = new List<SelectListItem>();
                items.Add(new SelectListItem() { Text = db.Events.Find(eventID).Name, Value = eventID.ToString()});
                if (db.Events.Find(eventID).Pro == "FIN")
                {
                    ViewBag.IsFin = true;
                }
                else
                {
                    ViewBag.IsFin = false;
                }
                ViewBag.Events = items;
            }


            ViewBag.Company = new SelectList(db.Companys, "Id", "Name");

            ViewBag.CompetitorId = new SelectList(db.Competitors, "Id", "Name");
            ViewBag.Error = error;
            return View();
        }

        // POST: Scores/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,EventId,TimeConsume,TimePenalty,CompetitorId,Mark")] Score score)
        {
            if (ModelState.IsValid)
            {
                score.JudgeTime = DateTime.Now;
                score.ModifyTime = DateTime.Now;
                //TimeSpan consume=new TimeSpan(Convert.ToInt32( Request.Form["ConsumeHour"]), Convert.ToInt32(Request.Form["ConsumeMin"]), Convert.ToInt32(Request.Form["ConsumeSec"]));
                TimeSpan consume = new TimeSpan(0, Convert.ToInt32(Request.Form["ConsumeMin"]), Convert.ToInt32(Request.Form["ConsumeSec"]));
                score.TimeConsume = consume;
                //TimeSpan penalty = new TimeSpan(Convert.ToInt32(Request.Form["PenaltyHour"]), Convert.ToInt32(Request.Form["PenaltyMin"]), Convert.ToInt32(Request.Form["PenaltySec"]));
                TimeSpan penalty = new TimeSpan(0, Convert.ToInt32(Request.Form["PenaltyMin"]), Convert.ToInt32(Request.Form["PenaltySec"]));
                score.TimePenalty = penalty;
                string name = User.Identity.Name;

                Judge judge = db.Judges.Where(x => x.StaffId == name).First();
                if (judge != null)
                {
                    score.Judge = judge;
                }
                else
                {
                    return RedirectToAction("Index", new { eventID = score.EventId });
                }

                IQueryable<Score> sameCompetitor= db.Scores.Where(x => (x.EventId == score.EventId)&&(x.JudgeId == judge.Id)&&(x.CompetitorId==score.CompetitorId));
                if( sameCompetitor.Count()>0)
                {
                    string mes = "同一个裁判对同一选手只允许评分一次！";                    
                    return RedirectToAction("Create", new { eventID = score.EventId ,error=mes});
                }
                db.Scores.Add(score);
                db.SaveChanges();
                return RedirectToAction ("Index", new { eventID=score.EventId });
            }

            ViewBag.EventId = new SelectList(db.Events, "Id", "Name", score.EventId);
            ViewBag.CompetitorId = new SelectList(db.Competitors, "Id", "Name", score.CompetitorId);
            return View(score);
        }

        // GET: Scores/Edit/5
        [Authorize(Roles ="Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Score score = db.Scores.Find(id);
            if (score == null)
            {
                return HttpNotFound();
            }
            if (score.Event.Pro == "FIN")
            {
                ViewBag.IsFin = true;
            }
            else
            {
                ViewBag.IsFin = false;
            }
            ViewBag.IsTs =score.Event.Name.Contains("综合");
            ViewBag.Company = new SelectList(db.Companys, "Id", "Name",score.Competitor.CompanyId);
            ViewBag.EventId = new SelectList(db.Events, "Id", "Name", score.EventId);
            ViewBag.CompetitorId = new SelectList(db.Competitors, "Id", "Name", score.CompetitorId);
            return View(score);
        }

        // POST: Scores/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EventId,TimeConsume,TimePenalty,CompetitorId,JudgeId,JudgeTime,Mark")] Score score)
        {
            if (ModelState.IsValid)
            {
                //TimeSpan consume = new TimeSpan(Convert.ToInt32(Request.Form["ConsumeHour"]), Convert.ToInt32(Request.Form["ConsumeMin"]), Convert.ToInt32(Request.Form["ConsumeSec"]));
                string s = Request.Form["ConsumeMin"];
                TimeSpan consume = new TimeSpan(0, Convert.ToInt32(Request.Form["ConsumeMin"]), Convert.ToInt32(Request.Form["ConsumeSec"]));
                score.TimeConsume = consume;
                //TimeSpan penalty = new TimeSpan(Convert.ToInt32(Request.Form["PenaltyHour"]), Convert.ToInt32(Request.Form["PenaltyMin"]), Convert.ToInt32(Request.Form["PenaltySec"]));
                TimeSpan penalty = new TimeSpan(0, Convert.ToInt32(Request.Form["PenaltyMin"]), Convert.ToInt32(Request.Form["PenaltySec"]));
                score.TimePenalty = penalty;
                score.ModifyTime = DateTime.Now;

                db.Entry(score).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { eventID = score.EventId });
            }
                      
            ViewBag.Company = new SelectList(db.Companys, "Id", "Name"); ;
            ViewBag.EventId = new SelectList(db.Events, "Id", "Name", score.EventId);
            ViewBag.CompetitorId = new SelectList(db.Competitors, "Id", "Name", score.CompetitorId);
            return View(score);
        }

        // GET: Scores/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Score score = db.Scores.Find(id);
            if (score == null)
            {
                return HttpNotFound();
            }
            return View(score);
        }

        // POST: Scores/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Score score = db.Scores.Find(id);
            db.Scores.Remove(score);
            db.SaveChanges();
            ViewBag.IsTs = db.Events.Find(score.EventId).Name.Contains("综合");
            return RedirectToAction("Index", new { eventID = score.EventId });
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
