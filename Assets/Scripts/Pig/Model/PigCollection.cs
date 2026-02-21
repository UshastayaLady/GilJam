using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PigCollection
{
    private readonly Dictionary<PigModel, PigInfo> _pigModelsByInfos = new();
    
    private readonly Subject<(PigView, PigModel)> _onSpawned = new Subject<(PigView, PigModel)>();
    
    public IObservable<(PigView, PigModel)> OnSpawned => _onSpawned;

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

    public void UpdateSpawnedPig(PigView spawnedPig, PigModel pigModel)
    {
        _onSpawned?.OnNext((spawnedPig, pigModel));
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