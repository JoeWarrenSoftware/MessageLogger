using MessageLogger.Api.Controllers;
using MessageLogger.Application.Models;
using MessageLogger.Application.Repositories;
using MessageLogger.Contracts.Requests;
using MessageLogger.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace MessageLogger.Tests;
public class LogsControllerTests
{
    private readonly Mock<ILogRepository> _mockRepo;
    private readonly Mock<ILogger<LogsController>> _mockLogger;
    private readonly LogsController _controller;

    public LogsControllerTests()
    {
        _mockRepo = new Mock<ILogRepository>();
        _mockLogger = new Mock<ILogger<LogsController>>();
        _controller = new LogsController(_mockRepo.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Create_ShouldReturn201Created_WhenValidRequest()
    {
        // Arrange
        var request = new CreateLogRequest { Text = "Valid log message" };
        _mockRepo.Setup(repo => repo.CreateAsync(It.IsAny<LogMessage>())).ReturnsAsync(true);

        // Act
        var result = await _controller.Create(request);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(201, createdResult.StatusCode);
        _mockRepo.Verify(repo => repo.CreateAsync(It.IsAny<LogMessage>()), Times.Once);
    }

    [Fact]
    public async Task Create_ShouldReturn400BadRequest_WhenTextIsTooLong()
    {
        // Arrange
        var request = new CreateLogRequest { Text = new string('A', 256) }; // 256 characters

        // Act
        var result = await _controller.Create(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task Get_ShouldReturn200OK_WhenLogExists()
    {
        // Arrange
        var logId = Guid.NewGuid();
        var log = new LogMessage { Id = logId, Date = DateTime.UtcNow, Text = "Found Log" };

        _mockRepo.Setup(repo => repo.GetByIdAsync(logId)).ReturnsAsync(log);

        // Act
        var result = await _controller.Get(logId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);

        var returnedLog = Assert.IsType<LogResponse>(okResult.Value);
        Assert.Equal(log.Text, returnedLog.Text);

        _mockRepo.Verify(repo => repo.GetByIdAsync(logId), Times.Once);
    }

    [Fact]
    public async Task Get_ShouldReturn404NotFound_WhenLogDoesNotExist()
    {
        // Arrange
        var logId = Guid.NewGuid();
        _mockRepo.Setup(repo => repo.GetByIdAsync(logId)).ReturnsAsync((LogMessage)null);

        // Act
        var result = await _controller.Get(logId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetAll_ShouldReturn200OK_WithAllLogs()
    {
        // Arrange
        var logs = new[]
        {
            new LogMessage { Id = Guid.NewGuid(), Date = DateTime.UtcNow, Text = "Log 1" },
            new LogMessage { Id = Guid.NewGuid(), Date = DateTime.UtcNow, Text = "Log 2" }
        };

        _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(logs);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);

        var returnedLogs = Assert.IsType<LogsResponse>(okResult.Value);
        Assert.Equal(2, returnedLogs.Items.Count());
    }
}
