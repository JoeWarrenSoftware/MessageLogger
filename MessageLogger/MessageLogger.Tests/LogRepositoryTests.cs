using MessageLogger.Application.Models;
using MessageLogger.Application.Repositories;

namespace MessageLogger.Tests;
public class LogRepositoryTests
{
    private LogRepository CreateRepository(string fileName)
    {
        DeleteTestFile(fileName);
        return new LogRepository(fileName);
    }

    [Fact]
    public async Task CreateAsync_ShouldWriteLogToFile()
    {
        var fileName = @"Test_LogRepositoryTests_CreateAsync_ShouldWriteLogToFile.txt";
        try
        {
            // Arrange
            var testMessage = "Test Message";
            var repository = CreateRepository(fileName);
            var log = new LogMessage { Id = Guid.NewGuid(), Date = DateTime.UtcNow, Text = testMessage };

            // Act
            await repository.CreateAsync(log);
            var logs = await repository.GetAllAsync();

            // Assert
            Assert.Single(logs);
            Assert.Equal(testMessage, logs.First().Text);
        }
        finally
        {
            DeleteTestFile(fileName);
        }
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCorrectLog()
    {
        var fileName = @"Test_LogRepositoryTests_GetByIdAsync_ShouldReturnCorrectLog.txt";
        try
        {
            // Arrange
            var repository = CreateRepository(fileName);
            var log = new LogMessage { Id = Guid.NewGuid(), Date = DateTime.UtcNow, Text = "Find Me" };
            await repository.CreateAsync(log);

            // Act
            var retrievedLog = await repository.GetByIdAsync(log.Id);

            // Assert
            Assert.NotNull(retrievedLog);
            Assert.Equal(log.Id, retrievedLog.Id);
        }
        finally
        {
            DeleteTestFile(fileName);
        }
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnCorrectLogs()
    {
        var fileName = @"Test_LogRepositoryTests_GetAllAsync_ShouldReturnCorrectLogs.txt";
        try
        {
            // Arrange
            var firstMessage = "First Message";
            var secondMessage = "Second Message";
            var thirdMessage = "Third Message";

            var repository = CreateRepository(fileName);
            var log1 = new LogMessage { Id = Guid.NewGuid(), Date = DateTime.UtcNow, Text = firstMessage };
            var log2 = new LogMessage { Id = Guid.NewGuid(), Date = DateTime.UtcNow, Text = secondMessage };
            var log3 = new LogMessage { Id = Guid.NewGuid(), Date = DateTime.UtcNow, Text = thirdMessage };
            await repository.CreateAsync(log1);
            await repository.CreateAsync(log2);
            await repository.CreateAsync(log3);

            // Act
            var retrievedLogs = (await repository.GetAllAsync()).ToList();

            // Assert
            Assert.NotNull(retrievedLogs);
            Assert.Equal(3, retrievedLogs.Count);
            Assert.Contains(retrievedLogs, l => l.Id == log1.Id && l.Text == firstMessage);
            Assert.Contains(retrievedLogs, l => l.Id == log2.Id && l.Text == secondMessage);
            Assert.Contains(retrievedLogs, l => l.Id == log3.Id && l.Text == thirdMessage);
        }
        finally
        {
            DeleteTestFile(fileName);
        }
    }

    private void DeleteTestFile(string fileName)
    {
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }
    }
}
