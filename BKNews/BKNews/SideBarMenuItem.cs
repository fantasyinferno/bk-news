using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKNews
{

    public class SideBarMenuItem
    {
        public SideBarMenuItem()
        {
            TargetType = typeof(SideBarDetail);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}