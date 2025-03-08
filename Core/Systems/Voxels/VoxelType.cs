using System;
using System.Collections.Generic;
using Godot;

namespace NewTera.Core.Systems.Voxels;

public class VoxelType(int id, string name, bool isSolid, Color color)
{
    public int Id { get; } = id;
    public string Name { get; } = name;
    public bool IsSolid { get; } = isSolid;
    public Color Color { get; } = color;

    public static VoxelType FromJson(Dictionary<string, object> data, int autoId)
    {
        var name = data["name"].ToString();
        var isSolid = bool.Parse(data["is_solid"].ToString());

        var colorArray = (List<object>)data["color"];
        var color = new Color(
            Convert.ToSingle(colorArray[0]),
            Convert.ToSingle(colorArray[1]),
            Convert.ToSingle(colorArray[2])
        );

        return new VoxelType(autoId, name, isSolid, color);
    }
}