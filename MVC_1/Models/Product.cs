using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_1.Models
{
    public class Product
    {
        [Key]
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public decimal? Amount { get; set; }
        [MaxLength(255)]
        public string? Image { get; set; }

        [ForeignKey("Category")]
        public string? CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
