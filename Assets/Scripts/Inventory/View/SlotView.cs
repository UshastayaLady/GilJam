using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class SlotView : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _text;
    
    public IObservable<Unit> OnClicked => _button.OnClickAsObservable();

    public void UpdatePrice(int price)
    {
        _text.text = price.ToString();
    }
}