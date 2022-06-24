using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineDashboard.Interfaces
{
    public interface IMachine
    {
        public string Ip { get; set; }
        public string Name { get; set; }
        public string ReservationStatus { get; set; }
        public string Group { get; set; }
        public bool JenkinsBringOnline { get; set; }
        public string JenkinsLabels { get; set; }
        public string JenkinsDescription { get; set; }
    }
}
