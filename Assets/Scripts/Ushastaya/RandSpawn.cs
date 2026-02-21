using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class RandSpawn : SpavnParent
{

    [SerializeField] private float minRandTime;
    [SerializeField] private float maxRandTime;
    [SerializeField] private List<Vector2> objectsStart;

    [Header("Проверка зоны спавна")]
    [SerializeField] private LayerMask spawnBlockMask; // слой объектов, которые нельзя пересекать
    [SerializeField] private Vector2 extraPadding = Vector2.zero; // небольшой запас
    [SerializeField] private int maxFindAttempts = 20;

    private float spawnTime;

    private void Start()
    {
        StartSpawn();
        StartCoroutine(RandSpawnLoop());
    }

    private void StartSpawn()
    {
        foreach (Vector2 t in objectsStart)
        {
            SpawnObject(t);
        }
    }

    private bool TryGetRandomSpawnPoint(out Vector2 result)
    {
        Vector3 squarePos = spawnSquare.transform.position;

        Vector2 squareSize = spawnSquare.bounds.size;
        Vector2 prefabSize = spawnPrefabView.bounds.size;

        float minX = squarePos.x - squareSize.x / 2f + prefabSize.x / 2f;
        float maxX = squarePos.x + squareSize.x / 2f - prefabSize.x / 2f;
        float minY = squarePos.y - squareSize.y / 2f + prefabSize.y / 2f;
        float maxY = squarePos.y + squareSize.y / 2f - prefabSize.y / 2f;

        if (minX > maxX || minY > maxY)
        {
            Debug.LogWarning("Зона спавна слишком маленькая для объекта.");
            result = Vector2.zero;
            return false;
        }

        Vector2 checkSize = prefabSize + extraPadding;

        for (int attempt = 0; attempt < maxFindAttempts; attempt++)
        {
            Vector2 candidate = new Vector2(
                Random.Range(minX, maxX),
                Random.Range(minY, maxY)
            );

            Collider2D hit = Physics2D.OverlapBox(candidate, checkSize, 0f, spawnBlockMask);

            if (hit == null)
            {
                result = candidate;
                return true;
            }
        }

        result = Vector2.zero;
        return false;
    }

    private IEnumerator RandSpawnLoop()
    {
        while (true)
        {
            spawnTime = Random.Range(minRandTime, maxRandTime);
            yield return new WaitForSeconds(spawnTime);

            if (TryGetRandomSpawnPoint(out Vector2 point))
            {
                SpawnObject(point);
            }
            else
            {
                Debug.LogWarning("Не найдено свободное место для спавна.");
            }
        }
    }

}
