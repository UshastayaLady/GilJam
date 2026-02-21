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
      //  OnClick();
    }

    public void OnClick()
    {
        Debug.LogError("ONCLICKEKa,,ra. ");
        SpriteRenderer objOnMouse = mouseHander.GetObjectOnMouse();
        Debug.LogError("Fkakf,ac");
        // if (objOnMouse == null)
        //     return;
        Debug.LogError("axxac,ac");

        // if (!objOnMouse.CompareTag("Player"))
        //     return;

        Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 point = new Vector2(mouseWorld.x, mouseWorld.y);

        Collider2D hit = Physics2D.OverlapPoint(point);
        Debug.LogError("2233,ac" + hit.GetComponent<SpriteRenderer>() + " " + spawnSquare);

        if (hit != null && hit.GetComponent<SpriteRenderer>() == spawnSquare)
        {
            Debug.LogError("65252,ac");

            spawnPoint = point;
            SpawnObject(spawnPoint);
        }


             

        
    }

}
