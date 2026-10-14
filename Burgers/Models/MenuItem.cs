using System.ComponentModel.DataAnnotations.Schema;

namespace Burgers.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public required decimal Price { get; set; }
        public bool IsAvailable { get; set; } = true;
        public required string Category { get; set; } = "Food";
    }
}
