using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace forum.Models
{
    public class Article
    {
        [Key]
        public int id { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }

        public string userID { get; set; }
        public string name { get; set; }
        public string articleType { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string tag { get; set; }
        public int like { get; set; }
        public int clickRateDaily { get; set; }
        public int clickRateWeekly { get; set; }
        public int top { get; set; }
        public int view { get; set; }
        public int ReplyCount { get; set; }
        public DateTime datetime { get; set; }
        public DateTime Replydatetime { get; set; }
    }
}
