using System.ComponentModel.DataAnnotations;

namespace Burgers.Api.DTOs
{
    public class MenuItemDto
    {
        [Required]
        public required string Name { get; set; }
        public string? Description { get; set; }

        [Range(0, 100000 , ErrorMessage = "Price Must be from 0 to 10000")] 
        public decimal Price { get; set; } 
        [Required]
        public required string Category { get; set; }

        public bool IsAvailable { get; set; }
    }
}
