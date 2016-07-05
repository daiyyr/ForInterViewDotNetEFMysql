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


        public ActionResult Create()
        {
            student stu = new student();
            //班级列表下拉框
            IEnumerable<SelectListItem> listItem = from c in db.@class
                                                   where c.id != null
                                                   select new SelectListItem{
                                                       Value = c.id.ToString(),
                                                       Text = c.name,};

            ViewBag.classList = listItem;
            return View(stu);
        }

        //
        // POST: /Home/Create

        [HttpPost]
        public ActionResult Create(student model)
        {
            if (string.IsNullOrEmpty(model.name))
            {
                ModelState.AddModelError("name", "please type in a name");
            }
            if (string.IsNullOrEmpty(Request.Form["dob"]))
            {
                ModelState.AddModelError("date of birth", "please type in a valid date");
            }

            if (!ModelState.IsValid)
            {
                //返回前先处理下拉框
                IEnumerable<SelectListItem> listItem = from c in db.@class
                                                       where c.id != null
                                                       select new SelectListItem
                                                       {
                                                           Value = c.id.ToString(),
                                                           Text = c.name,
                                                       };

                ViewBag.classList = listItem;
                return View(model);
            }

            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "yyyy-MM-dd";
            try
            {
                model.dob = Convert.ToDateTime(Request.Form["dob"], dtFormat);
            }
            catch (Exception)
            {
                ModelState.AddModelError("date of birth", "please type in a valid date");
                //返回前先处理下拉框
                IEnumerable<SelectListItem> listItem = from c in db.@class
                                                       where c.id != null
                                                       select new SelectListItem
                                                       {
                                                           Value = c.id.ToString(),
                                                           Text = c.name,
                                                       };

                ViewBag.classList = listItem;
                return View(model);
            }

            db.student.Add(model);
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Content("Modify failed. " + ex.Message);
            }

            int id = db.student.Select(p => p.id).Max();

            db.sxc.Add(new sxc { student_id = id,  class_id = int.Parse(Request.Form["class"]) });
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

        //
        // GET: /Home/Modify/5
        [HttpGet]
        public ActionResult Modify(int id)
        {
            student stu = (from a in db.student where a.id == id select a).FirstOrDefault();

            //班级列表下拉框
            IEnumerable<SelectListItem> listItem = from c in db.@class 
                                                where c.id != null 
                                                select new SelectListItem { Value = c.id.ToString(), Text = c.name, Selected=
                                                (c.sxc.FirstOrDefault().student_id == stu.id ? true : false)

                                                //(stu.sxc.FirstOrDefault().class_id == c.id ? true : false)
                                                //这才是正确的下拉框设置, 但是会报错, ?

                                                    };
                                                
            ViewBag.classList = listItem;
            return View(stu);
        }

        //
        // POST: /Home/Modify/5

        [HttpPost]
        public ActionResult Modify(student model)
        {
            if (string.IsNullOrEmpty(model.name))
            {
                ModelState.AddModelError("name", "please type in a name");
            }
            if (string.IsNullOrEmpty(Request.Form["dob"]))
            {
                ModelState.AddModelError("date of birth", "please type in a valid date");

                //找回原来的生日:
                student stu = (from a in db.student where a.id == model.id select a).FirstOrDefault();
                model.dob = stu.dob;
            }

            if (!ModelState.IsValid)
            {
                //返回前先处理下拉框
                IEnumerable<SelectListItem> listItem = from c in db.@class
                                                       where c.id != null
                                                       select new SelectListItem
                                                       {
                                                           Value = c.id.ToString(),
                                                           Text = c.name,
                                                           Selected =
                                                               (c.sxc.FirstOrDefault().student_id == model.id ? true : false)

                                                           //(stu.sxc.FirstOrDefault().class_id == c.id ? true : false)
                                                           //这才是正确的下拉框设置, 但是会报错, ?

                                                       };

                ViewBag.classList = listItem;
                return View(model);
            }
            
            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "yyyy-MM-dd";
            try
            {
                model.dob = Convert.ToDateTime(Request.Form["dob"], dtFormat);
            }
            catch (Exception)
            {
                ModelState.AddModelError("date of birth", "please type in a valid date");

                //找回原来的生日:
                student stu = (from a in db.student where a.id == model.id select a).FirstOrDefault();
                model.dob = stu.dob;

                //返回前先处理下拉框
                IEnumerable<SelectListItem> listItem = from c in db.@class
                                                       where c.id != null
                                                       select new SelectListItem
                                                       {
                                                           Value = c.id.ToString(),
                                                           Text = c.name,
                                                           Selected =
                                                               (c.sxc.FirstOrDefault().student_id == model.id ? true : false)

                                                           //(stu.sxc.FirstOrDefault().class_id == c.id ? true : false)
                                                           //这才是正确的下拉框设置, 但是会报错, ?
                                                       };

                ViewBag.classList = listItem;
                return View(model);
            }

            

            //将实体装入EF对象容器
            DbEntityEntry<student> entry = db.Entry<student>(model);
            entry.State = EntityState.Unchanged;

            //设置被修改属性
            entry.Property(a => a.name).IsModified = true;
            entry.Property(a => a.dob).IsModified = true;
//            entry.Collection(a => a.sxc).IsLoaded = true;

            sxc ss = (from a in db.sxc where a.student_id == model.id select a).FirstOrDefault();
            db.sxc.Attach(ss);
            db.sxc.Remove(ss);
            db.sxc.Add(new sxc { student_id = (model.id), class_id =int.Parse(Request.Form["class"]) });
            
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
