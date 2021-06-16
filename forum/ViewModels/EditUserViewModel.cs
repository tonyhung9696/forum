using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace forum.ViewModels
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            Claims = new List<string>();
            Roles = new List<string>();
        }

        public string Id { get; set; }

        [Required]
        public string UserName { get; set; }

        public string Email { get; set; }

        public string website { get; set; }
        public string introduction { get; set; }
        public string ExistingPhotoPath { get; set; }
        public IFormFile PhotoPath { get; set; }
        public List<string> Claims { get; set; }

        public IList<string> Roles { get; set; }
    }
}
