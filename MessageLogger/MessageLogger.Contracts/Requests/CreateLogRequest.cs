using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MessageLogger.Contracts.Requests;
public class CreateLogRequest
{
    [Required]
    [MaxLength(255, ErrorMessage = "Text cannot exceed 255 characters.")]
    public required string Text { get; init; }
}