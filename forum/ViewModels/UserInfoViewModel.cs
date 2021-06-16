using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace forum.ViewModels
{
    public class UserInfoViewModel
    {
        public string introduction { get; set; }
        public string UserName { get; set; }
        public string website { get; set; }
        public string ExistingPhotoPath { get; set; }

        public int Article { get; set; }
        public int like { get; set; }
        public int reply { get; set; }

    }
}
