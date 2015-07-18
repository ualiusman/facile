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
    [Authorize]
    public class ProjectController : Controller
    {

        private ApplicationDbContext _db = new ApplicationDbContext();
        //
        //
        // GET: /Project/
        public ActionResult Index()
        {
            List<Project> projectList = new List<Project>();
            if (User.IsInRole("Product Owner"))
            {
                var user = _db.Users.First(u => u.UserName == User.Identity.Name);
                projectList = _db.Projects.Where(f => f.IsDeleted == false && f.ProductOwnerID == user.UserID).ToList();

            }
            else if(User.IsInRole("Scrum Master"))
            {
                var user = _db.Users.First(u => u.UserName == User.Identity.Name);
                projectList = _db.Projects.Where(f => f.IsDeleted == false && f.ScrumMasterID == user.UserID).ToList();

            }
            else if(User.IsInRole("Team Member"))
            {
                var user = _db.Users.First(u => u.UserName == User.Identity.Name);
                var pl = _db.Projects.Where(f => f.IsDeleted == false).ToList();
                foreach (var item in pl)
                {
                    foreach (var i in item.ProjectTeam)
                    {
                        if(i.UserID == user.UserID)
                        {
                            projectList.Add(item);
                        }
                    }
                }
            }
            else
            {
                projectList = _db.Projects.Where(f => f.IsDeleted == false).ToList();

            }
            return View(projectList);
        }

        //
        // GET: /Project/Details/5
        public ActionResult Details(long id)
        {
            var project = _db.Projects.Where(p => p.ProjectID == id).First();
            ViewBag.ProductOwner = _db.Users.Where(f => f.UserID == project.ProductOwnerID).Select(f => new { ProductOwner = f.FirstName + " " + f.LastName }).First().ProductOwner;
            ViewBag.ScrumMaster = _db.Users.Where(f => f.UserID == project.ScrumMasterID).Select(f => new { ScrumMaster = f.FirstName + " " + f.LastName }).First().ScrumMaster;
            string teamMembers = string.Empty;
            foreach (var team in project.ProjectTeam)
            {
                teamMembers += _db.Users.Where(f => f.UserID == team.UserID).Select(f => new { TeamMember = f.FirstName + " " + f.LastName }).First().TeamMember + ", ";
            }
            ViewBag.TeamMembers = new  MvcHtmlString( teamMembers);
            Session["CurProject"] = id;
            return View(project);
        }

        //
        // GET: /Project/Create
        [Authorize(Roles = "Admin,Product Owner")]
        public ActionResult Create()
        {
            var users = _db.Users.Where(u => u.IsDeleted == false).ToList();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_db));
            List<SelectListItem> productOwnerUsers = new List<SelectListItem>();
            List<SelectListItem> scrumMasterUsers = new List<SelectListItem>();
            List<SelectListItem> teamMembers = new List<SelectListItem>();
            foreach (var user in users)
            {
                if (UserManager.IsInRole<ApplicationUser>(user.Id, "Product Owner"))
                {
                    productOwnerUsers.Add(new SelectListItem() { Text = user.FirstName + " " + user.LastName, Value = user.UserID.ToString() });
                }

            }

            foreach (var user in users)
            {
                if (UserManager.IsInRole<ApplicationUser>(user.Id, "Scrum Master"))
                {
                    scrumMasterUsers.Add(new SelectListItem() { Text = user.FirstName + " " + user.LastName, Value = user.UserID.ToString() });
                }
            }

            foreach (var user in users)
            {
                if (UserManager.IsInRole<ApplicationUser>(user.Id, "Team Member"))
                {
                    teamMembers.Add(new SelectListItem() { Text = user.FirstName + " " + user.LastName, Value = user.UserID.ToString() });
                }
            }
            ViewBag.ProductOwners = productOwnerUsers;
            ViewBag.ScrumMasters = scrumMasterUsers;
            ViewBag.TeamMembers = teamMembers;
            return View();
        }

        //
        // POST: /Project/Create
        [Authorize(Roles = "Admin, Product Owner")]
        [HttpPost]
        public ActionResult Create(Project model, FormCollection collection)
        {
            var users = _db.Users.Where(u => u.IsDeleted == false).Select(u => u.UserID).ToList();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_db));


            try
            {
                string productOwner = collection["ProductOwners"];
                string scrumMaster = collection["ScrumMasters"];
                if (!string.IsNullOrEmpty(productOwner))
                    model.ProductOwnerID = Convert.ToInt64(productOwner);
                if (!string.IsNullOrEmpty(scrumMaster))
                    model.ScrumMasterID = Convert.ToInt64(scrumMaster);
                List<ProjectTeam> projectTeam = new List<ProjectTeam>();


                model.Status = "Yet To Start";
                model.Phase = string.Empty;
                if (ModelState.IsValid)
                {
                    _db.Projects.Add(model);
                    _db.SaveChanges();
                    // add project team members
                    foreach (var userId in users)
                    {
                        if (collection[userId.ToString()] == "on")
                        {
                            projectTeam.Add(new ProjectTeam() { ProjectID = model.ProjectID, UserID = userId });
                        }
                    }

                    if(projectTeam.Count >0)
                    {
                        _db.TeamProjects.AddRange(projectTeam);
                        _db.SaveChanges();
                    }

                    // add Project Activites
                    AddProjectActivties(model.ProjectID);

                    return RedirectToAction("Index");



                }
                else
                {
                    return RedirectToAction("Create");
                }
                // TODO: Add insert logic here
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Project/Edit/5
        [Authorize(Roles = "Admin, Product Owner")]
        public ActionResult Edit(long id)
        {
            var users = _db.Users.Where(u => u.IsDeleted == false).ToList();
            var project = _db.Projects.Where(p => p.ProjectID == id).First();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_db));
            List<SelectListItem> productOwnerUsers = new List<SelectListItem>();
            List<SelectListItem> scrumMasterUsers = new List<SelectListItem>();
            List<SelectListItem> teamMembers = new List<SelectListItem>();
            foreach (var user in users)
            {
                if (UserManager.IsInRole<ApplicationUser>(user.Id, "Product Owner"))
                {
                    var item = new SelectListItem() { Text = user.FirstName + " " + user.LastName, Value = user.UserID.ToString() };
                    if (project.ProductOwnerID == user.UserID)
                        item.Selected = true;
                    productOwnerUsers.Add(item);
                }

            }

            foreach (var user in users)
            {
                if (UserManager.IsInRole<ApplicationUser>(user.Id, "Scrum Master"))
                {
                    var item = new SelectListItem() { Text = user.FirstName + " " + user.LastName, Value = user.UserID.ToString() };
                    if (project.ScrumMasterID == user.UserID)
                        item.Selected = true;
                    scrumMasterUsers.Add(item);
                }
            }

            foreach (var user in users)
            {
                if (UserManager.IsInRole<ApplicationUser>(user.Id, "Team Member"))
                {
                    var item = new SelectListItem() { Text = user.FirstName + " " + user.LastName, Value = user.UserID.ToString() };
                    foreach (var team in project.ProjectTeam)
                    {
                        if(user.UserID == team.UserID)
                        {
                            item.Selected = true;
                        }
                    }
                    teamMembers.Add(item);
                }
            }
            ViewBag.ProductOwners = productOwnerUsers;
            ViewBag.ScrumMasters = scrumMasterUsers;
            ViewBag.TeamMembers = teamMembers;
            return View(project);
        }

        //
        // POST: /Project/Edit/5
        [Authorize(Roles = "Admin, Product Owner")]
        [HttpPost]
        public ActionResult Edit(long id,Project model, FormCollection collection)
        {
            try
            {
                var users = _db.Users.Where(u => u.IsDeleted == false).Select(u => u.UserID).ToList();
                var project = _db.Projects.Where(p => p.ProjectID == id).First();
                List<ProjectTeam> projectTeam = new List<ProjectTeam>();

                project.Name = model.Name;
                project.StartDate = model.StartDate;
                project.ExpectedEndDate = model.ExpectedEndDate;
                project.EndDate = model.EndDate;
                project.Description = model.Description;

                string productOwner = collection["ProductOwners"];
                string scrumMaster = collection["ScrumMasters"];
                if (!string.IsNullOrEmpty(productOwner))
                    model.ProductOwnerID = Convert.ToInt64(productOwner);
                if (!string.IsNullOrEmpty(scrumMaster))
                    model.ScrumMasterID = Convert.ToInt64(scrumMaster);

                _db.Entry(project).State = EntityState.Modified;
                _db.SaveChanges();

                _db.TeamProjects.RemoveRange(project.ProjectTeam);
                _db.SaveChanges();

                foreach (var userId in users)
                {
                    if (collection[userId.ToString()] == "on")
                    {
                        projectTeam.Add(new ProjectTeam() { ProjectID = model.ProjectID, UserID = userId });
                    }
                }

                if (projectTeam.Count > 0)
                {
                    _db.TeamProjects.AddRange(projectTeam);
                    _db.SaveChanges();
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
        // GET: /Project/Delete/5
        [Authorize(Roles = "Admin, Product Owner")]
        public ActionResult Delete(long id)
        {
            var project = _db.Projects.Where(p => p.ProjectID == id).First();

            return View(project);
        }

        //
        // POST: /Project/Delete/5
        [Authorize(Roles = "Admin, Product Owner")]
        [HttpPost]
        public ActionResult Delete(long id, FormCollection collection)
        {


            try
            {
                var project = _db.Projects.Where(p => p.ProjectID == id).First();
                project.IsDeleted = true;
                _db.Entry(project).State = EntityState.Modified;
                _db.SaveChanges();

                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [Authorize]
        public ActionResult PDetail(long id)
        {
            var projectActivties = _db.ProjectActivties.Where(f => f.ProjectID == id).ToList();
            var pn = _db.Projects.Find(id);
            ViewBag.Title = pn.Name;
            return View(projectActivties);
        }


        #region Mehtods
                    
        private void AddProjectActivties(long projectId)
        {
            List<ProjectActivty> projectActivties = new List<ProjectActivty>();

            var activties = _db.Activties.Where(f => f.IsDeletable == false);
            bool ft = true;
            foreach (var item in activties)
            {
                string status = "Yet To Start";
                if(ft)
                {
                    status = "Running";
                    ft = false;
                }
                projectActivties.Add(new ProjectActivty() { ActivtyID = item.ActivtyID, ProjectID = projectId, Status = status });
            }

            _db.ProjectActivties.AddRange(projectActivties);
            _db.SaveChanges();
        }
         
        #endregion
    }
}
