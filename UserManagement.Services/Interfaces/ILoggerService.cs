using System.Collections.Generic;
using Microsoft.Extensions.Logging.Abstractions;
using UserManagement.Models;

namespace UserManagement.Services.Domain.Interfaces;
public interface ILogService
{
    IEnumerable<LogEntry> GetAllLogs();
    IEnumerable<LogEntry> GetLogs(int id);
    IEnumerable<LogEntry> FilterLogs(string criteria);
    void Create(LogEntry logEntry);
    void Delete(LogEntry logEntry);
    void Update(LogEntry logEntry);


}
