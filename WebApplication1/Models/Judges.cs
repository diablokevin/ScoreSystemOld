using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Judges
    {

        public string Name
        {
            get
            {
                return this.User.RealName;
            }
        }
        public string StaffId
        {
            get
            {
                return this.User.StaffId;
            }
        }

        public virtual Company Company { get; set; }
        public virtual Event Event { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}