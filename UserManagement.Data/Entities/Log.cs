using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Models;
public class LogEntry
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long LogId { get; set; }
    public long UserId { get; set; }
    public string ActionDescription { get; set; } = default!;
    public string AdditionalDetails { get; set; } = default!;
    public DateTime Timestamp { get; set; }

    public LogEntry(long userId, string actionDescription, DateTime timestamp)
    {
        UserId = userId;
        ActionDescription = actionDescription;
        Timestamp = timestamp;
    }
    public LogEntry(long userId, string actionDescription, string actionDetails, DateTime timeStamp)
    {
        UserId = userId;
        ActionDescription = actionDescription;
        AdditionalDetails = actionDetails;
        Timestamp = timeStamp;
    }
}
