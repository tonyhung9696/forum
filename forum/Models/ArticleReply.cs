using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace forum.Models
{
    public class ArticleReply
    {
        [Key]
        public int id { get; set; }
        public string userID { get; set; }
        public string articleID { get; set; }
        public string name { get; set; }
        public string content { get; set; }
        public DateTime datetime { get; set; }
        
    }
}
