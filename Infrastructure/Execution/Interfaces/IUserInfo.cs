using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Execution.Interfaces
{
    public interface IUserInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public bool IsAdmin { get; set; }  
    }
}
