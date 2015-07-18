using HandoverTracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HandoverTracker.Controllers
{
    [Authorize(Roles="Admin")]
    public class UsersController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        //
        // GET: /Users/
        public ActionResult Index()
        {
            var usersList = new List<UserViewModel>();
            foreach(var user in _db.Users.Where(u => u.IsDeleted == false ))
            {
                var userModel = new UserViewModel(user);
                usersList.Add(userModel);
            }
            return View(usersList);
        }

        //
        // GET: /Users/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Users/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Users/Create
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
        // GET: /Users/Edit/5
        public ActionResult Edit(string id)
        {
           var user =  _db.Users.First(u => u.UserName == id);
           var userModel = new UserViewModel(user);
            return View(userModel);
        }

        //
        // POST: /Users/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, FormCollection collection)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var user = _db.Users.First(u => u.UserName == id);
                    user.FirstName = collection["FirstName"];
                    user.LastName = collection["LastName"];
                    user.Email = collection["Email"];
                    _db.Entry(user).State = EntityState.Modified;
                    _db.SaveChanges();
                    var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_db));
                    var allRoles = _db.Roles.Select(r => r.Name).ToList();
                    var roles = user.Roles.ToList().Select(u => u.Role.Name).ToList();
                    foreach (var role in roles)
                    {
                        UserManager.RemoveFromRole(user.Id, role);

                    }
                    foreach (var item in allRoles)
                    {
                        if( collection[item] =="on")
                        {
                            UserManager.AddToRole(user.Id, item);
                        }
                    }


                    
                }
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Users/Delete/5
        public ActionResult Delete(string id)
        {
            var user = _db.Users.First(u => u.UserName == id);
            var userModel = new UserViewModel(user);
            return View(userModel);
        }

        //
        // POST: /Users/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                var user = _db.Users.First(u => u.UserName == id);
                if (user.UserName != "admin")
                {
                    user.IsDeleted = true;
                    _db.Entry(user).State = EntityState.Modified;
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
