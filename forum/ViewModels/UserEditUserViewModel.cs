using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace forum.ViewModels
{
    public class UserEditUserViewModel
    {
        public string Email { get; set; }

        public string website { get; set; }
        public string introduction { get; set; }
        public string ExistingPhotoPath { get; set; }
        public IFormFile PhotoPath { get; set; }

    }
}
