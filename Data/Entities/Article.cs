using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
        [JsonIgnore]
        public bool IsSold { get; set; }
        [JsonIgnore]
        public DateTime? SoldDate { get; set; }
        [JsonIgnore]
        public int? UserId { get; set; }
        [JsonIgnore]
        public User? User { get; set; }
    }
}
