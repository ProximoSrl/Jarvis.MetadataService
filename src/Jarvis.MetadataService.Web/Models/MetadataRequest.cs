using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Jarvis.MetadataService.Web.Models
{
    public class MetadataRequest
    {
        [Required]
        [MinLength(1)]
        public string Kind { get; set; }

        [Required]
        [MinLength(1)]
        public string Key { get; set; }
    }
}