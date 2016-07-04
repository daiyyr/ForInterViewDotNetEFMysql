using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication1.Models;

namespace MvcApplication1.Controllers
{
    public class HomeController : Controller
    {
        testdbEntities db = new testdbEntities();
        public ActionResult Index()
        {

            //DbQuery<student> query =  (db.student.Where(d=> d.id != null)) as DbQuery<student>;
            //List<Models.student> list = db.student.Where(d => d.id != null).ToList();

            List<Models.student> list = (from d in db.student where d.id != null select d).ToList();

            ViewData["DataList"] = list;
            return View();
        }


        //
        // GET: /Home/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Home/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Home/Modify/5
        [HttpGet]
        public ActionResult Modify(int id)
        {
            return View();
        }

        //
        // POST: /Home/Modify/5

        [HttpPost]
        public ActionResult Modify(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        public ActionResult Del(int id)
        {
            
            try
            {
                student stu = new student { id = id };

                //sxc s = new sxc { student_id = id };
                //db.sxc.Attach(s);
                //db.sxc.Remove(s);
                //db.SaveChanges();

                db.student.Attach(stu);
                db.student.Remove(stu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return Content("delete failed. " + ex.Message);
            }
        }

    }
}
