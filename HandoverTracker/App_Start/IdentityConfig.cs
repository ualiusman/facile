using HandoverTracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HandoverTracker
{
    public class ApplicationDbInitializer 
    : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            InitializeIdentityForEF(context);
            base.Seed(context);
        }

        public static void InitializeIdentityForEF(ApplicationDbContext context)
        {
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            
            CreateRole(RoleManager, "Admin");
            CreateRole(RoleManager, "Product Owner");
            CreateRole(RoleManager, "Scrum Master");
            CreateRole(RoleManager, "Team Member");

            var user = new ApplicationUser();
            user.UserName = "admin";
            user.FirstName = "Admin";
            user.LastName = "Admin";
            user.Email = "admin@facile.com";
            var result = UserManager.Create(user,"uml2uml");
            if(result.Succeeded)
            {
                UserManager.AddToRole(user.Id, "Admin");
            }

            CreateData(context);

        }

        public static void CreateRole(RoleManager<IdentityRole> RoleManager, string role)
        {
            if(!RoleManager.RoleExists(role))
            {
                RoleManager.Create(new IdentityRole(role));
            }
        }

        private static void CreateData(ApplicationDbContext context)
        {
            List<Artifact> artifacts = new List<Artifact>();
            List<Activty> activties = new List<Activty>();

            #region PD Handover
            //Management Activties
            string po = "Product Owner";
            string sm = "Scrum Master";
            string tm = "Team Member";

            var vd = new Artifact() { IsDeletable = false, IsDeleted = false, Name = "Vision Document", Description = "Vision Document" };
            context.Artifacts.Add(vd);
            context.SaveChanges();
            
            activties.Add(new Activty()
           {
               ActivtyRoles = new List<ActivtyRole>()
               {
                new ActivtyRole(){ RoleName = po},
                new ActivtyRole(){ RoleName = sm}
               },
               IsDeletable = false,
               
               Type = (long)ActivtyTypes.PDMangementActivties,
               IsDeleted = false,
               OutputArtifact = vd,
               Name = "Prodcut Vision Planning",
           });

            var pb = new Artifact() { IsDeletable = false, IsDeleted = false, Name = "Product Backlog", Description = "Product Backlog" };
            context.Artifacts.Add(pb);
            context.SaveChanges();
            
            activties.Add(new Activty()
            {
                ActivtyRoles = new List<ActivtyRole>()
               {
                new ActivtyRole(){ RoleName = po},
                new ActivtyRole(){ RoleName = sm}
               },
                IsDeletable = false,
                
                Type = (long)ActivtyTypes.PDMangementActivties,
                IsDeleted = false,
                InputArtifact = vd,
                OutputArtifact = pb,
                Name = "Inital Setup and Release Planning",
            });


            var sb = new Artifact() { IsDeletable = false, IsDeleted = false, Name = "Sprint Backlog", Description = "Sprint Backlog" };
            context.Artifacts.Add(sb);
            context.SaveChanges();

            activties.Add(new Activty()
            {
                ActivtyRoles = new List<ActivtyRole>() {
                new ActivtyRole() { RoleName = sm },
                new ActivtyRole() { RoleName = tm }
                },
                IsDeletable = false,
                
                Type = (long)ActivtyTypes.PDMangementActivties,
                IsDeleted = false,
                InputArtifact = pb,
                OutputArtifact = sb,
                Name = "Iteration Planning",
            });


            var sr = new Artifact() { IsDeletable = false, IsDeleted = false, Name = "Status Report", Description = "Status Report" };
            context.Artifacts.Add(sr);
            context.SaveChanges();


            activties.Add(new Activty()
            {
                ActivtyRoles = new List<ActivtyRole>() {
                    new ActivtyRole(){ RoleName = po},
                    new ActivtyRole(){ RoleName = sm},
                    new ActivtyRole(){ RoleName = tm},
                },
                IsDeletable = false,
                
                Type = (long)ActivtyTypes.PDMangementActivties,
                IsDeleted = false,
                OutputArtifact = sr,
                Name = "Daily Task Status And Planing Updates",
            });


            var rr = new Artifact() { IsDeletable = false, IsDeleted = false, Name = "Reivew Report", Description = "Reivew Report" };
            context.Artifacts.Add(rr);
            context.SaveChanges();

            activties.Add(new Activty()
            {
                ActivtyRoles = new List<ActivtyRole>() {
                    new ActivtyRole(){ RoleName = po},
                    new ActivtyRole(){ RoleName = sm},
                    new ActivtyRole(){ RoleName = tm},
                },
                IsDeletable = false,
                
                Type = (long)ActivtyTypes.PDMangementActivties,
                IsDeleted = false,
                OutputArtifact = rr,
                Name = "Frequent Sprint Reviews",
            });

            var ps = new Artifact() { IsDeletable = false, IsDeleted = false, Name = "Potensial Shipable (PD)", Description = "Potensial Shipable (PD)" };
            context.Artifacts.Add(ps);
            context.SaveChanges();

            activties.Add(new Activty()
            {
                ActivtyRoles = new List<ActivtyRole>() {
                    new ActivtyRole(){ RoleName = po},
                    new ActivtyRole(){ RoleName = sm},
                    new ActivtyRole(){ RoleName = tm},
                },
                IsDeletable = false,
                
                Type = (long)ActivtyTypes.PDMangementActivties,
                IsDeleted = false,
                OutputArtifact = ps,
                Name = "Frequent Sprint Reviews",
            });

           //Development Activties
           //=========================================================================

            var us = new Artifact() { IsDeletable = false, IsDeleted = false, Name = "User Stories", Description = "User Stories" };
            context.Artifacts.Add(us);
            context.SaveChanges();

            activties.Add(new Activty()
            {
                ActivtyRoles = new List<ActivtyRole>() {
                    new ActivtyRole(){ RoleName = po},
                    new ActivtyRole(){ RoleName = sm},
                    new ActivtyRole(){ RoleName = tm},
                },
                IsDeletable = false,
                
                Type = (long)ActivtyTypes.PDDevelopmentActivties,
                IsDeleted = false,
                OutputArtifact = us,
                Name = "User Stories Construction",
            });

            var mf = new Artifact() { IsDeletable = false, IsDeleted = false, Name = "Mockup Files", Description = "Mockup Files" };
            context.Artifacts.Add(mf);
            context.SaveChanges();

            activties.Add(new Activty()
            {
                ActivtyRoles = new List<ActivtyRole>() {
                    new ActivtyRole(){ RoleName = po},
                    new ActivtyRole(){ RoleName = sm},
                    new ActivtyRole(){ RoleName = tm},
                },
                IsDeletable = false,
                
                Type = (long)ActivtyTypes.PDDevelopmentActivties,
                IsDeleted = false,
                InputArtifact = us,
                OutputArtifact = mf,
                Name = "Mockup Construction",
            });

            var siuModel = new Artifact() { IsDeletable = false, IsDeleted = false, Name = "SUI Model", Description = "SUI Model" };
            context.Artifacts.Add(siuModel);
            context.SaveChanges();

            activties.Add(new Activty()
            {
                ActivtyRoles = new List<ActivtyRole>() {
                    new ActivtyRole(){ RoleName = po},
                    new ActivtyRole(){ RoleName = sm},
                    new ActivtyRole(){ RoleName = tm},
                },
                IsDeletable = false,
                
                Type = (long)ActivtyTypes.PDDevelopmentActivties,
                IsDeleted = false,
                InputArtifact = mf,
                OutputArtifact = siuModel,
                Name = "Mockup Processing and Widget detection",
            });

            var EnsiuModel = new Artifact() { IsDeletable = false, IsDeleted = false, Name = "Enrich SUI Model", Description = "Enrich SUI Model" };
            context.Artifacts.Add(EnsiuModel);
            context.SaveChanges();

            activties.Add(new Activty()
            {
                ActivtyRoles = new List<ActivtyRole>() {
                    new ActivtyRole(){ RoleName = po},
                    new ActivtyRole(){ RoleName = sm},
                    new ActivtyRole(){ RoleName = tm},
                },
                IsDeletable = false,
                
                Type = (long)ActivtyTypes.PDDevelopmentActivties,
                IsDeleted = false,
                InputArtifact = siuModel,
                OutputArtifact = EnsiuModel,
                Name = "Feature Specifaction",
            });

            var wa = new Artifact() { IsDeletable = false, IsDeleted = false, Name = "Web Application", Description = "Web Application" };
            context.Artifacts.Add(wa);
            context.SaveChanges();

            activties.Add(new Activty()
            {
                ActivtyRoles = new List<ActivtyRole>() {
                    new ActivtyRole(){ RoleName = tm},
                },
                IsDeletable = false,
                
                Type = (long)ActivtyTypes.PDDevelopmentActivties,
                IsDeleted = false,
                InputArtifact = EnsiuModel,
                OutputArtifact = wa,
                Name = "Code Generation",
            });



            activties.Add(new Activty()
            {
                ActivtyRoles = new List<ActivtyRole>() {
                    new ActivtyRole(){ RoleName = po},
                },
                IsDeletable = false,
                
                Type = (long)ActivtyTypes.PDDevelopmentActivties,
                IsDeleted = false,
                OutputArtifact = wa,
                Name = "Run Demo Appliation",
            });


            var mModels = new Artifact() { IsDeletable = false, IsDeleted = false, Name = "MDWE Models", Description = "MDWE Models" };
            context.Artifacts.Add(mModels);
            context.SaveChanges();

            activties.Add(new Activty()
            {
                ActivtyRoles = new List<ActivtyRole>() {
                    new ActivtyRole(){ RoleName = tm},
                },
                IsDeletable = false,
                
                Type = (long)ActivtyTypes.PDDevelopmentActivties,
                IsDeleted = false,
                OutputArtifact = mModels,
                Name = "Model Generation",
            });

            // Handover Activties
            //=====================================================================================

            var mpd = new Artifact() { IsDeletable = false, IsDeleted = false, Name = "Mangement Plans Doc", Description = "Mangement Plans Doc" };
            context.Artifacts.Add(mpd);
            context.SaveChanges();

            activties.Add(new Activty()
            {
                ActivtyRoles = new List<ActivtyRole>() {
                    new ActivtyRole(){ RoleName = sm},
                },
                IsDeletable = false,
                
                Type = (long)ActivtyTypes.PDHandoverActivties,
                IsDeleted = false,
                OutputArtifact = mpd,
                Name = "Mangement and Adminstration",
            });

            var mtpd = new Artifact() { IsDeletable = false, IsDeleted = false, Name = "Mantainance Plans Doc", Description = "Mantainance Plans Doc" };
            context.Artifacts.Add(mtpd);
            context.SaveChanges();

            activties.Add(new Activty()
            {
                ActivtyRoles = new List<ActivtyRole>() {
                    new ActivtyRole(){ RoleName = sm},
                },
                IsDeletable = false,
                
                Type = (long)ActivtyTypes.PDHandoverActivties,
                IsDeleted = false,
                OutputArtifact = mtpd,
                Name = "Mantainance Planning",
            });


            var docPD = new Artifact() { IsDeletable = false, IsDeleted = false, Name = "Documentation (PD)", Description = "Documentation (PD)" };
            context.Artifacts.Add(docPD);
            context.SaveChanges();

            activties.Add(new Activty()
            {
                ActivtyRoles = new List<ActivtyRole>() {
                    new ActivtyRole(){ RoleName = sm},
                    new ActivtyRole(){ RoleName = tm},

                },
                IsDeletable = false,
                
                Type = (long)ActivtyTypes.PDHandoverActivties,
                IsDeleted = false,
                OutputArtifact = docPD,
                Name = "Documentation (PD)",
            });



            #endregion


            #region Transition


            var ptsT = new Artifact() { IsDeletable = false, IsDeleted = false, Name = "Potensial Shipable (T)", Description = "Potensial Shipable (T)" };
            context.Artifacts.Add(ptsT);
            context.SaveChanges();

            activties.Add(new Activty()
            {
                ActivtyRoles = new List<ActivtyRole>() {
                    new ActivtyRole(){ RoleName = po},
                    new ActivtyRole(){ RoleName = sm},
                    new ActivtyRole(){ RoleName = tm},

                },
                IsDeletable = false,
                
                Type = (long)ActivtyTypes.TMangementActivties,
                IsDeleted = false,
                OutputArtifact = ptsT,
                Name = "Itration Completion Final",
            });

            var rDoc = new Artifact() { IsDeletable = false, IsDeleted = false, Name = "Release plans docs", Description = "Release plans docs" };
            context.Artifacts.Add(rDoc);
            context.SaveChanges();

            activties.Add(new Activty()
            {
                ActivtyRoles = new List<ActivtyRole>() {
                    new ActivtyRole(){ RoleName = sm},

                },
                IsDeletable = false,
                
                Type = (long)ActivtyTypes.TMangementActivties,
                IsDeleted = false,
                OutputArtifact = rDoc,
                Name = "Release Planning",
            });

            var ddDoc = new Artifact() { IsDeletable = false, IsDeleted = false, Name = "Deployemnt Detail Doc", Description = "Deployemnt Detail Doc" };
            context.Artifacts.Add(ddDoc);
            context.SaveChanges();

            activties.Add(new Activty()
            {
                ActivtyRoles = new List<ActivtyRole>() {
                    new ActivtyRole(){ RoleName = sm},
                    new ActivtyRole(){ RoleName = tm},
                },
                IsDeletable = false,
                
                Type = (long)ActivtyTypes.THandoverActivties,
                IsDeleted = false,
                OutputArtifact = ddDoc,
                Name = "Deployment",
            });

            var docT = new Artifact() { IsDeletable = false, IsDeleted = false, Name = "Documentation (T)", Description = "Documentation (T)" };
            context.Artifacts.Add(docT);
            context.SaveChanges();

            activties.Add(new Activty()
            {
                ActivtyRoles = new List<ActivtyRole>() {
                    new ActivtyRole(){ RoleName = sm},
                    new ActivtyRole(){ RoleName = tm},
                },
                IsDeletable = false,
                
                Type = (long)ActivtyTypes.THandoverActivties,
                IsDeleted = false,
                OutputArtifact = docT,
                Name = "Documentation",
            });

            var meDoc = new Artifact() { IsDeletable = false, IsDeleted = false, Name = "Maintenance Envior Doc", Description = "Maintenance Envior Doc" };
            context.Artifacts.Add(meDoc);
            context.SaveChanges();

            activties.Add(new Activty()
            {
                ActivtyRoles = new List<ActivtyRole>() {
                    new ActivtyRole(){ RoleName = po},
                    new ActivtyRole(){ RoleName = sm},
                    new ActivtyRole(){ RoleName = tm},
                },
                IsDeletable = false,
                
                Type = (long)ActivtyTypes.THandoverActivties,
                IsDeleted = false,
                OutputArtifact = meDoc,
                Name = "Maintenance Enviroment",
            });


            #endregion

            #region PTD Handover

            var fvd = new Artifact() { IsDeletable = false, IsDeleted = false, Name = "Final Version Doc", Description = "Final Version Doc" };
            context.Artifacts.Add(fvd);
            context.SaveChanges();

            activties.Add(new Activty()
            {
                ActivtyRoles = new List<ActivtyRole>() {
                    new ActivtyRole(){ RoleName = po},
                    new ActivtyRole(){ RoleName = sm},
                },
                IsDeletable = false,
                
                Type = (long)ActivtyTypes.PTDManagementActivties,
                IsDeleted = false,
                OutputArtifact = fvd,
                Name = "Product Vision planning Final",
            });










            var um = new Artifact() { IsDeletable = false, IsDeleted = false, Name = "User Manuals", Description = "User Manuals" };
            context.Artifacts.Add(um);
            context.SaveChanges();

            activties.Add(new Activty()
            {
                ActivtyRoles = new List<ActivtyRole>() {
                    new ActivtyRole(){ RoleName = po},
                    new ActivtyRole(){ RoleName = sm},
                    new ActivtyRole(){ RoleName = tm},
                },
                IsDeletable = false,
                
                Type = (long)ActivtyTypes.PTDHandoverActivties,
                IsDeleted = false,
                OutputArtifact = um,
                Name = "Training",
            });

            var vcDoc = new Artifact() { IsDeletable = false, IsDeleted = false, Name = "Version/Config Doc", Description = "Version/Config Doc" };
            context.Artifacts.Add(vcDoc);
            context.SaveChanges();

            activties.Add(new Activty()
            {
                ActivtyRoles = new List<ActivtyRole>() {
                    new ActivtyRole(){ RoleName = sm},
                    new ActivtyRole(){ RoleName = tm},
                },
                IsDeletable = false,
                
                Type = (long)ActivtyTypes.PTDHandoverActivties,
                IsDeleted = false,
                OutputArtifact = vcDoc,
                Name = "Vision and Configuration Mangement",
            });


            var mmDoc = new Artifact() { IsDeletable = false, IsDeleted = false, Name = "Maintainability management Doc", Description = "Maintainability management Doc" };
            context.Artifacts.Add(mmDoc);
            context.SaveChanges();

            activties.Add(new Activty()
            {
                ActivtyRoles = new List<ActivtyRole>() {
                    new ActivtyRole(){ RoleName = sm},
                    new ActivtyRole(){ RoleName = tm},
                },
                IsDeletable = false,
                
                Type = (long)ActivtyTypes.PTDHandoverActivties,
                IsDeleted = false,
                OutputArtifact = mmDoc,
                Name = "Maintainability Management",
            });


            var ptdDoc = new Artifact() { IsDeletable = false, IsDeleted = false, Name = "Documentation (PTD)", Description = "Documentation (PTD)" };
            context.Artifacts.Add(ptdDoc);
            context.SaveChanges();

            activties.Add(new Activty()
            {
                ActivtyRoles = new List<ActivtyRole>() {
                    new ActivtyRole(){ RoleName = sm},
                    new ActivtyRole(){ RoleName = tm},
                },
                IsDeletable = false,
                
                Type = (long)ActivtyTypes.PTDHandoverActivties,
                IsDeleted = false,
                OutputArtifact = ptdDoc,
                Name = "Documenation (PTD)",
            });

            #endregion

            context.Activties.AddRange(activties);
            context.SaveChanges();
            
        }
    }
}