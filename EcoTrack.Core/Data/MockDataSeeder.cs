using EcoTrack.Core.Models;

namespace EcoTrack.Core.Data;

/// <summary>
/// Seeds the database with mock assets and telemetry records for development/demo purposes.
/// </summary>
public static class MockDataSeeder
{
    public static void Seed(AppDbContext context)
    {
        // Only seed if the database is empty
        if (context.Assets.Any())
            return;

        var now = DateTime.UtcNow;

        var assets = new List<Asset>
        {
            new()
            {
                Name = "Solar Panel Array A",
                SerialNumber = "SP-2025-001",
                Status = "Active",
                CreatedAt = now.AddDays(-90)
            },
            new()
            {
                Name = "Wind Turbine North",
                SerialNumber = "WT-2025-002",
                Status = "Active",
                CreatedAt = now.AddDays(-85)
            },
            new()
            {
                Name = "Hydroelectric Generator 3",
                SerialNumber = "HG-2025-003",
                Status = "Maintenance",
                CreatedAt = now.AddDays(-80)
            },
            new()
            {
                Name = "Battery Storage Unit B1",
                SerialNumber = "BS-2025-004",
                Status = "Active",
                CreatedAt = now.AddDays(-75)
            },
            new()
            {
                Name = "Smart Meter Gateway",
                SerialNumber = "SM-2025-005",
                Status = "Inactive",
                CreatedAt = now.AddDays(-70)
            },
            new()
            {
                Name = "Geothermal Pump Station 2",
                SerialNumber = "GP-2025-006",
                Status = "Active",
                CreatedAt = now.AddDays(-60)
            },
            new()
            {
                Name = "Cooling Tower West",
                SerialNumber = "CT-2025-007",
                Status = "Maintenance",
                CreatedAt = now.AddDays(-50)
            },
            new()
            {
                Name = "Biomass Boiler Unit",
                SerialNumber = "BB-2025-008",
                Status = "Active",
                CreatedAt = now.AddDays(-45)
            }
        };

        context.Assets.AddRange(assets);
        context.SaveChanges();

        // Generate telemetry records for each asset
        var random = new Random(42); // Fixed seed for reproducible mock data
        var metricNames = new[] { "Temperature_C", "Humidity_Pct", "PowerOutput_kW", "Voltage_V", "Current_A", "RPM", "FlowRate_Lmin", "Pressure_Bar" };

        var telemetryRecords = new List<TelemetryRecord>();

        foreach (var asset in assets)
        {
            // Generate 50 records per asset spanning the last 30 days
            for (int i = 0; i < 50; i++)
            {
                var metricName = metricNames[random.Next(metricNames.Length)];
                var metricValue = metricName switch
                {
                    "Temperature_C" => (random.NextDouble() * 60 - 10).ToString("F1"),
                    "Humidity_Pct" => (random.NextDouble() * 100).ToString("F0"),
                    "PowerOutput_kW" => (random.NextDouble() * 500).ToString("F2"),
                    "Voltage_V" => (random.NextDouble() * 480).ToString("F1"),
                    "Current_A" => (random.NextDouble() * 100).ToString("F2"),
                    "RPM" => (random.NextDouble() * 3000).ToString("F0"),
                    "FlowRate_Lmin" => (random.NextDouble() * 200).ToString("F1"),
                    "Pressure_Bar" => (random.NextDouble() * 10).ToString("F2"),
                    _ => "0"
                };

                telemetryRecords.Add(new TelemetryRecord
                {
                    AssetId = asset.Id,
                    Timestamp = now.AddDays(-30).AddSeconds(i * 50000 + random.Next(1000)),
                    MetricName = metricName,
                    MetricValue = metricValue
                });
            }
        }

        context.TelemetryRecords.AddRange(telemetryRecords);
        context.SaveChanges();
    }
}