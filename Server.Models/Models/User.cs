using System;
using System.Collections.Generic;

namespace Server.Models.Models;

public partial class User
{
    public byte[] UserId { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public string Password { get; set; } = null!;

    /// <summary>
    /// sup amdin - 1
    /// admin - 2
    /// lector - 3
    /// student -4
    /// </summary>
    public byte Role { get; set; }

    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }

    public virtual Student? Student { get; set; }
    public virtual Worker? Worker { get; set; }
}
