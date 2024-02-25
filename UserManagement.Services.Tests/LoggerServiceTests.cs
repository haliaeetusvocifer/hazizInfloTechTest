using System;
using System.Collections.Generic;
using System.Linq;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Implementations;


namespace UserManagement.Services.Tests
{
    public class LoggerServiceTests
    {
        private readonly Mock<IDataContext> _dataContext = new();
        private LoggerService CreateService() => new(_dataContext.Object);

        [Fact]
        public void Create_ShouldAddNewLogEntry()
        {
            // Arrange
            var newLogEntry = new LogEntry(1, "User created", DateTime.Now);
            var mockDataContext = new Mock<IDataContext>();
            var loggerService = new LoggerService(mockDataContext.Object);

            // Act
            loggerService.Create(newLogEntry);

            // Assert
            mockDataContext.Verify(x => x.Create(It.Is<LogEntry>(log =>
                  log.UserId == newLogEntry.UserId &&
                  log.ActionDescription == newLogEntry.ActionDescription &&
                  log.Timestamp == newLogEntry.Timestamp)), Times.Once());
        }
        [Fact]
        public void FilterLogs_ShouldReturnLogsMatchingCriteria()
        {
            // Arrange
            var logEntries = new List<LogEntry>
    {
        new LogEntry(1, "User created", DateTime.Now),
        new LogEntry(2, "User deleted", DateTime.Now),
        new LogEntry(1, "Action unrelated", DateTime.Now)
    };
            var criteria = "deleted";
            var mockDataContext = new Mock<IDataContext>();
            mockDataContext.Setup(x => x.GetAll<LogEntry>()).Returns(logEntries.AsQueryable());
            var loggerService = new LoggerService(mockDataContext.Object);

            // Act
            var filteredLogs = loggerService.FilterLogs(criteria);

            // Assert
            filteredLogs.Should().HaveCount(1);
            filteredLogs.First().ActionDescription.Should().Be("User deleted");
        }
    }

}
