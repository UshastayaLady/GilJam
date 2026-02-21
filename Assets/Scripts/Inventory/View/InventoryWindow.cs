using UnityEngine;
using UnityEngine.UI;
using WebUtility;

public class InventoryWindow : AbstractWindowUi
{
    [SerializeField] private SlotView _prefab;
    [SerializeField] private Transform _parent;
    [SerializeField] private Image _mouseIcon;
    
    public Image MouseIcon => _mouseIcon;

    public override void Init()
    {
    }

    public SlotView Create()
    {
        SlotView slotView = Instantiate(_prefab, _parent);

        return slotView;
    }
}