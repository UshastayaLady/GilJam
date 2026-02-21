using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using WebUtility;

public class DragAndDropModel 
{
    private readonly Image _icon;
    private readonly Canvas _canvas;
    private bool _isWorking;
    
    public DragAndDropModel(Image icon)
    {
        _icon = icon;
        _canvas = _icon.GetComponentInParent<Canvas>();
    }

    public void Activate(Sprite sprite)
    {
        _isWorking = true;
        _icon.sprite = sprite;
        _icon.gameObject.SetActive(true);
        _canvas.enabled = true;
    }   
    
    public void Disable()
    {
        _isWorking = false;
        _icon.gameObject.SetActive(false);
        _canvas.enabled = false;
    }

    public void Update()
    {
        if (_isWorking == false)
        {
            return;
        }
        
        _icon.transform.position = Input.mousePosition;
    }
}
