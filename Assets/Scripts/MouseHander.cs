using UnityEngine;

public class MouseHander : MonoBehaviour
{
    [SerializeField] private SpriteRenderer objectOnMouse;
    

    public void SetObjectOnMouse(SpriteRenderer objectOnMouse)
    {
        this.objectOnMouse = objectOnMouse;
    }
    public SpriteRenderer GetObjectOnMouse()
    {
        return objectOnMouse;
    }
}
