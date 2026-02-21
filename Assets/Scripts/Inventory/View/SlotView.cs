using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using UnityEngine.EventSystems;

public class SlotView : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _text;
    
    private readonly Subject<PointerEventData> _onBeginDragSubject = new Subject<PointerEventData>();
    private readonly Subject<PointerEventData> _onEndDragSubject = new Subject<PointerEventData>();
    private Image _image;

    // public IObservable<Unit> OnClicked => _button.OnClickAsObservable();
    public IObservable<PointerEventData> OnBeginDragged => _onBeginDragSubject;
    public IObservable<PointerEventData> OnEndDragged => _onEndDragSubject;

    private void Awake()
    {
        if (_image == null)
            _image = GetComponent<Image>();
            
        _image.raycastTarget = true;

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.LogError("OnBeginDrag");
        _onBeginDragSubject.OnNext(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _onEndDragSubject.OnNext(eventData);
    }

    public void UpdatePrice(int price)
    {
        _text.text = price.ToString();
    }

    public void UpdateIcon(Sprite sprite)
    {
        _icon.sprite = sprite;
    }

    private void OnDestroy()
    {
        _onBeginDragSubject?.Dispose();
        _onEndDragSubject?.Dispose();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UnityEngine.Debug.LogError("CLICKED!");
    }
}