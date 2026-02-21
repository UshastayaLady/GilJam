using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class RandSpawn : SpavnParent
{

    [SerializeField] private float minRandTime;
    [SerializeField] private float maxRandTime;
    private float spawnTime;
    [SerializeField] private SpriteRenderer spawnPrefabView;

    private void Start()
    {
        StartCoroutine(RandSpawnLoop());
    }

    private Vector2 RandVector()
    {
        float transformX = Random.Range(
            spawnSquare.GameObject().transform.position.x - spawnSquare.GameObject().transform.localScale.x/2 + spawnPrefabView.GameObject().transform.localScale.x / 2, 
            spawnSquare.GameObject().transform.position.x + spawnSquare.GameObject().transform.localScale.x / 2 - spawnPrefabView.GameObject().transform.localScale.x / 2);
        
        float transformY = Random.Range(
            spawnSquare.GameObject().transform.position.y - spawnSquare.GameObject().transform.localScale.y / 2 + spawnPrefabView.GameObject().transform.localScale.y / 2, 
            spawnSquare.GameObject().transform.position.y + spawnSquare.GameObject().transform.localScale.y / 2 - spawnPrefabView.GameObject().transform.localScale.y / 2);
        return new Vector2(transformX, transformY);
    }

    IEnumerator RandSpawnLoop()
    {
        while(true)
        {
            spawnTime = Random.Range(minRandTime, maxRandTime);

            spawnPoint = RandVector();
            SpawnObject(spawnPoint);

            yield return new WaitForSeconds(spawnTime);
        }
    }
}
