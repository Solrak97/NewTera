using System.Collections.Generic;
using System.Linq;
using Godot;


namespace NewTera.Utils;

public static class JsonLoader
{
    public static List<Godot.Collections.Dictionary<string, string>> LoadJsonAsDictionary(string filePath)
    {
        if (!FileAccess.FileExists(filePath))
        {
            GD.PushError($"File not found: {filePath}");
            return [];
        }

        using var file = FileAccess.Open(filePath, FileAccess.ModeFlags.Read);
        if (file == null)
        {
            GD.PushError($"Failed to open file: {filePath}");
            return [];
        }

        var jsonText = file.GetAsText();

        var json = new Json();
        var parseResult = json.Parse(jsonText);

        if (parseResult != Error.Ok) return [];
        if (json.Data.VariantType != Variant.Type.Array) return [];
        var dataList = json.Data.AsGodotArray();
        return (from item in dataList where item.VariantType == Variant.Type.Dictionary select item.AsGodotDictionary<string, string>()).ToList();

    }
}