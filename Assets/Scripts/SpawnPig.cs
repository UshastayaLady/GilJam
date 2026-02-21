using UnityEngine;

public class SpawnPig : SpavnParent
{
    private Camera cam;
    [SerializeField] private MouseHander mouseHander;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        OnClick();
    }

    private void OnClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SpriteRenderer objOnMouse = mouseHander.GetObjectOnMouse();
            if (objOnMouse == null)
                return;

            if (!objOnMouse.CompareTag("Player"))
                return;

            Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 point = new Vector2(mouseWorld.x, mouseWorld.y);

            Collider2D hit = Physics2D.OverlapPoint(point);

            if (hit != null && hit == spawnSquare)
            {
                spawnPoint = point;
                SpawnObject(spawnPoint);
            }
        }
             

        
    }

}
