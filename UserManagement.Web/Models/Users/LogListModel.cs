using System;

namespace UserManagement.Web.Models.Users;

public class LogListViewModel
{
    public List<LogListItemViewModel> Items { get; set; } = new();
}

public class LogListItemViewModel
{
    public long LogId { get; set; }
    public long UserId { get; set; } = default!;
    public string ActionDescription { get; set; } = default!;
    public string? AdditionalDetails { get; set; }
    public DateTime TimeStamp { get; set; }

    public UserListItemViewModel User { get; set; } = default!;
    public List<LogListItemViewModel> Items { get; set; } = new();
}
