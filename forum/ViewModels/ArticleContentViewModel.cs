using forum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace forum.ViewModels
{
    public class ArticleContentViewModel
    {
        public Article Article { get; set; }

        public IEnumerable<ArticleReply> ArticleReplyList { get; set; }
        public ArticleReply ArticleReply { get; set; }
        public bool like  { get; set; }
}
}
