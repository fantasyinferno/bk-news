using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKNews
{

    public class SidebarPageMenuItem
    {
        public SidebarPageMenuItem()
        {
            TargetType = typeof(SidebarPageDetail);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}