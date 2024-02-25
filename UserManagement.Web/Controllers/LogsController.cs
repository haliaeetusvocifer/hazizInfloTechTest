using System.Linq;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;

namespace UserManagement.Web.Controllers;
[Route("logs")]
public class LogsController : Controller
{
    private readonly ILogService _logService;
    private readonly IUserService _userService;
    public LogsController(ILogService logService, IUserService userService)
    {
        _logService = logService;
        _userService = userService;
    }
    [HttpGet("LogsList")]
    public ViewResult LogsList()
    {
        var items = _logService.GetAllLogs().Select(p => new LogListItemViewModel
        {
            LogId = p.LogId,
            UserId = p.UserId,
            ActionDescription = p.ActionDescription,
            AdditionalDetails = p.AdditionalDetails,
            TimeStamp = p.Timestamp
        });

        var model = new LogListViewModel
        {
            Items = items.ToList()
        };

        return View(model);
    }
    [HttpGet("LogEntry/{logId}")]
    public ViewResult ViewLogs(int logId)
    {
        var logEntry = _logService.GetLogs(logId).Select(t => new LogListItemViewModel
        {
            LogId = t.LogId,
            UserId = t.UserId,
            ActionDescription = t.ActionDescription,
            AdditionalDetails = t.AdditionalDetails,
            TimeStamp = t.Timestamp
        }).First();

        var userList = _userService.GetUserID((int)logEntry.UserId).Select(t => new UserListItemViewModel
        {
            Id = t.Id,
            Forename = t.Forename,
            Surname = t.Surname,
            Email = t.Email,
            IsActive = t.IsActive,
            DateofBirth = t.DateofBirth
        });

        if (userList.Any())
        {
            logEntry.User = userList.First();
        }

        return View(logEntry);
    }
    [HttpGet("FilterLogs/{criteria}")]
    public ViewResult FilterLogs(string criteria)
    {
        var items = _logService.FilterLogs(criteria).Select(p => new LogListItemViewModel
        {
            LogId = p.LogId,
            UserId = p.UserId,
            ActionDescription = p.ActionDescription,
            AdditionalDetails = p.AdditionalDetails,
            TimeStamp = p.Timestamp
        });

        var model = new LogListViewModel
        {
            Items = items.ToList()
        };

        return View(model);
    }

}
