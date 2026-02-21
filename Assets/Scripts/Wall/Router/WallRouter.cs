using UnityEngine;
using System.Collections.Generic;
using WebUtility;

public class WallRouter : IDIRouter
{
    [Inject] private DIContainer _container;

    public List<IPresenter> Init()
    {
        WallPresenter wallPresenter = _container.RegisterSingleton<WallPresenter>();
        _container.RegisterSingleton<WallsCollection>();
        
        return new List<IPresenter>()
        {
            wallPresenter
        };
    }
}