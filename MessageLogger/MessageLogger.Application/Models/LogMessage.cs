using System.ComponentModel.DataAnnotations;

namespace MessageLogger.Application.Models;
public class LogMessage
{
    public required Guid Id { get; init; }

    public required DateTime Date { get; set; }

    [MaxLength(255, ErrorMessage = "Text cannot exceed 255 characters.")]
    public required string Text { get; set; }
}