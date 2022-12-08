using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    public class Article
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Key { get; set; } 
        [Required]
        public decimal Price { get; set; }
        public bool IsSold { get; set; }
        public DateTime? SoldDate { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}
