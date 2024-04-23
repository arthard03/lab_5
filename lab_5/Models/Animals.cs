using System.ComponentModel.DataAnnotations;

namespace lab_5.Models
{
    public class Animals
    {
 
        public int IdAnimal { get; set; }
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        [Required]
        [MaxLength(200)]
        public string Description { get; set; } 
        [Required]
        [MaxLength(200)]
        public string Category { get; set; }
        [Required]
        [MaxLength(200)]
        public string Area { get; set; }    
    }
}