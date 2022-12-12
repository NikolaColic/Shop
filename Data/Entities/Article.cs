using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    [Serializable]
    public class Article
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Key { get; set; } 
        [Required]
        public decimal Price { get; set; }
        public bool IsSold { get; set; }
        public DateTime? SoldDate { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }
    }
}
