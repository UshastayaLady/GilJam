using System.Collections.Generic;
using UnityEngine;
using UniRx; 
using WebUtility; 

public class InventoryPresenter : IPresenter
{
    [Inject] private DIContainer _container;
    [Inject] private InventoryWindow _window;
    
    private readonly CompositeDisposable _disposables = new CompositeDisposable(); 

    public void Init()
    {
        foreach (var pigModel in GetPigs())
        {
            SlotView slotView = _window.Create();
            slotView.UpdatePrice(pigModel.CurrentPrice.Value);
            
            slotView.OnClicked
                .Subscribe(_ => {
                    Buy(pigModel); 
                })
                .AddTo(_disposables); 
        }
    }

    private void Buy(PigModel pigModel) 
    {
        Debug.Log($"Покупка свинки за {pigModel.CurrentPrice.Value}");
    }

    private IEnumerable<PigModel> GetPigs()
    {
        return new List<PigModel>()
        {
            new PigModel(100, 10),
            new PigModel(100, 20)
        };
    }

    public void Exit()
    {
        _disposables.Clear(); 
    }
}