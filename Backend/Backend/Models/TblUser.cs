using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public partial class TblUser
{
    public int UserId { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    public string UserName { get; set; } = null!;

    [Required]
    public string Email { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;

    public string? ProfileImage { get; set; }

    public string? Gender { get; set; }

    public string? Bio { get; set; }

    public bool? IsPrivate { get; set; }

    public string? Status { get; set; }

    public DateTime? LastLoginAt { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
