using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HandoverTracker.Models
{
    public class Activty
    {
        public Activty()
        {
            ActivtyRoles = new List<ActivtyRole>();
            ProjectActivties = new List<ProjectActivty>();
        }

        [Key]
        public long ActivtyID { get; set; }

        [Required]
        [MaxLength(200)]
        [Display(Name="Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name="Type")]
        public long Type { get; set; }

        

        public bool IsDeletable { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<ActivtyRole> ActivtyRoles { get; set; }
        public virtual ICollection<ProjectActivty> ProjectActivties { get; set; }

        [InverseProperty("InputActivties")]
        public virtual Artifact InputArtifact { get; set; }
        
        [InverseProperty("OutputActivties")]
        public virtual Artifact OutputArtifact { get; set; }
    }



    public class ActivtyRole
    {
        [Key]
        public long ActivtyRoleID { get; set; }

        public long ActivtyID { get; set; }

        public string RoleName { get; set; }

        [ForeignKey("ActivtyID")]
        public virtual Activty Activty { get; set; }
    }

    public class ProjectActivty
    {
        [Key]
        public long ProjectActivtyID { get; set; }
        public long ProjectID { get; set; }
        public long ActivtyID { get; set; }
        [MaxLength(100)]
        public string Status { get; set; }
        
        [ForeignKey("ActivtyID")]
        public virtual Activty Activty { get; set; }

        [ForeignKey("ProjectID")]
        public virtual Project Project { get; set; }
    }
    
}