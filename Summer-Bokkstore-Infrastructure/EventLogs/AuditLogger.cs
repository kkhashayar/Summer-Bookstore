
using Summer_Bookstore_Domain.Entities;
using Summer_Bookstore_Infrastructure.Data;

namespace Summer_Bookstore_Infrastructure.EventLogs;

public class AuditLogger
{
    private readonly BookstoreDbContext _context;

    public AuditLogger(BookstoreDbContext context)
    {
        _context = context; 
    }


    public async Task LogAsync(string message, LogType logType = LogType.Information, User? user = null)
    {
        var entry = new AuditEntry
        {
            Message = message,
            LogType = logType,
            TimeStamp = DateTime.UtcNow,
            User = user

        };  

        await _context.AuditEntries.AddAsync(entry);    
        await _context.SaveChangesAsync();  
    }

}
