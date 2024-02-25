using System;
using System.Linq;
using System.Text;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService) => _userService = userService;
    [BindProperty]
    public User newSaveEditUser { get; set; } = default!;
    [HttpGet]
    public ViewResult List(bool showActiveOnly = false, bool showNonActiveOnly = false)
    {
        IEnumerable<User> users;
        if (showActiveOnly)
        {
            users = _userService.FilterByActive(true);
        }
        else if (showNonActiveOnly)
        {
            users = _userService.FilterByActive(false);
        }
        else
        {
            users = _userService.GetAll(); // Default if no filter
        }
        var items = users.Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            IsActive = p.IsActive,
            DateofBirth = p.DateofBirth
        });

        var model = new UserListViewModel
        {
            Items = items.ToList()
        };

        return View(model);
    }
    [HttpGet("Add")]
    public IActionResult Add()
    {
        return View();
    }

    [HttpPost("Add")]
    public IActionResult Create(User user)
    {
        if (ModelState.IsValid)
        {
            var logEntry = new LogEntry(newSaveEditUser.Id, $"User {newSaveEditUser.Forename} {newSaveEditUser.Surname} [Id: {newSaveEditUser.Id}] created", DateTime.Now);
            _userService.Create(user);
            return RedirectToAction("List");
        }
        return View(user);
    }
    [HttpGet("View/{userId}")]
    public ViewResult View(int userId)
    {
        var user = _userService.GetUserID(userId).Select(u => new UserListItemViewModel
        {
            Id = u.Id,
            Forename = u.Forename,
            Surname = u.Surname,
            Email = u.Email,
            IsActive = u.IsActive,
            DateofBirth = u.DateofBirth
        }).First();

        return View(user);
    }
    [HttpGet("Edit/{userId}")]
    public ViewResult Edit(int userId)
    {
        var user = _userService.GetUserID(userId).First();

        return View(user);
    }

    [HttpPost("Edit/{userId}")]
    public ActionResult SaveEdit(int userId)
    {
        var logEntry = new LogEntry(userId, $"User {newSaveEditUser.Forename} {newSaveEditUser.Surname} [Id: {userId}] updated", BuildLogEntryDetails(userId), DateTime.Now);
        _userService.CreateLogEntry(logEntry);

        newSaveEditUser.Id = userId;
        _userService.Update(newSaveEditUser);

        return RedirectToAction("List");
    }
    [HttpGet("Delete/{userId}")]
    public ViewResult Delete(int userId)
    {
        var user = _userService.GetUserID(userId).First();

        return View(user);
    }

    [HttpPost("Delete/{userId}")]
    public ActionResult UserDeleted(int userId)
    {
        var user = _userService.GetUserID(userId).First();
        _userService.Delete(user);
        var logEntry = new LogEntry(userId, $"User {user.Forename} {user.Surname} [Id: {userId}] deleted", DateTime.Now);
        _userService.CreateLogEntry(logEntry);
        return RedirectToAction("List");
    }
    private string BuildLogEntryDetails(int userId)
    {
        var log = new StringBuilder();

        var oldUser = _userService.GetUserID(userId).First();

        if (newSaveEditUser.Forename != oldUser.Forename)
        {
            log.AppendLine($"Forename changed from {oldUser.Forename} to {newSaveEditUser.Forename}");
        }
        if (newSaveEditUser.Surname != oldUser.Surname)
        {
            log.AppendLine($"Surname changed from {oldUser.Surname} to {newSaveEditUser.Surname}");
        }
        if (newSaveEditUser.Email != oldUser.Email)
        {
            log.AppendLine($"Email changed from {oldUser.Email} to {newSaveEditUser.Email}");
        }
        if (newSaveEditUser.IsActive != oldUser.IsActive)
        {
            log.AppendLine($"Account Active changed from {(oldUser.IsActive ? "Yes" : "No")} to {(newSaveEditUser.IsActive ? "Yes" : "No")}");
        }
        if (newSaveEditUser.DateofBirth != oldUser.DateofBirth)
        {
            log.AppendLine($"Date of Birth changed from {oldUser.DateofBirth.ToString("dd/MM/yyyy")} to {newSaveEditUser.DateofBirth.ToString("dd/MM/yyyy")}");
        }

        return log.ToString();
    }
}
