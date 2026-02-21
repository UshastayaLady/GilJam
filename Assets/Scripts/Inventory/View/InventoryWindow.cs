using UnityEngine;
using WebUtility;

public class InventoryWindow : AbstractWindowUi
{
    [SerializeField] private SlotView _prefab;
    [SerializeField] private Transform _parent;
    
    public override void Init()
    {
    }

    public SlotView Create()
    {
        SlotView slotView = Instantiate(_prefab, _parent);

        return slotView;
    }
}