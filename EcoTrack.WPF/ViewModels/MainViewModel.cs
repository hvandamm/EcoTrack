using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EcoTrack.Core.Data;
using EcoTrack.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace EcoTrack.WPF.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly AppDbContext _context;

    public MainViewModel(AppDbContext context)
    {
        _context = context;
    }

    [ObservableProperty]
    private ObservableCollection<Asset> _assets = new();

    [ObservableProperty]
    private Asset? _selectedAsset;

    [ObservableProperty]
    private ObservableCollection<TelemetryRecord> _telemetryRecords = new();

    [RelayCommand]
    private async Task LoadDataAsync()
    {
        var assets = await _context.Assets
            .AsNoTracking()
            .Include(a => a.TelemetryRecords)
            .OrderBy(a => a.Name)
            .ToListAsync();

        Assets = new ObservableCollection<Asset>(assets);

        if (SelectedAsset is not null)
        {
            // Refresh selected asset from the new list
            SelectedAsset = Assets.FirstOrDefault(a => a.Id == SelectedAsset.Id);
            UpdateTelemetryRecordsFromSelected();
        }
    }

    partial void OnSelectedAssetChanged(Asset? value)
    {
        UpdateTelemetryRecordsFromSelected();
    }

    private void UpdateTelemetryRecordsFromSelected()
    {
        if (SelectedAsset is null)
        {
            TelemetryRecords = new ObservableCollection<TelemetryRecord>();
            return;
        }

        var records = SelectedAsset.TelemetryRecords
            .OrderByDescending(tr => tr.Timestamp)
            .ToList();

        TelemetryRecords = new ObservableCollection<TelemetryRecord>(records);
    }
}