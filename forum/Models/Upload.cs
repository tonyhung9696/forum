using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace forum.Models
{
    public class Upload
    {
        public IFormFile PhotoPath { get; set; }
    }
}
