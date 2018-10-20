using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WebApplication1.Models;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    public class JudgesController : Controller
    {
        // GET: Judges
        private CompetitionEntities db = new CompetitionEntities();
        public ActionResult Index()
        {
            return View(db.Judges.ToList());
        }

        public ActionResult SetEvent(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Judge judge = db.Judges.Find(id);
            if(judge==null)
            {
                return HttpNotFound();
            }

            ViewBag.CompanyId = new SelectList(db.Companys, "Id", "Name",judge.CompanyId);
            ViewBag.EventId = new SelectList(db.Events, "Id", "Name",judge.EventId);
            return View(judge);
        }

        [HttpPost]
        public ActionResult SetEvent(Judge judge)
        {
            if (ModelState.IsValid)
            {
               db.Entry(judge).State = EntityState.Modified;
                //var setEntry = ((IObjectContextAdapter)db).ObjectContext.ObjectStateManager.GetObjectStateEntry(judge);
                //setEntry.SetModifiedProperty("EventId");
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CompanyId = new SelectList(db.Companys, "Id", "Name",judge.CompanyId);
            ViewBag.EventId = new SelectList(db.Events, "Id", "Name", judge.EventId);
            return View(judge);
        }

        public ActionResult Multi(int? id)
        { return View(); }
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Multi()
        {
            if (ModelState.IsValid)
            {
                string content = Request["List"];


                List<string> t = content.Split('\r','\n').ToList();
                ViewBag.Content = t;
                ViewBag.Count = t.Count;

                foreach (string item in t)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        string company = item.Split('\t')[0];
                        int? companyId=null;
                        IQueryable<Company> companys = db.Companys.Where(x => x.Name.Contains(company));
                        if(companys.Count()>0)
                        {
                            companyId =companys.First().Id;
                        }
                       
                        string staffid = item.Split('\t')[1];
                        string Name = item.Split('\t')[2];
                        string eventName = item.Split('\t')[3];
                        int? eventId = null;
                        IQueryable<Event> events = db.Events.Where(x => x.Name.Contains(eventName));
                        if(events.Count()>0)
                        {
                            eventId=events.First().Id;
                        }
                           
                        var user = new ApplicationUser { UserName = staffid, RealName = Name,StaffId=staffid };
                        var result = await UserManager.CreateAsync(user, "123456");
                        if (result.Succeeded)
                        {
                            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                            UserManager.AddToRole(user.Id, "Judge");

                            db.Judges.Add(new Judge { Name = Name, UserId = user.Id,StaffId=staffid, CompanyId = companyId ,EventId=eventId});
                            db.SaveChanges();
                        }
                        
                        
                    }
                }
                return View();
            }


            return View();
        }

    }
}