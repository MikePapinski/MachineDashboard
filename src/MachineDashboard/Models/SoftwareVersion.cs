using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineDashboard.Interfaces;

namespace MachineDashboard.Models
{
    public class SoftwareVersion : ISoftwareVersion
    {
        public string Major { get; set; }
        public string Minor { get; set; }
        public string Patch { get; set; }
    }
}
