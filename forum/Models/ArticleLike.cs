using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace forum.Models
{
    public class ArticleLike
    {
        [Key]
        public int id { get; set; }
        public string userID { get; set; }
        public string ArticleID { get; set; }
    }
}
