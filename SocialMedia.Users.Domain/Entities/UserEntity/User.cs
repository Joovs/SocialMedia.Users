using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Users.Domain.Entities.UserEntity;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

}
