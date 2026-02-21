using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PoolObjects))]
public class SpavnParent : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer spawnSquare;       
    protected Vector2 spawnPoint;
    private SpriteRenderer spawnObject;
    private PoolObjects poolObjects;

    private void Awake()
    {
        poolObjects = GetComponent<PoolObjects>();
    }

    protected void SpawnObject(Vector2 spawnPoint)
    {
        spawnObject = poolObjects.GetObjectInPool();
        spawnObject.GameObject().transform.position = spawnPoint;
        spawnObject.GameObject().SetActive(true);
    }
}
