
using Summer_Bookstore_Domain.Entities;


namespace Summer_Bookstore_Infrastructure.EventLogs;

public class AuditEntry
{
    public int Id { get; set; }
    public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    public LogType LogType { get; set; } = LogType.Information;   
    public string? Message { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }

}
