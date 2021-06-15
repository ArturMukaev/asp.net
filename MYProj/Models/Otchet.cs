using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MYProj.Models
{
    public class Otchet
    {
        public IEnumerable<Project> Projects { get; set; }
        public IEnumerable<Departs_Tasks> Tasks { get; set; }
    }
}