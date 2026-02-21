using System.Collections.Generic;
using System.Linq;
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
    [Inject] private WallsCollection _wallsCollection;
    [Inject] private SeedbedsCollection _seedbedsCollection;

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
        _dictionary[boughtItem1] = (boughtItem2, new PigBehaviour(boughtItem1.transform, GetWalls, GetSeedbeds, Trigger));
    }

    private void Trigger(PigView arg1, SeedbedView arg2)
    {
        if (SeedbedsToPigsMetrix.IsAbleToEat(arg1.Name, arg2.Name))
        {
            arg2.gameObject.SetActive(false);
        }
        else
        {
            arg1.gameObject.SetActive(false);

            _window.CreatePickableMoney(arg2.transform.position);
        }
    }

    private List<SeedbedView> GetSeedbeds()
    {
        return _seedbedsCollection.GetSeedbeds().ToList();
    }

    private List<WallView> GetWalls()
    {
        return  _wallsCollection.GetWalls().ToList();
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