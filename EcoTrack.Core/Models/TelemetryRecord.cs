using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoTrack.Core.Models;

public class TelemetryRecord
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int AssetId { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    [Required]
    [MaxLength(100)]
    public string MetricName { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string MetricValue { get; set; } = string.Empty;

    // Navigation property
    [ForeignKey(nameof(AssetId))]
    public Asset Asset { get; set; } = null!;
}