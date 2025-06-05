namespace _04_form.Models;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string AvatarPath { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
