using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging.Abstractions;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Services.Domain.Implementations;
// LoggerService.cs 
public class LoggerService : ILogService
{
    private readonly IDataContext _dataAccess;
    public LoggerService(IDataContext dataAccess) => _dataAccess = dataAccess;

    public void Create(LogEntry logEntry) => _dataAccess.Create(logEntry);
    public void Update(LogEntry logEntry) => _dataAccess.Create(logEntry);
    public void Delete(LogEntry logEntry) => _dataAccess.Create(logEntry);
    public IEnumerable<LogEntry> FilterLogs(string criteria) => _dataAccess.GetAll<LogEntry>().Where(y => y.ActionDescription.Contains(criteria));
    public IEnumerable<LogEntry> GetAllLogs() => _dataAccess.GetAll<LogEntry>();
    public IEnumerable<LogEntry> GetLogs(int id) => _dataAccess.GetAll<LogEntry>().Where(x => x.LogId == id);
}
