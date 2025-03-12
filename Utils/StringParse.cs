using Godot;

namespace NewTera.Utils;

public static class StringParse
{
    public static Color StringToColor(string color)
    {
        var cleanInput = color.Trim('[', ']');
        
        var components = cleanInput.Split(',');
        
        var r = float.Parse(components[0].Trim());
        var g = float.Parse(components[1].Trim());
        var b = float.Parse(components[2].Trim());
        
        return new Color(r, g, b);
    }
}