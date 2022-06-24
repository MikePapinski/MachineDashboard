using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineDashboard.Interfaces
{
    public interface ISoftwareVersion
    {
        string Major { get; }
        string Minor { get; }
        string Patch { get; }
    }
}
