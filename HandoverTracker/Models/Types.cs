using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HandoverTracker.Models
{
    public enum ActivtyTypes: int
    {
        PDMangementActivties = 1,
        PDDevelopmentActivties = 2,
        PDHandoverActivties = 3,

        TMangementActivties = 4,
        THandoverActivties = 5,

        PTDManagementActivties = 6,
        PTDHandoverActivties = 7

    }
}