using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ListObjects : MonoBehaviour
{
    [SerializeField] private Sprite obgectForPool;    
    [SerializeField] private int countObgects;

    private List<Sprite> poolObgect;
    private readonly Vector2 pointSpawn = new Vector2(0, 0);

    private void Awake()
    {
        InitializationPoolObjects();
        ClosePoolObjects();
    }

    private void InitializationPoolObjects()
    {
        for (int i=0; i < countObgects; i++)
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
        poolObgect[poolObgect.Count-1].GameObject().SetActive(false);
    }

    public Sprite GetObjectInPool()
    {
        for (int i = 0; i < poolObgect.Count; i++)
        {
            if (poolObgect[i].GameObject().activeSelf)
            {
                return poolObgect[i];
            }  
        }
        AddPoolObjects();
        return poolObgect[poolObgect.Count - 1];
    }

}