using System.ComponentModel.DataAnnotations;

namespace Burgers.Api.DTOs
{
    public class MenuItemAdminDto
    {
        public int Id {  get; set; }

        [Required]
        public required string Name { get; set; }
        public string? Description { get; set; }

        [Range(0, int.MaxValue)]
        public decimal Price { get; set; }
        [Required]
        public required string Category { get; set; }

        public bool IsAvailable { get; set; }
    }
}
