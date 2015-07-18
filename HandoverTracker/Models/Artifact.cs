using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HandoverTracker.Models
{
    public class Artifact
    {
        public Artifact()
        {
            InputActivties = new List<Activty>();
            OutputActivties = new List<Activty>();
        }

        [Key]
        public long ArtifacatID { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description {get;set;}

        public bool IsDeletable { get; set; }
        
        public bool IsDeleted {get;set;}

        public virtual ICollection<Activty> InputActivties { get; set; }
        public virtual ICollection<Activty> OutputActivties { get; set; }
    }
}