namespace Server.Models.Models;

public partial class Auditlog
{
    public uint Id { get; set; }

    public string UserId { get; set; } = null!;

    public string ActionType { get; set; } = null!; // "Insert", "Update", "Delete"

    public string EntityName { get; set; } = null!;

    public string? EntityId { get; set; }

    public DateTime Timestamp { get; set; }

    public string? Description { get; set; }

    public string? OldValue { get; set; }

    public string? NewValue { get; set; }
}
