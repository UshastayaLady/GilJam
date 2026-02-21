using UnityEngine;
using WebUtility;
using WebUtility.Data;

public class PigPresenter : IPresenter
{
    [Inject] private DIContainer _container;
    [Inject] private PigWindow _window;
    [Inject] private PigCollection _pigCollection;

    public void Init()
    {
        var pigs = DataConfigManager.GetAllDataOfType<PigData>();

        foreach (var data in pigs)
        {
            _pigCollection.AddPig(new PigModel(100, data.Price), new PigInfo(data.Prefab, data.Sprite));
        }
    }

    public void Exit()
    {
        
    }
}