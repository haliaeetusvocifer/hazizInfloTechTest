using System.Collections.Generic;
using Microsoft.Extensions.Logging.Abstractions;
using UserManagement.Models;

namespace UserManagement.Services.Domain.Interfaces;

public interface IUserService
{
    IEnumerable<User> FilterByActive(bool isActive);
    IEnumerable<User> GetAll();
    IEnumerable<User> GetUserID(int id);
    void Create(User user);
    void Delete(User user);
    void Update(User user);
    IEnumerable<LogEntry> GetLogsForUser(int userId);

    void CreateLogEntry(LogEntry logEntry);
}
