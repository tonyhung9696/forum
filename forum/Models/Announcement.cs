using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace forum.Models
{
    public class Announcement
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public DateTime datetime { get; set; }
    }
}
