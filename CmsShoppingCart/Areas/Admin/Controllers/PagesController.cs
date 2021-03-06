﻿using CmsShoppingCart.Models.Data;
using CmsShoppingCart.Models.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CmsShoppingCart.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            List<PageVM> pageList;

            using (Db db = new Db())
            {
                pageList = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageVM(x)).ToList();
            }

            return View(pageList);
        }

        // GET: Admin/Pages/AddPage
        [HttpGet]
        public ActionResult AddPage()
        {
            return View();
        }

        // POST: Admin/Pages/AddPage
        [HttpPost]
        public ActionResult AddPage(PageVM model)
        {
            if (!ModelState.IsValid) return View(model);

            using (Db db = new Db())
            {
                string slug = model.Slug;

                PageDTO pdto = new PageDTO();

                pdto.Title = model.Title;

                if (string.IsNullOrWhiteSpace(slug))
                    slug = model.Title.Replace(" ", "-").ToLower();
                else
                    slug = slug.Replace(" ", "-").ToLower();

                if (db.Pages.Any(x => x.Title.Equals(model.Title)) || db.Pages.Any(x => x.Slug.Equals(slug)))
                {
                    if (db.Pages.Any(x => x.Slug.Equals(slug)))
                        ModelState.AddModelError("", "Thje Slug already exist. Please change the Slug");
                    if (db.Pages.Any(x => x.Title.Equals(model.Title)))
                        ModelState.AddModelError("", "The Title already exist. Please change the Title");
                    return View(model);
                }

                pdto.Slug = slug;
                pdto.Body = model.Body;
                pdto.HasSidebar = model.HasSidebar;
                pdto.Sorting = 100;

                db.Pages.Add(pdto);
                db.SaveChanges();
            }

            TempData["SM"] = "You have added a new page : " + model.Title;

            return RedirectToAction("AddPage");
        }

        // GET: Admin/Pages/EditPage/id
        [HttpGet]
        public ActionResult EditPage(int id)
        {
            PageVM model;
            using (Db db = new Db())
            {
                PageDTO pdto = db.Pages.Find(id);
                
                if (pdto == null)
                {
                    return Content("This page does not exist.");
                }

                model = new PageVM(pdto);

            }
            return View(model);
        }

        // POST: Admin/Pages/EditPage
        [HttpPost]
        public ActionResult EditPage(PageVM model)
        {
            if (!ModelState.IsValid) return View(model);

            using (Db db = new Db())
            {
                PageDTO pdto = new PageDTO();
                pdto = db.Pages.Find(model.id);

                string slug = model.Slug;

                if (model.Slug != "home")
                {
                    if (string.IsNullOrWhiteSpace(slug))
                        slug = model.Title.Replace(" ", "-").ToLower();
                    else
                        slug = slug.Replace(" ", "-").ToLower();
                }

                if (db.Pages.Where(x => x.id != model.id).Any(x => x.Title.Equals(model.Title)) || db.Pages.Where(x => x.id != model.id).Any(x => x.Slug.Equals(slug)))
                {
                    if (db.Pages.Where(x => x.id != model.id).Any(x => x.Slug.Equals(slug)))
                        ModelState.AddModelError("", "Thje Slug already exist. Please change the Slug");
                    if (db.Pages.Where(x => x.id != model.id).Any(x => x.Title.Equals(model.Title)))
                        ModelState.AddModelError("", "The Title already exist. Please change the Title");
                    return View(model);
                }

                pdto.Title = model.Title;
                pdto.Slug = slug;
                pdto.Body = model.Body;
                pdto.HasSidebar = model.HasSidebar;
                pdto.Sorting = 100;

                db.SaveChanges();
            }

            TempData["SM"] = "You have edited page : " + model.Title;

            return RedirectToAction("EditPage");
        }


        // GET: Admin/Pages/DeletePage/id
        [HttpGet]
        public ActionResult DeletePage(int id)
        {
            PageVM model;
            using (Db db = new Db())
            {
                PageDTO pdto = db.Pages.Find(id);

                if (pdto == null)
                {
                    return Content("This page does not exist.");
                }

                model = new PageVM(pdto);

            }
            return View(model);
        }

        // Post: Admin/Pages/DeletePage/id
        [HttpPost]
        public ActionResult DeletePage(PageVM model)
        {
            using (Db db = new Db())
            {
                PageDTO pdto = db.Pages.Find(model.id);

                db.Pages.Remove(pdto);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // GET: Admin/Pages/Detail/id
        [HttpGet]
        public ActionResult DetailPage(int id)
        {
            PageVM model;
            using (Db db = new Db())
            {
                PageDTO pdto = db.Pages.Find(id);

                if (pdto == null)
                {
                    return Content("This page does not exist.");
                }

                model = new PageVM(pdto);

            }
            return View(model);
        }

        // Post: Admin/Pages/ReorderPages
        [HttpPost]
        public void ReorderPages(int[] id)
        {
            using (Db db = new Db())
            {
                int count = 1;
                PageDTO pdto;

                foreach (var PageID in id)
                {
                    pdto = db.Pages.Find(PageID);

                    pdto.Sorting = count;

                    db.SaveChanges();

                    count++;
                }
            }
        }

        // GET: Admin/Pages/EditSidebar/id
        [HttpGet]
        public ActionResult EditSidebar()
        {
            SidebarVM model;
            using (Db db = new Db())
            {
                SidebarDTO sdto = db.Sidebar.Find(1);

                if (sdto == null)
                {
                    return Content("This page does not exist.");
                }

                model = new SidebarVM(sdto);

            }
            return View(model);
        }

        // GET: Admin/Pages/EditSidebar/id
        [HttpPost]
        public ActionResult EditSidebar(SidebarVM model)
        {
            if (!ModelState.IsValid) return View(model);

            using (Db db = new Db())
            {
                SidebarDTO sdto = db.Sidebar.Find(1);

                sdto.Body = model.Body;

                db.SaveChanges();
            }

            TempData["SM"] = "You have edited sidebar : " + model.Id;

            return RedirectToAction("EditSidebar");
        }
    }
}