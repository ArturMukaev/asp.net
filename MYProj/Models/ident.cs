using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MYProj.Models
{
    public class ident
    {
        public IEnumerable<AspNetUser> Users { get; set; }
        public  IEnumerable<AspNetRole> Roles { get; set; }
    }
}