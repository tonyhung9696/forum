using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace forum.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string name { get; set; }
        public string photopath { get; set; }
        public int grade { get; set; }
        public string registerIP { get; set; }
        public string loginIP { get; set; }
        public DateTime registerDateTime { get; set; }
        public DateTime loginDateTime { get; set; }
        public string website { get; set; }
        public string introduction { get; set; }

    }
}
