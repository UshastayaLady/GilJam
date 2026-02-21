using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using WebUtility;
using WebUtility.Data;

public class PigPresenter : IPresenter
{
    [Inject] private DIContainer _container;
    [Inject] private PigWindow _window;
    [Inject] private PigCollection _pigCollection;

    private Dictionary<PigView, (PigModel, PigBehaviour)> _dictionary = new();

    private readonly CompositeDisposable _disposables = new CompositeDisposable();

    public void Init()
    {
        var pigs = DataConfigManager.GetAllDataOfType<PigData>();

        foreach (var data in pigs)
        {
            _pigCollection.AddPig(new PigModel(100, data.Price), new PigInfo(data.Prefab, data.Sprite));
        }

        _pigCollection.OnSpawned.Subscribe(bought => {
                Spawn(bought.Item1, bought.Item2); 
            })
            .AddTo(_disposables); 

        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                Update();
            })
            .AddTo(_disposables);
    }

    private void Spawn(PigView boughtItem1, PigModel boughtItem2)
    {
        _dictionary[boughtItem1] = (boughtItem2, new PigBehaviour(boughtItem1.transform));
    }

    private void Update()
    {
        foreach (var item in _dictionary)
        {
            item.Value.Item2.Update();
        }
    }

    public void Exit()
    {
        
    }
}