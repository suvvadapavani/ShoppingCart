using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCart.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        [StringLength(500)]
        public string Description {  get; set; }
        [Required]
        public decimal Price { get; set; }
        public string? ImagePath {  get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [NotMapped]
        public IFormFile? ImageFile { get; set; }

    }
}
