using System;
using Godot;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using NewTera.Utils;


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
        _nextId = voxel.Id + 1;
        return voxel;
    }

    public static void LoadVoxels(string jsonPath)
    {
        try
        {
            var voxelList = JsonLoader.LoadJsonAsDictionary(jsonPath);
        
            foreach (var voxelEntry in voxelList)
            {
                var name = voxelEntry["Name"];
                var solid = bool.Parse(voxelEntry["IsSolid"]);
                var color = StringParse.StringToColor(voxelEntry["Color"]);
                
                Register(new VoxelType(_nextId, name, solid, color));
            }

            GD.Print($"Loaded {VoxelMap.Count} voxel types.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Unable to load Voxel File: {e}");
            throw;
        }
    }

    public static VoxelType GetVoxel(int id) => VoxelMap.GetValueOrDefault(id, AIR);

    public static VoxelType GetVoxel(string name) =>
        VoxelNames.ContainsKey(name.ToUpper()) ? VoxelNames[name.ToUpper()] : AIR;

    public static VoxelType GetRandomVoxel()
    {
        var keys = new List<string>(VoxelNames.Keys);
        var randomKey = keys[GD.RandRange(0, keys.Count - 1)];
        return VoxelNames[randomKey];
    }
}