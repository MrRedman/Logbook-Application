using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Globalization;

namespace LogBook.Controllers
{
    public class DailyRegisterController : Controller
    {
        private LogBookDBContext logbookDb = new LogBookDBContext();
        //
        // GET: /DailyRegister/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /DailyRegister/Details/5
        //
        // GET: /DailyRegister/Create

        public ActionResult Create()
        {
            PopulateLearnerDropDownList();
            PopulateMentorDropDownList();

            return View();
        }

        //
        // POST: /DailyRegister/Create

        [HttpPost]
        public ActionResult Create([Bind(Include= "AttendanceDate, Hour, TrainingOn, PersonId, Mentor")]AttendanceDetail attendance)
        {
            try
            {
                // TODO: Add insert logic here

                PopulateLearnerDropDownList();
                PopulateMentorDropDownList();

                if (ModelState.IsValid) 
                {
                    logbookDb.AttendanceDetails.Add(attendance);
                    logbookDb.SaveChanges();
                    return RedirectToAction("Index", new { controller = "Home", action = "Index", id = UrlParameter.Optional });
                }


                return RedirectToAction("Home/Index");
            }
            catch(RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes, Try again, and if the problem persist, see your system adminstrator.");
                
            }

            return View();
        }

        [HttpGet]
        public JsonResult LoadAttendance(DateTime attendanceDate) 
        {
     
            var model = logbookDb.AttendanceDetails
                .Where(a => a.AttendanceDate >= attendanceDate.Date && a.AttendanceDate <= attendanceDate.Date)
                .Take(1)
                .Select(a => new
                    {
                        Learner = a.PersonId,
                        Hours = a.Hour,
                        Mentor = a.Mentor,
                        _TrainingOn = a.TrainingOn
                    }).ToList();


            return Json(model, JsonRequestBehavior.AllowGet); 

           
        }


  
 
        private void PopulateLearnerDropDownList(object selectedLearner = null) 
        {
            var learnerQuery = from l in logbookDb.People
                               where l.RoleId == 1
                               orderby l.FirstName
                               select l;

            ViewBag.Learners = new SelectList(learnerQuery, "ID", "FirstName", selectedLearner);
        }


        private void PopulateMentorDropDownList(object selectedMentor = null) 
        {
            var mentorQuery = from m in logbookDb.People
                              where m.RoleId == 2
                              orderby m.FirstName
                              select m;

            ViewBag.Mentors = new SelectList(mentorQuery, "ID", "FirstName", selectedMentor);
        }

        protected override void Dispose(bool disposing)
        {

            if (logbookDb != null) 
            {
                logbookDb.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
