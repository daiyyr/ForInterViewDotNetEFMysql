using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication1.Models;

namespace MvcApplication1.Controllers
{
    public class ClassController : Controller
    {
        testdbEntities db = new testdbEntities();
        public ActionResult Index()
        {
            List<Models.@class> list = (from d in db.@class where d.id != null select d).ToList();

            ViewData["DataList"] = list;
            return View();
        }


        public ActionResult Create()
        {
            @class cla = new @class();
            return View(cla);
        }

        //
        // POST: /Home/Create

        [HttpPost]
        public ActionResult Create(@class model)
        {
            if (string.IsNullOrEmpty(model.name))
            {
                ModelState.AddModelError("name", "please type in a name");
            }
            if (string.IsNullOrEmpty(model.description))
            {
                ModelState.AddModelError("description", "please type in description");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            db.@class.Add(model);
            try
            {
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Content("Modify failed. " + ex.Message);
            }
        }

        [Authorize(Users = "admin@qq.com")]
        [HttpGet]
        public ActionResult Modify(int id)
        {
            @class cla = (from a in db.@class where a.id == id select a).FirstOrDefault();
            return View(cla);
        }

        [Authorize(Users = "admin@qq.com")]
        [HttpPost]
        public ActionResult Modify(@class model)
        {
            if (string.IsNullOrEmpty(model.name))
            {
                ModelState.AddModelError("name", "please type in a name");
            }
            if (string.IsNullOrEmpty(model.description))
            {
                ModelState.AddModelError("description", "please type in description");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            

            //将实体装入EF对象容器
            DbEntityEntry<@class> entry = db.Entry<@class>(model);
            entry.State = EntityState.Unchanged;

            //设置被修改属性
            entry.Property(a => a.name).IsModified = true;
            entry.Property(a => a.description).IsModified = true;
            
            
            try
            {
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Content("Modify failed. " + ex.Message);
            }
        }

        [Authorize(Users = "admin@qq.com")]
        public ActionResult Del(int id)
        {
            sxc ss = (from a in db.sxc where a.class_id == id select a).FirstOrDefault();
            if(ss != null){
                return Content("Sorry, you can not delete a class with student(s)");
            }
            try
            {
                @class cla = new @class { id = id };
                db.@class.Attach(cla);
                db.@class.Remove(cla);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Content("delete failed. " + ex.Message);
            }
        }

    }
}
