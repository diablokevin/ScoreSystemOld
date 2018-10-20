using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private CompetitionEntities db = new CompetitionEntities();
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                return View(db.Events.ToList());
            }

            if (User.IsInRole("Judge"))
            {
                string name = User.Identity.Name;
        
                Judge judge= db.Judges.Where(x => x.StaffId == name).First();
                if (judge != null)
                {
                    return View( db.Events.Where(x=>x.Id==judge.EventId));
                }
                else
                {
                    return View(new List< Event>());
                }


               
            }
            ViewBag.Title="主页";
            return View(new List<Event>());

        }
        public ActionResult Test()
        {
            DateTime data = DateTime.Now;
            return View(data);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [Authorize(Roles ="Admin")]
        public ActionResult Setting()
        {
            ViewBag.Message = "基础数据设置";

            return View();
        }
      

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return View();
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }
    }
}