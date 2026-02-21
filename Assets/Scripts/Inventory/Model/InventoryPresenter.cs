using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx; 
using WebUtility; 

public class InventoryPresenter : IPresenter, IDisposable
{
    [Inject] private DIContainer _container;
    [Inject] private InventoryWindow _window;
    [Inject] private PigCollection _pigCollection;
    [Inject] private MoneyModel _moneyModel;
    [Inject] private PaymentHandler _paymentHandler;

    private readonly CompositeDisposable _disposables = new CompositeDisposable();

    private DragAndDropModel _dragAndDropModel;

    public void Init()
    {
        _dragAndDropModel = new DragAndDropModel(_window.MouseIcon);
        
        foreach (var pigModel in _pigCollection.GetPigs())
        {
            SlotView slotView = _window.Create();
            slotView.UpdatePrice(pigModel.CurrentPrice.Value);
            slotView.UpdateIcon(_pigCollection.GetSpriteBy(pigModel));
            
            SubscribeToSlot(slotView, pigModel);
        }
        
        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                Update();
            })
            .AddTo(_disposables);

        _moneyModel.CurrentMoney.Subscribe(_ => { _window.DisplayMoney(_moneyModel.CurrentMoney.Value);});
    }

    private void SubscribeToSlot(SlotView slotView, PigModel pigModel)
    {
        // slotView.OnClicked
        //     .Subscribe(_ => {
        //         Buy(pigModel); 
        //     })
        //     .AddTo(_disposables); 
            
        slotView.OnBeginDragged  
            .Subscribe(_ => {
                BeginDrag(pigModel); 
            })
            .AddTo(_disposables); 
            
        slotView.OnEndDragged  
            .Subscribe(_ => {
                EndDrag();
                Buy(pigModel);
            })
            .AddTo(_disposables); 
    }

    private void EndDrag()
    {
        Debug.LogError("End drag!");

        _dragAndDropModel.Disable();
    }

    private void BeginDrag(PigModel pigModel)
    {
        Debug.LogError("DRAG!");
        _dragAndDropModel.Activate(_pigCollection.GetSpriteBy(pigModel));
    }

    private void Buy(PigModel pigModel) 
    {
        Debug.Log($"Покупка свинки за {pigModel.CurrentPrice.Value}");

        _paymentHandler.TryBuy(pigModel);
    }

    private void Update()
    {
        _dragAndDropModel.Update();
    }

    public void Dispose()
    {
        _disposables.Clear(); 
        _disposables?.Dispose();
    }
}