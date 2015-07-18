using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HandoverTracker;

namespace HandoverTracker.Models
{
    public class UserViewModel
    {
        public UserViewModel() { }
        public UserViewModel(ApplicationUser user)
        {
            UserName = user.UserName;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            Roles = user.Roles.ToList().Select( u => u.Role.Name).ToList();
            RoleList = GetSelectedList(Roles);
        }

        [Display(Name="User Name")]
        public string UserName{get;set;}

        [Required]
        [Display(Name="First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name="Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name="Email")]
        public string Email { get; set; }

        [Display(Name="User Roles")]
        public List<string> Roles { get; set; }

        public List<SelectListItem> RoleList { get; set; }

        private List<SelectListItem> GetSelectedList(List<string> roles)
        {
            ApplicationDbContext _db = new ApplicationDbContext();
             var allRoles = _db.Roles.Select(r => r.Name).ToList();
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in allRoles)
            {
                var si = new SelectListItem();
                si.Text = item;
                if(roles.Contains(item))
                {
                    si.Selected = true;
                }
                si.Value = item;
                list.Add(si);
            }

            return list;
        }
    }
}