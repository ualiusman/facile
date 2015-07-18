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
    public class ActivtyController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        private long CurProjectID {
            get
            {
                if (Session["CurProject"] != null)
                    return Convert.ToInt64(Session["CurProject"]);
                else
                    return 0;
            }
        }

        //
        // GET: /Activty/
        [Authorize(Roles = "Admin,Product Owner")]        
        public ActionResult Index()
        {
            var activteis = _db.Activties.Where(f => f.IsDeleted == false);
            return View(activteis);
        }

        //
        // GET: /Activty/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Type(int id)
        {
            List<ProjectActivty> pa = new List<ProjectActivty>();
            
            var activties = _db.ProjectActivties.Where(f => f.Activty.Type == id && f.ProjectID == CurProjectID);
            foreach (var item in activties)
            {
                foreach (var i in item.Activty.ActivtyRoles)
                {
                    if(User.IsInRole(i.RoleName))
                    {
                        pa.Add(item);
                        break;
                    }
                }
            }
            Session["pat"] = id;           

            return View(pa);
        }

        public ActionResult Work(long id)
        {
            var activty = _db.ProjectActivties.Find(id);
            return View(activty);
        }

        [HttpPost]
        public ActionResult Work(long id, ProjectActivty model)
        {
            var activty = _db.ProjectActivties.Find(id);
            activty.Status = "Done";
            long nextActivty = activty.ActivtyID;
            nextActivty++;
            long curProj = activty.ProjectID;
            _db.Entry(activty).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
          
            var na =   _db.ProjectActivties.Where(f => f.ProjectID == curProj && f.ActivtyID == nextActivty).First();
            if( na != null)
            { 
                na.Status = "Running";

                _db.Entry(activty).State = System.Data.Entity.EntityState.Modified;
                _db.SaveChanges();
            }
            return View(activty);
        }

        //
        // GET: /Activty/Create
        [Authorize(Roles = "Admin,Product Owner")]        
        public ActionResult Create()
        {
            List<SelectListItem> activtyType = new List<SelectListItem>();
            List<SelectListItem> roles = new List<SelectListItem>();
            activtyType.Add(new SelectListItem(){ Text="PDMangementActivties", Value="1"});
            activtyType.Add(new SelectListItem(){ Text="PDDevelopmentActivties", Value="2"});
            activtyType.Add(new SelectListItem(){ Text="PDHandoverActivties", Value="3"});
            activtyType.Add(new SelectListItem(){ Text="TMangementActivties", Value="4"});
            activtyType.Add(new SelectListItem(){ Text="THandoverActivties", Value="5"});
            activtyType.Add(new SelectListItem(){ Text="PTDManagementActivties", Value="6"});
            activtyType.Add(new SelectListItem(){ Text="PTDHandoverActivties", Value="7"});

            ViewBag.InputArtifacts = _db.Artifacts.Where(f => f.IsDeleted == false).ToList().Select(a => new SelectListItem() { Value = Convert.ToString(a.ArtifacatID), Text = a.Name }).ToList();
            ViewBag.OutputArtifacts = _db.Artifacts.Where(f => f.IsDeleted == false).ToList().Select(a => new SelectListItem() { Value = Convert.ToString(a.ArtifacatID), Text = a.Name }).ToList();

            ViewBag.ActivtyType = activtyType;

            roles.Add(new SelectListItem() { Text = "Product Owner", Value = "Product Owner" });
            roles.Add(new SelectListItem() { Text = "Scrum Master", Value = "Scrum Master" });
            roles.Add(new SelectListItem() { Text = "Team Member", Value = "Team Member" });

            ViewBag.Roles = roles;

            return View();
        }

        //
        // POST: /Activty/Create
        [HttpPost]
        [Authorize(Roles = "Admin,Product Owner")]        
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                string name = collection["Name"];
                long input = Convert.ToInt64( collection["InputArtifacts"]);
                long output = Convert.ToInt64(collection["OutputArtifacts"]);
                long type = Convert.ToInt64(collection["ActivtyType"]);
                if(!string.IsNullOrEmpty(name))
                {
                    Activty activty = new Activty();
                    List<ActivtyRole> activtyRoles = new List<ActivtyRole>();
                    activty.Name = name;
                    activty.IsDeletable = true;
                    activty.Type = type;
                    activty.IsDeleted = false;
                    Artifact IArtifact = _db.Artifacts.Find(input);
                    Artifact OArtifact = _db.Artifacts.Find(output);
                    if (IArtifact != null)
                        activty.InputArtifact = IArtifact;
                    if (OArtifact != null)
                        activty.OutputArtifact = OArtifact;
                    _db.Activties.Add(activty);
                    _db.SaveChanges();
                    if(collection["Scrum Master"] == "on")
                    {
                        activtyRoles.Add(new ActivtyRole() { ActivtyID = activty.ActivtyID, RoleName = "Scrum Master" });
                    }
                    if(collection["Product Owner"] == "on")
                    {
                        activtyRoles.Add(new ActivtyRole() { ActivtyID = activty.ActivtyID, RoleName = "Product Owner" });
                        
                    }
                    if(collection["Team Member"] == "on")
                    {
                        activtyRoles.Add(new ActivtyRole() { ActivtyID = activty.ActivtyID, RoleName = "Team Member" });
                    }

                    _db.ActivtyRoles.AddRange(activtyRoles);
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
        // GET: /Activty/Edit/5
        public ActionResult Edit(long id)
        {

            var activty = _db.Activties.Find(id);




            List<SelectListItem> activtyType = new List<SelectListItem>();
            List<SelectListItem> roles = new List<SelectListItem>();
            
            activtyType.Add(new SelectListItem() { Text = "PDMangementActivties", Value = "1" });
            activtyType.Add(new SelectListItem() { Text = "PDDevelopmentActivties", Value = "2" });
            activtyType.Add(new SelectListItem() { Text = "PDHandoverActivties", Value = "3" });
            activtyType.Add(new SelectListItem() { Text = "TMangementActivties", Value = "4" });
            activtyType.Add(new SelectListItem() { Text = "THandoverActivties", Value = "5" });
            activtyType.Add(new SelectListItem() { Text = "PTDManagementActivties", Value = "6" });
            activtyType.Add(new SelectListItem() { Text = "PTDHandoverActivties", Value = "7" });

            foreach (var item in activtyType)
            {
                if(item.Value == activty.ActivtyID.ToString())
                {
                    item.Selected = true;
                }
            }

            List<Artifact> ias = _db.Artifacts.Where(f => f.IsDeleted == false).ToList();
            List<Artifact> oas= _db.Artifacts.Where(f => f.IsDeleted == false).ToList();

            List<SelectListItem> ia = new List<SelectListItem>();
            List<SelectListItem> oa = new List<SelectListItem>();

            foreach (var item in ias)
            {
                SelectListItem i = new SelectListItem();
                i.Text = item.Name;
                i.Value = item.ArtifacatID.ToString();
                if(activty.InputArtifact != null)
                {
                    if(item.ArtifacatID == activty.InputArtifact.ArtifacatID)
                    {
                        i.Selected = true;
                    }
                }

                ia.Add(i);
            }





            foreach (var item in oas)
            {
                SelectListItem i = new SelectListItem();
                i.Text = item.Name;
                i.Value = item.ArtifacatID.ToString();
                if (activty.OutputArtifact != null)
                {
                    if (item.ArtifacatID == activty.OutputArtifact.ArtifacatID)
                    {
                        i.Selected = true;
                    }
                }
                oa.Add(i);
            }




            ViewBag.InputArtifacts = ia;
            ViewBag.OutputArtifacts = oa;


            ViewBag.ActivtyType = activtyType;

            roles.Add(new SelectListItem() { Text = "Product Owner", Value = "Product Owner" });
            roles.Add(new SelectListItem() { Text = "Scrum Master", Value = "Scrum Master" });
            roles.Add(new SelectListItem() { Text = "Team Member", Value = "Team Member" });

            foreach (var item in activty.ActivtyRoles)
            {
                foreach (var i2 in roles)
                {
                    if(i2.Value == item.RoleName)
                    {
                        i2.Selected = true;
                    }
                }
            }

            ViewBag.Roles = roles;

            
            
            
            
            return View(activty);
        }

        //
        // POST: /Activty/Edit/5
        [HttpPost]
        public ActionResult Edit(long id, FormCollection collection)
        {
            try
            {
                string name = collection["Name"];
                long input = Convert.ToInt64(collection["InputArtifacts"]);
                long output = Convert.ToInt64(collection["OutputArtifacts"]);
                long type = Convert.ToInt64(collection["ActivtyType"]);
                if (!string.IsNullOrEmpty(name))
                {
                    Activty activty = _db.Activties.Find(id);
                    List<ActivtyRole> activtyRoles = new List<ActivtyRole>();
                    activty.Name = name;
                    activty.IsDeletable = true;
                    activty.Type = type;
                    activty.IsDeleted = false;
                    Artifact IArtifact = _db.Artifacts.Find(input);
                    Artifact OArtifact = _db.Artifacts.Find(output);
                    if (IArtifact != null)
                        activty.InputArtifact = IArtifact;
                    if (OArtifact != null)
                        activty.OutputArtifact = OArtifact;
                    _db.Entry(activty).State = EntityState.Modified;
                    _db.SaveChanges();

                    _db.ActivtyRoles.RemoveRange(activty.ActivtyRoles);
                    _db.SaveChanges();
                    if (collection["Scrum Master"] == "on")
                    {
                        activtyRoles.Add(new ActivtyRole() { ActivtyID = activty.ActivtyID, RoleName = "Scrum Master" });
                    }
                    if (collection["Product Owner"] == "on")
                    {
                        activtyRoles.Add(new ActivtyRole() { ActivtyID = activty.ActivtyID, RoleName = "Product Owner" });


                    }
                    if (collection["Team Member"] == "on")
                    {
                        activtyRoles.Add(new ActivtyRole() { ActivtyID = activty.ActivtyID, RoleName = "Team Member" });
                    }

                    _db.ActivtyRoles.AddRange(activtyRoles);
                    _db.SaveChanges();


                    // TODO: Add update logic here

                }
                return RedirectToAction("Index");

            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Activty/Delete/5
        public ActionResult Delete(long id)
        {
            Activty activty = _db.Activties.Find(id);
            return View(activty);
        }

        //
        // POST: /Activty/Delete/5
        [HttpPost]
        public ActionResult Delete(long id, FormCollection collection)
        {
            try
            {
               Activty activty =  _db.Activties.Find(id);
               if(activty.IsDeletable)
               {
                   activty.IsDeleted = true;
                   _db.Entry(activty).State = EntityState.Modified;
                   _db.SaveChanges();
               }

                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
