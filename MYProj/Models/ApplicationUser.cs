using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MYProj.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Pass { get; set; }
        public ApplicationUser()
        {
        }
    }
}