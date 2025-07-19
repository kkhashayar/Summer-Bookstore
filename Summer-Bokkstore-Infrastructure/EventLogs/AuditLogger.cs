
using Microsoft.EntityFrameworkCore;
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
        User? existingUser = null;
        if (!string.IsNullOrEmpty(user.Username))
        {
            existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
        }
        var entry = new AuditEntry
        {
            Message = message,
            LogType = logType,
            TimeStamp = DateTime.UtcNow,
            Username = existingUser?.Username ?? "Unknown"

        };  

        await _context.AuditEntries.AddAsync(entry);    
        await _context.SaveChangesAsync();  
    }

}
