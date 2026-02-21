using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolObjects : MonoBehaviour
{
    [SerializeField] private SpriteRenderer obgectForPool;    
    [SerializeField] private int countObgects;

    private List<SpriteRenderer> poolObgect;
    private readonly Vector2 pointSpawn = new Vector2(0, 0);

    private void Awake()
    {
        poolObgect = new List<SpriteRenderer>();
        InitializationPoolObjects();
        ClosePoolObjects();
    }

    private void InitializationPoolObjects()
    {
        for (int i = 0; i < countObgects; i++)
        {
            poolObgect.Add(Instantiate(obgectForPool, pointSpawn, Quaternion.identity));
        }
    }

    private void ClosePoolObjects()
    {
        for (int i = 0; i < poolObgect.Count; i++)
        {
            poolObgect[i].GameObject().SetActive(false);
        }
    }
    private void AddPoolObjects()
    {
        poolObgect.Add(Instantiate(obgectForPool, pointSpawn, Quaternion.identity));
        poolObgect[poolObgect.Count - 1].GameObject().SetActive(false);
    }

    public SpriteRenderer GetObjectInPool()
    {
        for (int i = 0; i < poolObgect.Count; i++)
        {
            if (!poolObgect[i].GameObject().activeSelf)
            {
                return poolObgect[i];
            }
        }
        AddPoolObjects();
        return poolObgect[poolObgect.Count - 1];
    }

}