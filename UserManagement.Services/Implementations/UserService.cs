using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Services.Domain.Implementations;

public class UserService : IUserService
{
    private readonly IDataContext _dataAccess;
    public UserService(IDataContext dataAccess) => _dataAccess = dataAccess;
    public IEnumerable<User> FilterByActive(bool isActive) => _dataAccess.GetAll<User>().Where(u => u.IsActive == isActive);

    public IEnumerable<User> GetAll() => _dataAccess.GetAll<User>();
    public void Create(User user) => _dataAccess.Create(user);

    public void Delete(User user) => _dataAccess.Delete(user);
    public void Update(User user) => _dataAccess.Update(user);
    public IEnumerable<User> GetUser(int id) => _dataAccess.GetAll<User>().Where(w => w.Id == id);
    public IEnumerable<User> GetUserID(int id) => _dataAccess.GetAll<User>().AsNoTracking().Where(w => w.Id == id);
    public IEnumerable<LogEntry> GetLogsForUser(int userId) => _dataAccess.GetAll<LogEntry>().Where(w => w.UserId == userId);
    public void CreateLogEntry(LogEntry logEntry) => _dataAccess.Create(logEntry);
}
