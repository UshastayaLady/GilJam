using UnityEngine;
using System.Collections;

public class RandSpawn : SpavnParent
{

    [SerializeField] private float minRandTime;
    [SerializeField] private float maxRandTime;
    [SerializeField] private SpriteRenderer spawnPrefabView;

    [Header("Проверка зоны спавна")]
    [SerializeField] private LayerMask spawnBlockMask; // слой объектов, которые нельзя пересекать
    [SerializeField] private Vector2 extraPadding = Vector2.zero; // небольшой запас

    private float spawnTime;

    private void Start()
    {
        StartCoroutine(RandSpawnLoop());
    }

    private Vector2 RandVector()
    {
        Vector3 squarePos = spawnSquare.transform.position;
        Vector3 squareScale = spawnSquare.transform.localScale;
        
        Vector2 prefabSize = spawnPrefabView.bounds.size;

        float minX = squarePos.x - squareScale.x / 2f + prefabSize.x / 2f;
        float maxX = squarePos.x + squareScale.x / 2f - prefabSize.x / 2f;
        float minY = squarePos.y - squareScale.y / 2f + prefabSize.y / 2f;
        float maxY = squarePos.y + squareScale.y / 2f - prefabSize.y / 2f;

        Vector2 checkSize = prefabSize + extraPadding;

        Vector2 candidate = new Vector2(
                Random.Range(minX, maxX),
                Random.Range(minY, maxY)
            );

        
        Collider2D hit = Physics2D.OverlapBox(candidate, checkSize, 0f, spawnBlockMask);

        if (hit == null)
            return candidate;

        Debug.LogWarning("Не найдено свободное место для спавна.");
        return spawnPoint;
    }

    private IEnumerator RandSpawnLoop()
    {
        while (true)
        {
            spawnTime = Random.Range(minRandTime, maxRandTime);

            spawnPoint = RandVector();
            SpawnObject(spawnPoint);

            yield return new WaitForSeconds(spawnTime);
        }
    }
}
