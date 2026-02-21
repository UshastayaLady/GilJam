using System.Collections.Generic;
using UnityEngine;

public class PigCollection
{
    private readonly Dictionary<PigModel, PigInfo> _pigModelsByInfos = new();
    
    public void AddPig(PigModel pigModel, PigInfo pigInfo)
    {
        _pigModelsByInfos.Add(pigModel, pigInfo);
    }

    public Sprite GetSpriteBy(PigModel pigModel)
    {
        return _pigModelsByInfos[pigModel].Sprite;
    }    
    
    public GameObject GetPrefabBy(PigModel pigModel)
    {
        return _pigModelsByInfos[pigModel].Prefab;
    }

    public IEnumerable<PigModel> GetPigs()
    {
        return _pigModelsByInfos.Keys;
    }
}

public class PigInfo
{
    public GameObject Prefab;
    public Sprite Sprite;

    public PigInfo(GameObject prefab, Sprite sprite)
    {
        Prefab = prefab;
        Sprite = sprite;
    }
}