using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HandoverTracker.Models
{
    public class Project
    {

        public Project()
        {
            ProjectTeam = new List<ProjectTeam>();
            ProjectActivties = new List<ProjectActivty>();
        }

        public long ProjectID { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        
        [MaxLength(200)]
        public string Status { get; set; }

        [MaxLength(200)]
        public string Phase { get; set; }

        [Required]
        [Display(Name="Start Date")]
        public DateTime StartDate { get; set; }
        
        [Required]
        [Display(Name="Expected End Date")]
        public DateTime ExpectedEndDate { get; set; }
        
        [Display( Name="End Date")]
        public Nullable<DateTime> EndDate { get; set; }
        
        [MaxLength(1000)]
        [Display(Name="Description")]
        public string Description { get; set; }
        
        [Required]
        [Display(Name="Product Owner")]
        public long ProductOwnerID { get; set; }
        
        [Required]
        [Display(Name="Scrum Master")]
        public long ScrumMasterID { get; set; }
        
        public bool IsDeleted { get; set; }

        public virtual ICollection<ProjectTeam> ProjectTeam { get; set; }
        public virtual ICollection<ProjectActivty> ProjectActivties { get; set; }

    }


    public class ProjectTeam
    {
        public long ProjectTeamID { get; set; }
        public long ProjectID { get; set; }
        public long UserID { get; set; }


        [ForeignKey("ProjectID")]
        public Project Project { get; set; }
    }
}