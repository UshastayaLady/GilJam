using System;
using UnityEngine;

[Serializable]
public class PigData : AbstractData
{
    public string Name;
    public GameObject Prefab;
    public Sprite Sprite;
    public int Price;
    public float Speed;
    public float Damage;
}
