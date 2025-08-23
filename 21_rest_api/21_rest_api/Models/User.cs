﻿using System.ComponentModel.DataAnnotations;

namespace _21_rest_api.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    [Required, StringLength(50)]
    public string Username { get; set; } = string.Empty;
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    public byte[] PasswordHash { get; set; } = [];
    [Required]
    public byte[] PasswordSalt { get; set; } = [];

    public List<TaskItem> TaskItems { get; set; } = [];
    public string Role { get; set; } = "User";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
