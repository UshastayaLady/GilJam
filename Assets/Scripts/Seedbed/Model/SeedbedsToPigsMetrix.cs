using System.Collections.Generic;
using UnityEngine;

public static class SeedbedsToPigsMetrix
{
    private static Dictionary<(string, string), bool> _dictionary =
        new Dictionary<(string, string), bool>()
        {
            { ("BrickPig", "Carrot"), false },
            { ("StrawPig", "Carrot"), false },
            { ("WoodcutterPig", "Carrot"), true },
            { ("BrickPig", "Cucumber"), false },
            { ("StrawPig", "Cucumber"), true },
            { ("WoodcutterPig", "Cucumber"),  false},
            { ("BrickPig", "Pepper"), true },
            { ("StrawPig", "Pepper"), false },
            { ("WoodcutterPig", "Pepper"),  false},
        };

    public static bool IsAbleToEat(string pigType, string seedbedType)
    {
        if (_dictionary.TryGetValue((pigType, seedbedType), out bool result))
        {
            return result;
        }
        
        Debug.LogWarning($"No entry found for pig: {pigType}, seedbed: {seedbedType}");
        return false;
    }
}