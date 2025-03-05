using Godot;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;


namespace NewTera.Core.Systems.Voxels;

public static class VoxelRegistry
{
    private static readonly Dictionary<int, VoxelType> VoxelMap = new();
    private static readonly Dictionary<string, VoxelType> VoxelNames = new();
    private static int _nextId = 0;

    public static readonly VoxelType AIR = Register(new VoxelType(0, "Air", false, Colors.Transparent));

    private static VoxelType Register(VoxelType voxel)
    {
        VoxelMap[voxel.Id] = voxel;
        VoxelNames[voxel.Name.ToUpper()] = voxel;
        _nextId = voxel.Id + 1; // Ensure ID keeps increasing
        return voxel;
    }

    public static void LoadVoxels(string jsonPath)
    {
        if (!File.Exists(jsonPath))
        {
            GD.PrintErr($"Voxel JSON file not found: {jsonPath}");
            return;
        }

        var jsonText = File.ReadAllText(jsonPath);
        var voxelList = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(jsonText);

        foreach (var voxel in voxelList.Select(voxelData => VoxelType.FromJson(voxelData, _nextId)))
        {
            Register(voxel);
        }

        GD.Print($"Loaded {VoxelMap.Count} voxel types.");
    }

    public static VoxelType GetVoxel(int id) => VoxelMap.GetValueOrDefault(id, AIR);

    public static VoxelType GetVoxel(string name) =>
        VoxelNames.ContainsKey(name.ToUpper()) ? VoxelNames[name.ToUpper()] : AIR;
}