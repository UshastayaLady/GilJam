using UnityEngine;
using WebUtility;

public class PigWindow : AbstractWindowUi
{
    [SerializeField] private PickableMoney _pickableMoney;
    
    public override void Init()
    {
        
    }

    public void CreatePickableMoney(Vector3 transformPosition)
    {
        Instantiate(_pickableMoney, transformPosition, Quaternion.identity);
    }
}