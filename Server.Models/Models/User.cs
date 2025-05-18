using Server.Models.Enums;
using System.Text.Json.Serialization;

namespace Server.Models.Models;

public partial class User
{
    [JsonIgnore]
    public byte[] UserId { get; set; } = null!;

    public string Email { get; set; } = null!;

    [JsonIgnore]
    public string Salt { get; set; } = null!;

    [JsonIgnore]
    public string Password { get; set; } = null!;

    /// <summary>
    /// sup amdin - 1
    /// admin - 2
    /// lector - 3
    /// student -4
    /// </summary>
    public Roles Role { get; set; }

    [JsonIgnore]
    public string? RefreshToken { get; set; }
    [JsonIgnore]
    public DateTime? RefreshTokenExpiry { get; set; }

    [JsonIgnore]
    public virtual Student? Student { get; set; }
    [JsonIgnore]
    public virtual Worker? Worker { get; set; }
}
