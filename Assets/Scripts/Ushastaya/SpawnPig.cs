using UnityEngine;

public class SpawnPig : SpavnParent
{
    private Camera cam;

    protected override void Awake()
    {
        base.Awake();
        cam = Camera.main;
    }

    public void OnClick(SpriteRenderer spriteRenderer, out SpriteRenderer created)
    {
        created = null;
        
        if (spriteRenderer != spawnPrefabView)
            return;
        
        Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 point = new Vector2(mouseWorld.x, mouseWorld.y);

        Collider2D hit = Physics2D.OverlapPoint(point);

        if (hit != null && hit.GetComponent<SpriteRenderer>() == spawnSquare)
        {
            spawnPoint = point;
            SpawnObject(spawnPoint);

            created = spawnObject;
        }

    }

}
