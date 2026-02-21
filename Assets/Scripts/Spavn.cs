using UnityEngine;

[RequireComponent(typeof(ListObjects))]
public class Spavn : MonoBehaviour
{
    [SerializeField] private Sprite spawnSquare;
    private Sprite spawnObject;   
    private Vector3 spawnPoint;
    private ListObjects listObjects;

    private void Awake()
    {
        listObjects = GetComponent<ListObjects>();
    }

    private void SpawnObject(Vector3 spawnPoint)
    {
        
    }
}
