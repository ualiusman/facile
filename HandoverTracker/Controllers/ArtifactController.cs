using HandoverTracker.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HandoverTracker.Controllers
{
    [Authorize]
    public class ArtifactController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        //
        // GET: /Artifact/
        [Authorize(Roles = "Admin, Product Owner, Scrum Master")]
        public ActionResult Index()
        {
            var artifacts = _db.Artifacts.Where(a => a.IsDeleted == false);
            return View(artifacts);
        }

        //
        // GET: /Artifact/Details/5
        [Authorize(Roles = "Admin, Product Owner, Scrum Master")]
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Artifact/Create
        [Authorize(Roles = "Admin, Product Owner")]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Artifact/Create
        [HttpPost]
        [Authorize(Roles = "Admin, Product Owner")]
        public ActionResult Create(Artifact model)
        {
            try
            {
                model.IsDeleted = false;
                model.IsDeletable = true;
                if (ModelState.IsValid)
                {
                    _db.Artifacts.Add(model);
                    _db.SaveChanges();
                }
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Artifact/Edit/5
        [Authorize(Roles = "Admin, Product Owner")]
        public ActionResult Edit(long id)
        {
            var artifact = _db.Artifacts.Find(id);
            if (artifact.IsDeletable)
                return View(artifact);
            else
                return RedirectToAction("Index");
        }

        //
        // POST: /Artifact/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin, Product Owner")]
        public ActionResult Edit(long id, Artifact model)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    var artifact = _db.Artifacts.Find(id);
                    artifact.Name = model.Name;
                    artifact.Description = model.Description;
                    if (artifact.IsDeletable)
                    {
                        _db.Entry(artifact).State = EntityState.Modified;
                        _db.SaveChanges();
                    }
                }
                return RedirectToAction("Index");

            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Artifact/Delete/5
        [Authorize(Roles = "Admin, Product Owner")]
        public ActionResult Delete(long id)
        {
            var artifact = _db.Artifacts.Find(id);
            if (artifact.IsDeletable)
                return View(artifact);
            else
                return RedirectToAction("Index");
        }

        //
        // POST: /Artifact/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin, Product Owner")]
        public ActionResult Delete(int id, Artifact model)
        {
            try
            {
                // TODO: Add delete logic here
                var artifact = _db.Artifacts.Find(id);

                if (artifact.IsDeletable)
                {
                    artifact.IsDeleted = true;
                    _db.Entry(artifact).State = EntityState.Modified;
                    _db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
