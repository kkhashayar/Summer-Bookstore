using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Summer_Bookstore_Domain.Entities;
using Summer_Bookstore_Infrastructure.EventLogs;

public class AuthorSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly AuditLogger _auditLogger;
    readonly IHttpContextAccessor _httpContextAccessor;

    public AuthorSaveChangesInterceptor(AuditLogger auditLogger, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _auditLogger = auditLogger;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        if (context == null) return await base.SavingChangesAsync(eventData, result, cancellationToken);

        string username = "Unknown User";

        if (_httpContextAccessor.HttpContext != null &&
            _httpContextAccessor.HttpContext.User != null &&
            _httpContextAccessor.HttpContext.User.Identity != null &&
            _httpContextAccessor.HttpContext.User.Identity.Name != null)
        {
            username = _httpContextAccessor.HttpContext.User.Identity.Name;
        }


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
                await _auditLogger.LogAsync(message, LogType.Information, );
            }
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
