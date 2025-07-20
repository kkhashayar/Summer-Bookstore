using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Summer_Bookstore_Domain.Entities;
using Summer_Bookstore_Infrastructure.Data;
using Summer_Bookstore_Infrastructure.EventLogs;

public class AuthorSaveChangesInterceptor : SaveChangesInterceptor
{
    //private readonly AuditLogger _auditLogger;
    //readonly IHttpContextAccessor _httpContextAccessor;

    //public AuthorSaveChangesInterceptor(AuditLogger auditLogger, IHttpContextAccessor httpContextAccessor)
    //{
    //    _httpContextAccessor = httpContextAccessor;
    //    _auditLogger = auditLogger;
    //}

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;

        // Skip if we're saving AuditEntry itself, to prevent recursion
        if (context?.ChangeTracker?.Entries<AuditEntry>().Any() == true &&
        context.ChangeTracker.Entries().All(e => e.Entity is AuditEntry))
        {
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        // Manually getting the username from ClaimsPrincipal if available if not will set it to "Unknown User" 
        string username = "Unknown User";

        var httpContext = new HttpContextAccessor().HttpContext;   

        if(httpContext is not null)
        {
            var user = httpContext.User;
            if(user is not null && user.Identity is not null && user.Identity.IsAuthenticated)
            {
                username = user.Identity.Name;  
            }
        }

        // Creating temporary context 
        var options = new DbContextOptionsBuilder<BookstoreDbContext>()
            .UseSqlServer("Server=localhost;Database=Summer_Bookstore_Db;Trusted_Connection=True;TrustServerCertificate=True;")
            .Options;

        await using var auditContext = new BookstoreDbContext(options); 

        foreach (var entry in context.ChangeTracker.Entries<Author>())
        {
            string message = null;
            switch (entry.State)
            {
                case EntityState.Added:
                    message = $"New author added: {entry.Entity.Name}";
                    break;
                case EntityState.Modified:
                    message = $"Author updated: {entry.Entity.Name}";
                    break;
                case EntityState.Deleted:
                    message = $"Author deleted: {entry.Entity.Name}";
                    break;
            }

            if (!string.IsNullOrEmpty(message))
            {
                // Log directly to database using the new context
                var audit = new AuditEntry
                {
                    LogType = LogType.Information,
                    Message = message,  
                    Username = username,
                    TimeStamp = DateTime.UtcNow

                };
                await auditContext.AuditEntries.AddAsync(audit, cancellationToken);
                await auditContext.SaveChangesAsync(cancellationToken);
            }
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
