using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NewMonoBehaviourScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image _image;
    
    private void Awake()
    {
        if (_image == null)
            _image = GetComponent<Image>();
            
        // Обязательно включаем raycast target
        _image.raycastTarget = true;
        
        Debug.Log($"TestDrag инициализирован на {gameObject.name}");
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.LogError("🎯 OnBeginDrag СРАБОТАЛ!");
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("👉 OnDrag");
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.LogError("✅ OnEndDrag СРАБОТАЛ!");
    }

}
