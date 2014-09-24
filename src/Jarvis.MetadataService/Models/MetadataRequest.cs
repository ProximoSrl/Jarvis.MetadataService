using System.ComponentModel.DataAnnotations;

namespace Jarvis.MetadataService.Models
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